using ECommerceWebApi.Data;
using ECommerceWebApi.DTO;
using ECommerceWebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace ECommerceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ECommerceDbContext _context;
        IWebHostEnvironment _environment;
        public ProductsController(ECommerceDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpGet]
        public async Task<IActionResult> AllProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
            if (products.Any())
            {
                var productsDto = products.Select(p => new ProductDto
                {
                    Name = p.Name,
                    Discription = p.Discription,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Image = p.Image,
                    CategoryId = p.CategoryId
                }).ToList();

                return Ok(productsDto);
            }
            return NotFound("There are no products.");
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ProductById(int? id)
        {
            if (id != null)
            {
                var product = await _context.Products
                    .Include(c => c.Category)
                    .SingleOrDefaultAsync(p => p.ProductId == id);
                if (product != null)
                {
                    ProductDto productDto = new ProductDto
                    {
                        Name = product.Name,
                        Discription = product.Discription,
                        Price = product.Price,
                        StockQuantity = product.StockQuantity,
                        Image = product.Image,
                        CategoryId = product.CategoryId
                    };
                    return Ok(productDto);
                }
                return NotFound($"Product with ID {id} not found.");
            }
            return BadRequest("Product ID cannot be null.");
        }
        //[HttpGet("[Action]/{id:int}")]
        //public async Task<IActionResult> ProductsWithCategoryById(int? id)
        //{
        //    if (id != null)
        //    {
        //        var products = await _context.Products
        //            .Include(c => c.Category)
        //            .Where(p => p.CategoryId == id).ToListAsync();
        //        if (products.Any())
        //        {
        //            var productdto = products.Select(p => new ProductDto
        //            {
        //                Name = p.Name,
        //                Discription = p.Discription,
        //                Price = p.Price,
        //                StockQuantity = p.StockQuantity,
        //                Image = p.Image

        //            });
        //            return Ok(productdto);
        //        }
        //        return NotFound($"Category with ID {id} not found.");
        //    }
        //    return BadRequest("Product ID cannot be null.");
        //}
        [HttpGet("{Name:alpha}")]
        public async Task<IActionResult> ProductByName(string? Name)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Name.Contains(Name))
                    .ToListAsync();
                if (products.Any())
                {
                    var productDto = products.Select(p => new ProductDto
                    {
                        Name = p.Name,
                        Discription = p.Discription,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        Image = p.Image,
                        CategoryId = p.CategoryId
                    }).ToList();
                    return Ok(productDto);
                }
                return NotFound($"No Products Found With Name Containing '{Name}'");
            }
            return BadRequest("Product name cannot be null or empty");
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto)
        {
            if (productDto.ProductImage == null || productDto.ProductImage.Length == 0)
            {
                return BadRequest("No Image File Provided");
            }
            if (ModelState.IsValid)
            {
                string fileName = UploadNewFile(productDto);
                Product product = new Product
                {
                    Name = productDto.Name,
                    Discription = productDto.Discription,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity,
                    CategoryId = productDto.CategoryId,
                    Image = fileName
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction("ProductById", new { id = product.ProductId }, product);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditProduct(int? id, [FromForm] ProductDto productdto)
        {
            if (id != null)
            {
                if (ModelState.IsValid)
                {
                    string fileName = UploadNewFile(productdto);
                    var _product = _context.Products.Find(id);
                    if (_product != null)
                    {
                        _product.Name = productdto.Name;
                        _product.Discription = productdto.Discription;
                        _product.Price = productdto.Price;
                        _product.StockQuantity = productdto.StockQuantity;
                        if (productdto.ProductImage != null)
                        {
                            if (_product.Image != null)
                            {
                                string rootPath = Path.Combine(_environment.WebRootPath, "images", _product.Image);
                                System.IO.File.Delete(rootPath);
                            }
                            _product.Image = fileName;
                        }
                        try
                        {
                            _product.CategoryId = productdto.CategoryId;
                        }
                        catch (Exception E)
                        {
                            return BadRequest(E.Message);
                        }
                        try
                        {
                            _context.Products.Update(_product);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception E)
                        {
                            return BadRequest(E.Message);
                        }
                        return CreatedAtAction("ProductById", new { id = _product.ProductId }, _product);
                    }
                    return NotFound($"Product with ID {id} not found ");
                }
                return BadRequest(ModelState);
            }
            return NotFound("Product ID cannot be null.");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                try
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return NotFound($"This Id {id} is Not Found");
        }
        [NonAction]
        public string UploadNewFile(ProductDto dto)
        {
            string newFullImgName = null;
            if (dto.ProductImage != null)
            {
                string fileRoot = Path.Combine(_environment.WebRootPath, "images");
                //asld544-asdcd755-ss544-_123.JPJ 
                newFullImgName = Guid.NewGuid().ToString() + "-" + dto.ProductImage.FileName;//,anx1445+_dog
                string fullPath = Path.Combine(fileRoot, newFullImgName);
                using (var myNewFile = new FileStream(fullPath, FileMode.Create))
                {
                    dto.ProductImage.CopyTo(myNewFile);
                }
            }
            return newFullImgName;
        }
    }
}
