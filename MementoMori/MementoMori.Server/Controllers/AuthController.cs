using MementoMori.Server.Data;
using MementoMori.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("auth/login")]
        public IActionResult Login(string userName, string password) {
            UserLoginData userData = new UserLoginData
            {
                UserName = userName, 
                Password = password
            };
            var user = TestUsers.Users.FirstOrDefault(user => user.UserLoginData == userData);
            if (user == null) {
                return BadRequest("Incorrect password.");
            }
            return Ok(user.Id);

        }
    }
}
