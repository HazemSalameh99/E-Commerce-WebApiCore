namespace ECommerceWebApi.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }//Foreign Key to "Order" Many to One
        public int ProductId { get; set; }//Foreign Key to "Product" Many to One
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        //Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
