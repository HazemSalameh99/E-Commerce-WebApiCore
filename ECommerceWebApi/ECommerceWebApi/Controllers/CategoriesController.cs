using ECommerceWebApi.Data;
using ECommerceWebApi.DTO;
using ECommerceWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ECommerceDbContext _context;
        public CategoriesController(ECommerceDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> AllCategories()
        {
            var categories = await _context.Categories
                .ToListAsync();
            if(categories.Any())
            {
                return Ok(categories);
            }
            return NotFound("There are no Categories.");
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> CategoryById(int? id)
        {
            if (id != null)
            {
                var category=await _context.Categories
                    .SingleOrDefaultAsync(c=>c.CategoryId == id);
                if(category != null)
                {
                    return Ok(category);
                }
                return NotFound($"Category with ID {id} not found");
            }
            return BadRequest("Category ID cannot be null.");
        }
        [HttpGet("[Action]/{id:int}")]
        public async Task<IActionResult> CategoryWithProductById(int? id)
        {
            if (id != null)
            {
                var category = await _context.Categories
                    .Include(c=>c.Products)
                    .SingleOrDefaultAsync(c => c.CategoryId == id);
                if (category != null)
                {
                    CategoryDto categoryDto=new CategoryDto();
                    categoryDto.Name= category.Name;
                    categoryDto.Discription= category.Discription;
                    if (category.Products.Any())
                    {
                        foreach (var product in category.Products)
                        {
                            categoryDto.Products.Add(new ProductDto
                            {
                                Name = product.Name,
                                Discription = product.Discription,
                                Price = product.Price,
                                StockQuantity = product.StockQuantity,
                                Image = product.Image,
                            });
                        }
                        return Ok(categoryDto);
                    }
                    return BadRequest($"The {category.Name} is Not Contain On Products");
                }
                return NotFound($"Category with ID {id} not found");
            }
            return BadRequest("Category ID cannot be null.");
        }
        [HttpGet("{Name:alpha}")]
        public async Task<IActionResult> CategoriesByName(string Name)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var Categories = await _context.Categories
                    .Where(c=>c.Name.Contains(Name))
                    .ToListAsync();
                if (Categories.Any())
                {
                    return Ok(Categories);
                }
                return NotFound($"No Category Found With Name Containing '{Name}'");
            }
            return BadRequest("Category name cannot be null or empty");
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
        {
            if(ModelState.IsValid)
            {
                Category category = new Category
                {
                    Name = categoryDto.Name,
                    Discription = categoryDto.Discription,
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return CreatedAtAction("CategoryById", new { id = category.CategoryId }, category);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditCategory(int? id, CategoryDto categoryDto)
        {
            if (id == null)
            {
                return NotFound(string.Empty);
            }
            if (ModelState.IsValid)
            {
                var category = _context.Categories.Find(id);
                if (category != null)
                {
                    category.Name = categoryDto.Name;
                    category.Discription = categoryDto.Discription;
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("CategoryById", new { id = category.CategoryId }, category);
                }
                return NotFound($"Category with ID {id} not found");
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.CategoryId == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                try
                {
                    _context.SaveChanges();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest($"Category with ID {id} not found");
        }

    }
}
