using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MyAspNetCore.Web.ViewModels
{
	public class ProductViewModel
	{
		public int Id { get; set; }

        [Remote(action: "Hasname", controller: "Products")]
        [StringLength(50, ErrorMessage = "isim alanına en fazla 50 karakter girilebilir.")]
        [Required(ErrorMessage = "İsim alanı boş olamaz.")]
        public string? Name { get; set; } = null!;

        //[RegularExpression(@"^[0-9]+(\,[0-9]{1,2})", ErrorMessage = "Fiyat alanınıda virgülden sonra en fazla 2 basamak olmalıdır.")]
        [Range(1, 1000, ErrorMessage = "Fiyat alanı 1 ile 1000 arasında bir değer olmalıdır.")]
        [Required(ErrorMessage = "Fiyat alanı boş olamaz.")]
        public decimal Price { get; set; }



        [Required(ErrorMessage = "Stok alanı boş olamaz.")]
        [Range(1, 200, ErrorMessage = "Stok alanı 1 ile 200 arasında bir değer olmalıdır.")]
        public int Stock { get; set; }

        public IFormFile? Image { get; set; } //http req gelen file nesnesidir.
		[ValidateNever]
		public string? ImagePath { get; set; } // dbden gelecek file pathini tutar.

        [Required(ErrorMessage ="Kategori seçimi boş olamaz.")]
        public int CategoryId { get; set; } // comboboxa gelen category id yi tutar.

        public string? CategoryName { get; set; }
	}
}
