using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWebApi.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public int ShoppingCartId { get; set; }  // Foreign Key to ShoppingCart
        public int ProductId { get; set; }  // Foreign Key to Product
        public int Quantity { get; set; }

        // Navigation properties
        [ForeignKey("ShoppingCartId")]
        public ShoppingCart ShoppingCart { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
