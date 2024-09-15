using ECommerceWebApi.Data;
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
                .Include(c=>c.Products)
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
                    .Include(c=>c.Products)
                    .SingleOrDefaultAsync(c=>c.CategoryId == id);
                if(category != null)
                {
                    return Ok(category);
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
                    .Include(c => c.Products)
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
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return CreatedAtAction("CategoryById", new { id = category.CategoryId }, category);
            }
            return BadRequest(ModelState);
        }

    }
}
