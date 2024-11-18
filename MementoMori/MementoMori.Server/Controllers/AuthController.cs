using MementoMori.Server.DTOS;
using MementoMori.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDetails registerDetails)
        {
            var user = await _authService.CreateUserAsync(registerDetails);

            _authService.AddCookie(HttpContext, user.Id, registerDetails.RememberMe);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDetails loginDetails)
        {
            var user = await _authService.GetUserByUsername(loginDetails.Username);
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

        [HttpGet("loginResponse")]
        public IActionResult GetLoginResponse()
        {
            bool isLoggedIn = _authService.GetRequesterId(HttpContext).HasValue;

            var loginResponse = new LoginResponse
            {
                IsLoggedIn = isLoggedIn
            };

            return Ok(loginResponse);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            _authService.RemoveCookie(HttpContext);
            return Ok();
        }
    }
}
