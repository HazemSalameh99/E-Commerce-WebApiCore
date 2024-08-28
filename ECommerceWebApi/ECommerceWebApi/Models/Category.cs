namespace ECommerceWebApi.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public ICollection<Product> Products { get; set; }


    }
}
