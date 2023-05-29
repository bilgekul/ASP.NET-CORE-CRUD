using Microsoft.AspNetCore.Mvc.Filters;

namespace MyAspNetCore.Web.Filters
{
	public class LogFilter:ActionFilterAttribute

	{
		public override void OnActionExecuting(ActionExecutingContext context) // action method çalışmadan önce
		{
			base.OnActionExecuting(context);
		}

		public override void OnActionExecuted(ActionExecutedContext context) // action method çalıştıktan sonra
		{
			base.OnActionExecuted(context);
		}

		public override void OnResultExecuting(ResultExecutingContext context) // sonuç üretilmeden önce 
		{
			base.OnResultExecuting(context);
		}

		public override void OnResultExecuted(ResultExecutedContext context) // sonuç üretildikten sonra
		{
			base.OnResultExecuted(context);
		}


	}
}
