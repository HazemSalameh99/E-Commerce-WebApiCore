using ECommerceWebApi.Data;
using ECommerceWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace ECommerceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ECommerceDbContext _context;
        public ProductsController(ECommerceDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> AllProducts()
        {
            var products = await _context.Products
                .Include(p=>p.Category)
                .ToListAsync();
            if (products.Any())
            {
                return Ok(products);
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
                    return Ok(product);
                }
                return NotFound($"Product with ID {id} not found.");
            }
            return BadRequest("Product ID cannot be null.");
        }
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
                    return Ok(products);
                }
                return NotFound($"No Products Found With Name Containing '{Name}'");
            }
            return BadRequest("Product name cannot be null or empty");
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction("ProductById", new { id = product.ProductId }, product);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditProduct(int? id, Product product)
        {
            if (id != null)
            {
                if (ModelState.IsValid)
                {
                    var _product = _context.Products.Find(id);
                    if (_product != null)
                    {
                        _product.Name = product.Name;
                        _product.Discription = product.Discription;
                        _product.Price = product.Price;
                        _product.StockQuantity = product.StockQuantity;
                        _product.Image = product.Image;
                        _product.CategoryId = product.CategoryId;
                        _product.Category = product.Category;
                        await _context.SaveChangesAsync();
                        return CreatedAtAction("ProductById", new { id = _product.ProductId }, _product);
                    }
                    return NotFound($"Product with ID {id} not found ");
                }
                return BadRequest(ModelState);
            }
            return NotFound("Product ID cannot be null.");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult>DeleteProduct(int id)
        {
            var product=_context.Products.SingleOrDefault(p=>p.ProductId==id);
            if (product != null)
            {
                try
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (Exception e) {
                    return BadRequest(e.Message);
                }
            }
            return NotFound($"This Id {id} is Not Found");
        }
    }
}
