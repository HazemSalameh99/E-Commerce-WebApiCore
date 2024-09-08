using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWebApi.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }//Foreign Ke to "ApplicationUser" Many to One
        public double Total { get; set; }

        //Navigation Properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
