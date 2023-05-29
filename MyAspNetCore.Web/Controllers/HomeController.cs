using Microsoft.AspNetCore.Mvc;
using MyAspNetCore.Web.Filters;
using MyAspNetCore.Web.Models;
using MyAspNetCore.Web.ViewModels;
using System.Diagnostics;

namespace MyAspNetCore.Web.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _appContext;

		public HomeController(ILogger<HomeController> logger, AppDbContext appContext)
		{
			_logger = logger;
			_appContext = appContext;
		}

		public IActionResult Index()
		{
			var partialProduct = _appContext.Products.OrderByDescending(x => x.Id).Select(x => new ProductPartialViewModel(){ Id = x.Id, Name = x.Name, Price = x.Price}).ToList();
			ViewBag.partialViewListModel = new ProductListPartialViewModel() { Products = partialProduct };	
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(ErrorViewModel e)
		{
			e.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
			return View(e);
		}
	}
}