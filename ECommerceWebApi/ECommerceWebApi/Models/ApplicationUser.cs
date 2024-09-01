using Microsoft.AspNetCore.Identity;

namespace ECommerceWebApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public DateTime LastName { get; set; }

        //Navigation Properties
        public ICollection<Order> Orders { get; set; } //One to Many 
        public ShoppingCart ShoppingCart { get; set; } //One to One
        public ICollection<ShippingAddress> ShippingAddress { get; set; } //One to Many



    }
}
