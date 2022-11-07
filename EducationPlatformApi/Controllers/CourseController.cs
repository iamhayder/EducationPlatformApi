using EducationPlatformApi.Data;
using EducationPlatformApi.Helpers;
using EducationPlatformApi.Models;
using EducationPlatformApi.Requests;
using EducationPlatformApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;


namespace EducationPlatformApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CourseController : ControllerBase
    {
        private readonly EducationPlatformApiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;


        public CourseController(EducationPlatformApiContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CourseListResponse>> Get([FromQuery] int? categoryId)
        {
            var coursesQuery = _context.Courses.AsQueryable();
            if (categoryId.HasValue)
            {
                coursesQuery = coursesQuery.Where(p => p.CategoryId == categoryId);
            }
            var courses = await coursesQuery.AsNoTracking().ToListAsync();
            return Ok(new CourseListResponse()
            {
                Status = StatusType.Success.Value,
                Data = courses
            });
        }

        [HttpGet("{id}")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseResponse>> Get(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new CourseResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "course not found"
                });
            return Ok(new CourseResponse()
            {
                Status = StatusType.Success.Value,
                Data = course,
            });
        }

        [Authorize(Roles = "Trainer")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<CourseResponse>> Add([FromForm] CourseRequest addCourseRequest)
        {
            var category = await _context.Categories.FindAsync(addCourseRequest.CategoryId);
            var user = await _userManager.GetUserAsync(User);
            if (category == null)
            {
                ModelState.AddModelError(nameof(addCourseRequest.CategoryId), "no category with this id");
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(new CourseResponse()
                {
                    Status = StatusType.Error.Value,
                    ModelError = ModelState
                });
            }
            var course = new Course()
            {
                Title = addCourseRequest.Title,
                Description = addCourseRequest.Description,
                Content = addCourseRequest.Content,
                Category = category!,
                TrainerUserId = new Guid(user.Id)
            };
            if (addCourseRequest.Image != null)
            {
                if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                {
                    _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }
                var uniqueFileName = FileHelper.GetUniqueFileName(addCourseRequest.Image.FileName);
                var uploads = Path.Combine(_environment.WebRootPath, "courses", "images");
                var filePath = Path.Combine(uploads, uniqueFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                await addCourseRequest.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));
                course.ImagePath = filePath;
            }
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return Ok(new CourseResponse()
            {
                Status = StatusType.Success.Value,
                Data = course
            });
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<CourseResponse>> Update(int id, [FromForm] CourseRequest updateCourseRequest)
        {
            var dbCourse = await _context.Courses.FindAsync(id);
            if (dbCourse == null)
                return NotFound(new CourseResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "course not found"
                });
            var category = await _context.Categories.FindAsync(updateCourseRequest.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError(nameof(updateCourseRequest.CategoryId), "no category with this id");
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(new CourseResponse()
                {
                    Status = StatusType.Error.Value,
                    ModelError = ModelState
                });
            }
            dbCourse.Title = updateCourseRequest.Title;
            dbCourse.Description = updateCourseRequest.Description;
            dbCourse.Content = updateCourseRequest.Content;
            dbCourse.Category = category!;
            if (updateCourseRequest.Image != null)
            {
                if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                {
                    _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }
                var uniqueFileName = FileHelper.GetUniqueFileName(updateCourseRequest.Image.FileName);
                var uploads = Path.Combine(_environment.WebRootPath, "courses", "images");
                var filePath = Path.Combine(uploads, uniqueFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                await updateCourseRequest.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));
                dbCourse.ImagePath = filePath;
            }
            await _context.SaveChangesAsync();
            return Ok(new CourseResponse()
            {
                Status = StatusType.Success.Value,
                Data = dbCourse
            });
        }

        [Authorize(Roles = "Trainer")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseResponse>> Delete(int id)
        {
            var dbCourse = await _context.Courses.FindAsync(id);
            if (dbCourse == null)
                return NotFound(new CourseResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "course not found"
                });

            _context.Courses.Remove(dbCourse);
            await _context.SaveChangesAsync();
            return Ok(new CategoryResponse()
            {
                Status = StatusType.Success.Value
            });
        }

        [Authorize(Roles = "Trainer")]
        [HttpGet("{id}/enrolledUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserListResponse>> GetCourseUsers(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new UserListResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "course not found"
                });
            var users = await _context.Entry(course).Collection(c => c.CourseUsers).Query().Select(x => new User
            {
                Id = x.User.Id,
                UserName = x.User.UserName,
                Email = x.User.Email,
                PhoneNumber = x.User.PhoneNumber,
                Role = x.User.Role
            }).AsNoTracking().ToListAsync();
            return Ok(new UserListResponse()
            {
                Status = StatusType.Success.Value,
                Data = users
            });
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}/enroll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> CourseEnroll(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new UserResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "course not found"
                });
            var user = await _userManager.GetUserAsync(User);

            var alreadyEnrolled = await _context.UserCourses.Where(uc => uc.Course == course && uc.User == user).CountAsync();
            if (alreadyEnrolled != 0)
            {
                return BadRequest(new UserResponse()
                {
                    Status = StatusType.Error.Value,
                    ErrorMessage = "already enrolled"
                });
            }
            user.UserCourses.Add(new UserCourse()
            {
                User = user,
                Course = course
            });
            await _context.SaveChangesAsync();
            return Ok(new UserResponse()
            {
                Status = StatusType.Success.Value
            });
        }
    }
}
