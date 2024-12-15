using MementoMori.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColorController(IAuthService authService, IAuthRepo authRepo) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IAuthRepo _authRepo = authRepo;

        [HttpGet("color")]
        public async Task<IActionResult> GetUserColor()
        {
            var userId = _authService.GetRequesterId(HttpContext);
            if (userId == null)
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
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _authRepo.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            user.HeaderColor = request.NewColor;

            return Ok();
        }
    }
}