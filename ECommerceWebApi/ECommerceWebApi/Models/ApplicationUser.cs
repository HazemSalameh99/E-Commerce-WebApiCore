using Microsoft.AspNetCore.Identity;

namespace ECommerceWebApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public DateTime LastName { get; set; }
        public ICollection<Order> Orders { get; set; }


    }
}
