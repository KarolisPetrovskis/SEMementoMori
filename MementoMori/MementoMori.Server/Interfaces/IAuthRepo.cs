using MementoMori.Server.DTOS;
using MementoMori.Server.Models;

namespace MementoMori.Server.Interfaces
{
    public interface IAuthRepo
    {
        Task<User> CreateUserAsync(RegisterDetails registerDetails);
        User[] GetAllUsers();
        Task<User> GetUserById(Guid id);
        Task<User?> GetUserByUsername(string username);
    }
}