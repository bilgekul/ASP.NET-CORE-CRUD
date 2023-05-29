using AutoMapper;
using MyAspNetCore.Web.Models;
using MyAspNetCore.Web.ViewModels;
using MyAspNetCore.Web.ViewModels;

namespace MyAspNetCore.Web.Mapping
{
	public class ViewModelMapping:Profile
	{
        public ViewModelMapping()
        {
            CreateMap<Product,ProductViewModel>().ReverseMap();
            CreateMap<Product,ProductUpdateViewModel>().ReverseMap();

        }
    }
}
