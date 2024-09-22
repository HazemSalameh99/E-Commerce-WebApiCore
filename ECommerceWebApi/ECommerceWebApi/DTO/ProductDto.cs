using ECommerceWebApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerceWebApi.DTO
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Discription { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Image { get; set; }
        public IFormFile ProductImage { get; set; }
        public int CategoryId { get; set; } //Foreign Key to "Categories" (Many to One)


    }
}
