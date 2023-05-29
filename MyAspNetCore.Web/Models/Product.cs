using System.ComponentModel.DataAnnotations.Schema;

namespace MyAspNetCore.Web.Models;

	public class Product
	{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImagePath { get; set; }

    [ForeignKey("CategoryId")]
    public int CategoryId { get; set; } // her product bir category id si vardır.
    public Category Category { get; set; } // birden çoka ilişkiyi sağlar
}
