using System.Text;
using System.Security.Cryptography;
using MementoMori.Server.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MementoMori.Server.Database;
using MementoMori.Server.Interfaces;
using MementoMori.Server.DTOS;
using Microsoft.EntityFrameworkCore;
using MementoMori.Server.Exceptions;

namespace MementoMori.Server.Service
{
    public class AuthService : IAuthService
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

        public async Task<User> CreateUserAsync(RegisterDetails registerDetails)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Username == registerDetails.Username);
            if (existingUser)
            {
                throw new Exception();
            }

            if (string.IsNullOrEmpty(registerDetails.Password))
            {
                throw new Exception();
            }

            var hashedPassword = HashPassword(registerDetails.Password);

            var user = new User
            {
                Username = registerDetails.Username,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
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

        public void RemoveCookie(HttpContext httpContext)
        {
            httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new UserNotFoundException(id);
            }
            return user;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
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
