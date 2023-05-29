using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyAspNetCore.Web.Filters;
using MyAspNetCore.Web.Helpers;
using MyAspNetCore.Web.Models;
using MyAspNetCore.Web.ViewModels;


//DI singletonla uygulamak için öncelikle dışardan ınterface tanımlamak lazım
namespace MyAspNetCore.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class ProductsController : Controller
	{
		private readonly AppDbContext _appContext;
		private readonly IHelper _helper; // burada tanımlama yaptık şimdi bunu constructorda gerçekleme yapmamız gerek.
		private readonly IMapper _mapper;
		private readonly IFileProvider _fileProvider;
		public ProductsController(AppDbContext appContext, IHelper helper, IMapper mapper,IFileProvider provider)
		{   //DI Container
			_appContext = appContext;
			_helper = helper; // burada gerçekleme yaptık fakat bunun bir singleton yaşam döngüsüne sahip olması gerektiğini belirtmedik onuda program.cs içinde yaparız böylece bu nesneden bir kez üretilmiş olur.
			_mapper = mapper;
			_fileProvider = provider;

		}

		public IActionResult Index([FromServices]IHelper helper2)// bunu dışarda tanımladığımız _helper ile gerçekleme yapıp nesne üretebiliriz.
		{
			List<ProductViewModel> products = _appContext.Products.Include(x => x.Category).Select(x => new ProductViewModel()
			{
				Id = x.Id,
				Name = x.Name,
				Price = x.Price,
				Stock = x.Stock,
				ImagePath = x.ImagePath,
				CategoryName= x.Category.Name
				
			}).ToList(); //burda include ile şunu amaçlarız Category tablosundan eleman içeren yeni bir View Model oluştur deriz. Bir nevi tablo birleştirme işlemine tabi tutarız. Bu tip durumlarda map kullanmaya gerek yoktur onun yerine böyle bir işlem yapabiliriz.
			ViewData["Count"] = products.Count;
			ViewData["UpperText"] = _helper.Upper("asp.net core app");

			return View(products);
		}


		[HttpGet]
		[ServiceFilter(typeof(NotFoundFilter))]
        [Route("urunler/urun/{productid}", Name = "product")]
        public IActionResult GetById(int productid)
		{
			var product = _appContext.Products.Find(productid);
			return View(_mapper.Map<ProductViewModel>(product));
		}


        [Route("[controller]/[action]/{page}/{pageSize}", Name = "productpage")]
        [HttpGet]
		public IActionResult Pages(int page, int pageSize)
		{

			var products = _appContext.Products.Skip((page - 1)*pageSize).Take(pageSize).ToList();
			ViewBag.page = page;
			ViewBag.pageSize = pageSize;
			ViewData["Count"] = products.Count;
			return View(_mapper.Map<List<ProductViewModel>>(products));
		}


		[HttpGet]
		public IActionResult Add()
		{
            var categories = _appContext.Category.ToList();
            ViewBag.CategorySelect = new SelectList(categories, "Id", "Name");

            return View();
		}



		[HttpPost]
		public IActionResult Add(ProductViewModel newProduct) // 3. yöntem req.body gelecek class nesnesi ile
		{
			// 1.yöntem model binding 
			//var name = HttpContext.Request.Form["Name"].ToString();
			//var price = decimal.Parse(HttpContext.Request.Form["Price"].ToString());
			//var stock = int.Parse(HttpContext.Request.Form["Stock"].ToString());
			//var newProduct = new Product() { Name=Name, Price=Price, Stock=Stock}; // 2.Yöntem Parametre ile gelecek req.body içinden karşılık gelen ifadeleri mapleme yapabiliriz.

			if (ModelState.IsValid)
			{
				try
				{
					var product = _mapper.Map<Product>(newProduct);
					if(newProduct.Image!=null && newProduct.Image.Length > 0)
					{
						var root = _fileProvider.GetDirectoryContents("wwwroot");

						var images = root.First(x => x.Name == "images");

						var randomImageName = Guid.NewGuid() + Path.GetExtension(newProduct.Image.FileName); // bu resimlerin birbirini ezmesini önlemek için random string üretir.

						var path = Path.Combine(images.PhysicalPath,randomImageName);

						using var stream = new FileStream(path, FileMode.Create);

						newProduct.Image.CopyTo(stream);

					    product.ImagePath = randomImageName;



					}
                    _appContext.Products.Add(product); // product viewmodeli alıp product nesnesine dönüştürme işlemi gerçekleşir.
					_appContext.SaveChanges();

					TempData["status"] = "Ürün başarıyla eklendi.";
					return RedirectToAction("Index");
				} 
				catch (Exception ex)
				{
                    return View(ex.Message);
				}
			}

		    var categories = _appContext.Category.ToList();

			ViewBag.CategorySelect = new SelectList(categories, "Id", "Name");

			return RedirectToAction("Add");
			
		}

        [AcceptVerbs("GET", "POST")]
        public IActionResult Hasname(string Name)
		{ 
			var ct = _appContext.Products.Any(p => p.Name == Name);

			if(ct)
			{
				return Json("Bu isim daha önce kayıt edilmiş.");
			}
			else
			{
				return Json(true);
			}

		}
		[ServiceFilter(typeof(NotFoundFilter))]
        [HttpGet("{id}")]
        public IActionResult Remove(int id)
		{
			var product = _appContext.Products.Find(id);
			_appContext.Products.Remove(product);
			_appContext.SaveChanges();
			TempData["status"] = "Ürün başarıyla silindi.";
			return RedirectToAction("Index");
		}

        [HttpGet]
		[ServiceFilter(typeof(NotFoundFilter))]
		public IActionResult Update(int productid)
        {       
			var product = _appContext.Products.First(P => P.Id == productid);

            return View(_mapper.Map<ProductUpdateViewModel>(product));
        }

        [HttpPost]
        public IActionResult Update(ProductUpdateViewModel updateProduct)
        {
           
            if (updateProduct.Image != null && updateProduct.Image.Length > 0)
            {
                var root = _fileProvider.GetDirectoryContents("wwwroot");

                var images = root.First(x => x.Name == "images");


                var randomImageName = Guid.NewGuid() + Path.GetExtension(updateProduct.Image.FileName);


                var path = Path.Combine(images.PhysicalPath, randomImageName);


                using var stream = new FileStream(path, FileMode.Create);


                updateProduct.Image.CopyTo(stream);

                updateProduct.ImagePath = randomImageName;
            }

			var product = _mapper.Map<Product>(updateProduct);
            _appContext.Products.Update(product);
            _appContext.SaveChanges();
            TempData["status"] = "Ürün başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
        public IActionResult Back()
		{


			return RedirectToAction("Index");

		}
	}
}
