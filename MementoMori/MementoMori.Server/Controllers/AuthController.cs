using MementoMori.Server.DTOS;
using MementoMori.Server.Exceptions;
using MementoMori.Server.Interfaces;
using MementoMori.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuthRepo _authRepo;
        private static readonly ConcurrentDictionary<string, User> _registeredUsers = new();
        private static bool initialized = false;

        public AuthController(IAuthService authService, IAuthRepo authRepo)
        {
            _authService = authService;
            _authRepo = authRepo;
            if (!initialized)
            {

                var users = _authRepo.GetAllUsers();
                foreach (var user in users)
                {
                    _registeredUsers.TryAdd(user.Username, user);
                }
                initialized = true;
            }
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDetails registerDetails)
        {
            if (_registeredUsers.ContainsKey(registerDetails.Username))
            {
                return Conflict(new { Message = "Username is already taken." });
            }

            var placeholderUser = new User
            {
                Username = registerDetails.Username,
                Password = string.Empty,
                Id = Guid.Empty,
                HeaderColor = "white"
            };
            _registeredUsers.TryAdd(registerDetails.Username, placeholderUser);

            var user = await _authRepo.CreateUserAsync(registerDetails);

            _registeredUsers[registerDetails.Username] = user;

            _authService.AddCookie(HttpContext, user.Id, registerDetails.RememberMe);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDetails loginDetails)
        {
            try
            {
                var user = await _authRepo.GetUserByUsernameAsync(loginDetails.Username);
                bool isValidPassword = _authService.VerifyPassword(loginDetails.Password, user.Password);
                if (!isValidPassword)
                {
                    return Unauthorized();
                }

                _authService.AddCookie(HttpContext, user.Id, loginDetails.RememberMe);

                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("loginResponse")]
        public IActionResult GetLoginResponse()
        {
            var userId = _authService.GetRequesterId(HttpContext);
            if (!userId.HasValue)
            {
                return Ok(new LoginResponse { IsLoggedIn = false });
            }

            var loginResponse = new LoginResponse
            {
                IsLoggedIn = true
            };

            return Ok(loginResponse);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _authService.RemoveCookie(HttpContext);
            return Ok();
        }

        [HttpGet("color")]
        public async Task<IActionResult> GetUserColor()
        {
            var userId = _authService.GetRequesterId(HttpContext);
            if (!userId.HasValue)
            {
                return BadRequest(new { Message = "User ID not found." });
            }

            var user = await _authRepo.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            return Ok(new { color = user.HeaderColor });
        }

        [HttpPost("newColor")]
        public async Task<IActionResult> UpdateUserColor([FromBody] UpdateColorRequest request)
        {
            var userId = _authService.GetRequesterId(HttpContext);
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var user = await _authRepo.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            user.HeaderColor = request.NewColor;

            var success = await _authRepo.SaveAsync();
            if (!success)
            {
                return StatusCode(500, new { Message = "Failed to update user color." });
            }

            return Ok(new { Message = "Header color updated successfully." });
        }
    }
}
