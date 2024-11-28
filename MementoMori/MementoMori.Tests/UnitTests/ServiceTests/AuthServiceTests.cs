using Castle.Core.Logging;
using MementoMori.Server.Database;
using MementoMori.Server.Service;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Http;
using Xunit;
using MementoMori.Server.DTOS;
using MementoMori.Server.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MementoMori.Tests.UnitTests.ServiceTests;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly AppDbContext _context;
    private readonly Mock<HttpContext> _mockHttpContext;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        _authService = new AuthService(_context);

        _mockHttpContext = new Mock<HttpContext>();
    }

    [Fact]
    public void HashPassword_ReturnsHashedPassword()
    {
        string password = "SecurePassword123";

        string hashedPassword = _authService.HashPassword(password);

        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }

    [Theory]
    [InlineData("SecurePassword123!", "DifferentPassword")]
    [InlineData("SecurePassword123", "SecurePassword123")]
    public void VerifyPassword_ReturnsExpectedResult(string storedPassword, string passwordToVerify)
    {
        var storedHash = _authService.HashPassword(storedPassword);

        var result = _authService.VerifyPassword(passwordToVerify, storedHash);

        if (storedPassword == passwordToVerify)
        {
            Assert.True(result);
        }
        else
        {
            Assert.False(result);
        }
    }

    [Fact]
    public async Task CreateUserAsync_CreatesUserSuccessfully()
    {
        var registerDetails = new RegisterDetails { Username = "newuser", Password = "SecurePassword123", RememberMe = true };
        var hashedPassword = _authService.HashPassword(registerDetails.Password);

        var user = await _authService.CreateUserAsync(registerDetails);

        Assert.NotNull(user);
        Assert.Equal(registerDetails.Username, user.Username);
        Assert.Equal(hashedPassword, user.Password);
    }

    [Fact]
    public async Task CreateUserAsync_ThrowsException_WhenUsernameExists()
    {
        var existingUser = new User { Username = "existinguser", Password = "SecurePassword123" };
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var registerDetails = new RegisterDetails { Username = "existinguser", Password = "SecurePassword123" };

        await Assert.ThrowsAsync<Exception>(async () => await _authService.CreateUserAsync(registerDetails));
    }

    [Fact]
    public async Task CreateUserAsync_ThrowsException_WhenPasswordIsNullOrEmpty()
    {
        var registerDetails = new RegisterDetails { Username = "somenewuser", Password = "" };

        await Assert.ThrowsAsync<Exception>(async () => await _authService.CreateUserAsync(registerDetails));
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var username = "existinguser";
        var password = "password";
        var passwordHash = _authService.HashPassword(password);
        var user = new User { Id = userId, Username = username, Password = password };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _authService.GetUserById(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(username, result.Username);
    }

    [Fact]
    public async Task GetUserById_ThrowsUserNotFoundException_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();

        await Assert.ThrowsAsync<UserNotFoundException>(async () => await _authService.GetUserById(userId));
    }

    [Fact]
    public async Task GetUserByUsername_ReturnsUser_WhenUserExists()
    {
        var username = "existinguser";
        var password = "password";
        var user = new User { Username = username, Password = password };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _authService.GetUserByUsername("existinguser");

        Assert.NotNull(result);
        Assert.Equal("existinguser", result.Username);
    }

    [Fact]
    public async Task GetUserByUsername_ReturnsNull_WhenUserDoesNotExist()
    {
        var result = await _authService.GetUserByUsername("nonexistentuser");

        Assert.Null(result);
    }

    [Fact]
    public void GetRequesterId_ReturnsRequesterId_WhenClaimExists()
    {
        var userId = Guid.NewGuid();
        _mockHttpContext.Setup(ctx => ctx.User.Claims).Returns(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        });

        var result = _authService.GetRequesterId(_mockHttpContext.Object);

        Assert.NotNull(result);
        Assert.Equal(userId, result);
    }

    [Fact]
    public void GetRequesterId_ReturnsNull_WhenClaimDoesNotExist()
    {
        _mockHttpContext.Setup(ctx => ctx.User.Claims).Returns(new List<Claim>());

        var result = _authService.GetRequesterId(_mockHttpContext.Object);

        Assert.Null(result);
    }
}