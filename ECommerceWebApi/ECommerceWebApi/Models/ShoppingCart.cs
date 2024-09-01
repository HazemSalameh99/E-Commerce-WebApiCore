namespace ECommerceWebApi.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }//Foreign Ke to "ApplicationUser" Many to One


        //Navigation Properties
        public ApplicationUser User { get; set; }

    }
}
