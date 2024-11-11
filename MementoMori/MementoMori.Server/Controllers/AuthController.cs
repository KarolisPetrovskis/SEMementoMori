using MementoMori.Server.Database;
using MementoMori.Server.DTOS;
using MementoMori.Server.Models;
using MementoMori.Server.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public AuthController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterDetails registerDetails)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Username == registerDetails.Username);
            if (existingUser)
            {
                return BadRequest(error: "Username is already taken.");
            }

            if (string.IsNullOrEmpty(registerDetails.Password))
            {
                return BadRequest(error: "Password cannot be empty.");
            }

            var hashedPassword = _authService.HashPassword(registerDetails.Password);

            var user = new User
            {
                Username = registerDetails.Username,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _authService.AddCookie(HttpContext, user.Id, registerDetails.RememberMe);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDetails loginDetails) {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDetails.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            bool isValidPassword = _authService.VerifyPassword(loginDetails.Password, user.Password);
            if (!isValidPassword)
            {
                return Unauthorized();
            }

            _authService.AddCookie(HttpContext, user.Id, loginDetails.RememberMe);

            return Ok();
        }
    }
}
