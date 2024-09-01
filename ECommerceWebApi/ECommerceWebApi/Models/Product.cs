using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWebApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; } //Foreign Key to "Categories" (Many to One)

        //Navigation Property
        public Category Category { get; set; }
        //public ICollection<OrderItem> OrderItems { get; set; }




    }
}
