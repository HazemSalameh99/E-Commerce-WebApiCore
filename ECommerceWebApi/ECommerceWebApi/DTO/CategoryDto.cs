using ECommerceWebApi.Models;

namespace ECommerceWebApi.DTO
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string Discription { get; set; }
        //Navigation Property
        public List<ProductDto>? Products { get; set; }=new List<ProductDto>();
    }
}
