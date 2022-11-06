using EducationPlatformApi.Data;
using EducationPlatformApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace EducationPlatformApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CategoryController : ControllerBase
    {
        private readonly EducationPlatformApiContext _context;

        public CategoryController(EducationPlatformApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return Ok(await _context.Categories.AsNoTracking().ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound("category not found");
            return Ok(category);
        }

        [Authorize(Roles = "Trainer")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> Add(AddCategoryRequest addCategoryRequest)
        {
            var category = new Category()
            {
                Name = addCategoryRequest.Name,
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Category>>> Update(int id, UpdateCategoryRequest updateCategoryRequest)
        {
            var dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null)
                return NotFound("category not found");

            dbCategory.Name = updateCategoryRequest.Name;

            await _context.SaveChangesAsync();

            return Ok(dbCategory);
        }

        [Authorize(Roles = "Trainer")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Category>>> Delete(int id)
        {
            var dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null)
                return NotFound("category not found");

            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
