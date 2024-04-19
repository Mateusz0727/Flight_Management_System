using Flight.Management.System.API.Configuration;
using Flight.Management.System.API.Controllers.Auth;
using Flight.Management.System.API.Models.Auth;
using Flight.Management.System.API.Services.Auth;
using Flight.Management.System.API.Services.User;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Management.System.Unit.Test.Controller
{
    public class AuthenticationControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<UserService> _mockUserService;
        private readonly Mock<AuthService> _mockAuthService;

        public AuthenticationControllerTests()
        {
            _mockUserService = new Mock<UserService>();
            _mockAuthService = new Mock<AuthService>();
            _controller = new AuthController( _mockAuthService.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task Login_WithValidModel_ReturnsOkResult()
        {
            // Arrange
            var validModel = new LoginFormModel
            {
                Email = "test@example.com",
                Password = "testpassword"
            };

            var fakeUser = new User(); // Create a fake user entity if needed

            _mockUserService.Setup(service => service.GetByEmailAsync(validModel.Email)).ReturnsAsync(fakeUser);
            _mockAuthService.Setup(service => service.Login(validModel, fakeUser)).Returns(true);
            _mockAuthService.Setup(service => service.CreateToken(fakeUser)).Returns(new UserTokens());

            // Act
            var result = await _controller.Login(validModel);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Login_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var invalidModel = new LoginFormModel
            {
                Email = "test@example.com" // Missing password
            };

            _controller.ModelState.AddModelError("Password", "The password field is required.");

            // Act
            var result = await _controller.Login(invalidModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

    }
}
