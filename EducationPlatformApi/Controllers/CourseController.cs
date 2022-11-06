using EducationPlatformApi.Data;
using EducationPlatformApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EducationPlatformApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CourseController : ControllerBase
    {
        private readonly EducationPlatformApiContext _context;

        public CourseController(EducationPlatformApiContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Course>>> Get([FromQuery] int? categoryId)
        {
            var courses = _context.Courses.AsQueryable();
            if (categoryId.HasValue)
            {
                courses = courses.Where(p => p.CategoryId == categoryId);
            }

            return Ok(await courses.AsNoTracking().ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Course>> Get(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound("course not found");
            return Ok(course);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<List<Course>>> Add(AddCourseRequest addCourseRequest)
        {
            var category = await _context.Categories.FindAsync(addCourseRequest.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError(nameof(addCourseRequest.CategoryId), "no category with this id");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var course = new Course()
            {
                Name = addCourseRequest.Name,
                CategoryId = addCourseRequest.CategoryId,
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<List<Course>>> Update(int id, UpdateCourseRequest updateCourseRequest)
        {
            var dbCourse = await _context.Courses.FindAsync(id);
            if (dbCourse == null)
                return NotFound("course not found");

            var category = await _context.Categories.FindAsync(updateCourseRequest.CategoryId);
            if (category == null)
                ModelState.AddModelError(nameof(updateCourseRequest.CategoryId), "no category with this id");

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            dbCourse.Name = updateCourseRequest.Name;
            dbCourse.CategoryId = updateCourseRequest.CategoryId;   

            await _context.SaveChangesAsync();

            return Ok(dbCourse);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Course>>> Delete(int id)
        {
            var dbCourse = await _context.Courses.FindAsync(id);
            if (dbCourse == null)
                return NotFound("course not found");

            _context.Courses.Remove(dbCourse);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
