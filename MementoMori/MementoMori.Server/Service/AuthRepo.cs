using MementoMori.Server.Models;
using MementoMori.Server.Database;
using MementoMori.Server.Interfaces;
using MementoMori.Server.DTOS;
using Microsoft.EntityFrameworkCore;
using MementoMori.Server.Exceptions;

namespace MementoMori.Server.Service
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthRepo(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public User[] GetAllUsers()
        {
            return _context.Users.ToArray();
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

            var hashedPassword = _authService.HashPassword(registerDetails.Password);

            var user = new User
            {
                Username = registerDetails.Username,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
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
    }
}
