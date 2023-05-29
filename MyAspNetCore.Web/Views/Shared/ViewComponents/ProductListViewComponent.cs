using Microsoft.AspNetCore.Mvc;
using MyAspNetCore.Web.Models;
using MyAspNetCore.Web.ViewModels;

namespace MyAspNetCore.Web.Views.Shared.ViewComponents
{
    public class ProductListViewComponent:ViewComponent
    {   private readonly AppDbContext _context;

        public ProductListViewComponent( AppDbContext context)
        {
            _context = context; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewmodels = _context.Products.Select(x => new ProductListComponentViewModel() { Name = x.Name, Stock = x.Stock }).ToList();

            return View(viewmodels);
           
        }
    }
}
