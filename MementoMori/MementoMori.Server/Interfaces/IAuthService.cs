using MementoMori.Server.DTOS;
using MementoMori.Server.Models;

namespace MementoMori.Server.Interfaces
{
    public interface IAuthService
    {
        void AddCookie(HttpContext httpContext, Guid userId, bool isPersistent);
        void RemoveCookie(HttpContext httpContext);
        Task<User> CreateUserAsync(RegisterDetails registerDetails);
        Guid? GetRequesterId(HttpContext httpContext);
        Task<User> GetUserById(Guid id);
        Task<User?> GetUserByUsername(string username);
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
        User[] GetAllUsers();
    }
}