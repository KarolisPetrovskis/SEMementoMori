using MementoMori.Server.Models;

namespace MementoMori.Server.Interfaces
{
    public interface IAuthService
    {
        void AddCookie(HttpContext httpContext, Guid userId, bool isPersistent);
        Guid? GetRequesterId(HttpContext httpContext);
        User GetUserById(Guid id);
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }
}