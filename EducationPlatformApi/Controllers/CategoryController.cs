using EducationPlatformApi.Data;
using EducationPlatformApi.Models;
using EducationPlatformApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using EducationPlatformApi.Helpers;
using EducationPlatformApi.Responses;

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
        public async Task<ActionResult<CategoryListResponse>> Get()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return Ok(new CategoryListResponse()
            {
                Status = StatusType.Success.Value,
                Data = categories,
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> Get(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new CategoryResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "category not found"
                });
            return Ok(new CategoryResponse()
            {
                Status = StatusType.Success.Value,
                Data = category,
            });
        }

        [Authorize(Roles = "Trainer")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CategoryResponse>> Add(CategoryRequest addCategoryRequest)
        {
            var category = new Category()
            {
                Name = addCategoryRequest.Name,
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new CategoryResponse()
            {
                Status = StatusType.Success.Value,
                Data = category,
            });
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> Update(int id, CategoryRequest updateCategoryRequest)
        {
            var dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null)
                return NotFound(new CategoryResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "category not found"
                });

            dbCategory.Name = updateCategoryRequest.Name;

            await _context.SaveChangesAsync();

            return Ok(new CategoryResponse()
            {
                Status = StatusType.Success.Value,
                Data = dbCategory,
            });
        }

        [Authorize(Roles = "Trainer")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> Delete(int id)
        {
            var dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null)
                return NotFound(new CategoryResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "category not found"
                });

            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();

            return Ok(new CategoryResponse()
            {
                Status = StatusType.Success.Value
            });
        }
    }
}
