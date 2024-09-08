using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWebApi.Models
{
    public class ShippingAddress
    {
        public int ShippingAddressId { get; set; }
        public string UserId { get; set; }//Foreign Key to "ApplicaionUser" Many to One
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        //Navigation Properties
        public Order Order { get; set; }//One to One
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
