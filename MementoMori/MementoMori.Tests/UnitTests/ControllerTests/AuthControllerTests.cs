using MementoMori.Server.Controllers;
using MementoMori.Server.DTOS;
using MementoMori.Server.Interfaces;
using MementoMori.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Xunit;

namespace MementoMori.Tests.UnitTests.ControllerTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {

            _mockAuthService = new Mock<IAuthService>();

            _controller = new AuthController(_mockAuthService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenRegistrationIsSuccessful()
        {
            var registerDetails = new RegisterDetails
            {
                Username = "testUser",
                Password = "TestPassword123!",
                RememberMe = true
            };
            var userId = Guid.NewGuid();
            _mockAuthService
                .Setup(service => service.CreateUserAsync(registerDetails))
                .ReturnsAsync(new User { Id = userId });
            _mockAuthService
                .Setup(service => service.AddCookie(It.IsAny<HttpContext>(), userId, registerDetails.RememberMe));

            var result = await _controller.Register(registerDetails);

            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenLoginIsSuccessful()
        {
            var loginDetails = new LoginDetails
            {
                Username = "testUser",
                Password = "TestPassword123!",
                RememberMe = true
            };
            var userId = Guid.NewGuid();
            var mockUser = new User { Id = userId, Password = "hashedPassword" };
            _mockAuthService
                .Setup(service => service.GetUserByUsername(loginDetails.Username))
                .ReturnsAsync(mockUser);
            _mockAuthService
                .Setup(service => service.VerifyPassword(loginDetails.Password, mockUser.Password))
                .Returns(true);
            _mockAuthService
                .Setup(service => service.AddCookie(It.IsAny<HttpContext>(), userId, loginDetails.RememberMe));

            var result = await _controller.Login(loginDetails);

            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            var loginDetails = new LoginDetails
            {
                Username = "nonExistentUser",
                Password = "TestPassword123!"
            };
            _mockAuthService
                .Setup(service => service.GetUserByUsername(loginDetails.Username))
                .ReturnsAsync((User)null); // Simulate user not found

            var result = await _controller.Login(loginDetails);

            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
        {
            var loginDetails = new LoginDetails
            {
                Username = "testUser",
                Password = "WrongPassword!"
            };
            var mockUser = new User { Id = Guid.NewGuid(), Password = "hashedPassword" };
            _mockAuthService
                .Setup(service => service.GetUserByUsername(loginDetails.Username))
                .ReturnsAsync(mockUser);
            _mockAuthService
                .Setup(service => service.VerifyPassword(loginDetails.Password, mockUser.Password))
                .Returns(false); // Simulate password mismatch

            var result = await _controller.Login(loginDetails);

            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
