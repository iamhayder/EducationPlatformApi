using EducationPlatformApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using EducationPlatformApi.Services;
using Microsoft.AspNetCore.Authorization;
using EducationPlatformApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatformApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly EducationPlatformApiContext _context;


        public UsersController(
            UserManager<ApplicationUser> userManager, JwtService jwtService, EducationPlatformApiContext context
        )
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> RegisterUser(UserRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(
                new ApplicationUser() { UserName = user.UserName, Email = user.Email, PhoneNumber = user.PhoneNumber, Role =user.Role },
                user.Password
            );

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction("RegisterUser", new UserResponse
            { 
                UserName = user.UserName, 
                Email = user.Email, 
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            }, user);

        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetUserByUsername(string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return new UserResponse
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticationResponse>> LoginUser(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return BadRequest("Bad credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var token = _jwtService.CreateToken(user);

            return Ok(token);
        }


        [Authorize(Roles = "Trainer")]
        [HttpGet("trainerCourses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<Course>>> GetTrainerCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            var courses = await _context.Courses.Where(c => c.TrainerUserId == new Guid (user.Id)).ToListAsync();
            return Ok(courses);

        }

        [Authorize(Roles = "User")]
        [HttpGet("enrolledCourses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<Course>>> GetUserEnrolledCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            var enrolledCourses = await _context.Entry(user).Collection(c => c.UserCourses).Query().Select(x => new Course
            {
                Id = x.Course.Id,
                Title = x.Course.Title,
                Description = x.Course.Description,
                Content = x.Course.Content,
                CategoryId = x.Course.CategoryId,
                TrainerUserId = x.Course.TrainerUserId
            }).ToListAsync();

            return Ok(enrolledCourses);

        }
    }
}