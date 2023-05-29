
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyAspNetCore.Web.ViewModels;
using MyAspNetCore.Web.Models;

namespace MyAspNetCore.Web.Filters
{
	public class NotFoundFilter:ActionFilterAttribute
	{   private readonly AppDbContext _appDbContext;

        public NotFoundFilter(AppDbContext appcontext)
        {
            _appDbContext = appcontext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
		{
			var valueId = context.ActionArguments.Values.First(); // bu yapı att yazıldığı action methodun parametresindeki ilk değer ne ise onu alır ve geriye bir nesne dönderir onu biz int yapalım.

			var id = (int)valueId;

			var hasproduct = _appDbContext.Products.Any(x => x.Id == id);

			if (hasproduct == false)
			{
				context.Result = new RedirectToActionResult("Error", "Home",new ErrorViewModel() { Errors = new List<string>() { $"Mevcut olan id numarası: {id} olan bir ürün bulunamamıştır."} }); 
				// bu yapı ile istediğimiz bir sayfaya sonuç dönebiliriz. ilk parametre action ikinici cont üçüncü taşınan değer
			}
		}
	}
}
