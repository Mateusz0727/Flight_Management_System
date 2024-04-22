using AutoMapper;
using Flight.Management.System.API.Configuration;
using Flight.Management.System.API.Controllers.Auth;
using Flight.Management.System.API.Models.Auth;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.API.Services.Airport;
using Flight.Management.System.API.Services.Auth;
using Flight.Management.System.API.Services.User;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Identity;
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
        private readonly TestDatabase _testDatabase;
       
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<JWTConfig> _jwtConfigMock;
        private readonly IMapper _mapper;
        public AuthenticationControllerTests()
        {

            _testDatabase = new TestDatabase();
            _jwtConfigMock= new Mock<JWTConfig>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();
            var userServiceMock =new UserService(_mapper,_passwordHasherMock.Object, dbContext);
            var authServiceMock = new AuthService(_jwtConfigMock.Object,_passwordHasherMock.Object);
        
            var controller = new AuthController(authServiceMock, userServiceMock);
            var loginFormModel = new LoginFormModel
            {
                Email = "1@1",
                Password = "admin"
            };

          
            // Act
            var result = await controller.Login(loginFormModel);

            // Assert
            var okResult = Assert.IsType<ActionResult<UserTokens>>(result);
            Assert.NotNull(okResult);
        }

        [Fact]
        public async Task Login_WithInvalidModel_ReturnsBadRequest()
        {
            var dbContext = await _testDatabase.GetDatabaseContext();
            var userServiceMock = new UserService(_mapper, _passwordHasherMock.Object, dbContext);
            var authServiceMock = new AuthService(_jwtConfigMock.Object, _passwordHasherMock.Object);

            var controller = new AuthController(authServiceMock, userServiceMock);
            // Arrange
            var invalidModel = new LoginFormModel
            {
                Email = "test@example.com" // Missing password
            };

         

            // Act
            var result = await controller.Login(invalidModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

    }
}
