﻿using MementoMori.Server.DTOS;
using MementoMori.Server.Interfaces;
using MementoMori.Server.Models;
using MementoMori.Server.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
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
                Id = Guid.Empty
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
            var user = await _authRepo.GetUserByUsername(loginDetails.Username);
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
