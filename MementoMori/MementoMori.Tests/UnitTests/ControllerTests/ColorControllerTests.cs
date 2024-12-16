using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.Controllers;
using MementoMori.Server.Interfaces;
using MementoMori.Server.Models;

namespace MementoMori.Tests.UnitTests.ControllerTests
{
    public class ColorControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IAuthRepo> _authRepoMock;
        private readonly ColorController _controller;

        public ColorControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _authRepoMock = new Mock<IAuthRepo>();
            _controller = new ColorController(_authServiceMock.Object, _authRepoMock.Object);
        }
        [Fact]
        public async Task UpdateUserColor_ValidRequest_UpdatesColorSuccessfully()
        {
            var userId = Guid.NewGuid();
            var user = new User { Username = "test", Password = "testy", HeaderColor = "#FFFFFF" };

            _authServiceMock.Setup(s => s.GetRequesterId(It.IsAny<HttpContext>())).Returns(userId);
            _authRepoMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var result = await _controller.UpdateUserColor(new UpdateColorRequest { NewColor = "#000000" });

            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal("#000000", user.HeaderColor);
        }
    }
}