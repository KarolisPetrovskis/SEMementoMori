using System.Text;
using System.Security.Cryptography;
using MementoMori.Server.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MementoMori.Server.Database;

namespace MementoMori.Server.Service
{
    public class AuthService
    {
        public readonly AppDbContext _context;

        public AuthService(AppDbContext context) 
        {
            _context = context;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }

        public async void AddCookie(HttpContext httpContext, Guid userId, bool isPersistent) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(10)
            });
        }
        
        public User GetUserById(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) 
            {
                throw new Exception();
            }
            return user;
        }

        public Guid? GetRequesterId(HttpContext httpContext)
        {
            var claim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out Guid requesterId))
            {
                return requesterId;
            }
            return null;
        }
    }
}
