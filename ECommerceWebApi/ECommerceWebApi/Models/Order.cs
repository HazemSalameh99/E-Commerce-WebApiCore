namespace ECommerceWebApi.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; } //Foreign Key to "ApplicationUser" Many to One
        public int PaymentId { get; set; }
        public int ShippingAddressId { get; set; }

        //Navigation Properties
        public ApplicationUser User { get; set; }
        public Payment Payment { get; set; }//One to One 
        public ShippingAddress shippingAddress { get; set; }//One to One
        public ICollection<OrderItem> OrderItems { get; set; }//One to Many


    }
}
