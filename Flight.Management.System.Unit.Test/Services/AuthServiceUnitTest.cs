using Flight.Management.System.API.Configuration;
using Flight.Management.System.API.Models.Auth;
using Flight.Management.System.API.Services.Auth;
using Flight.Management.System.Data.Model;
using Flight.Management.System.Unit.Test.Controller;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Flight.Management.System.Unit.Test.Services
{
    public class AuthServiceUnitTest
    {
        private readonly TestDatabase _testDatabase;
        private readonly Mock<JWTConfig> _jwtConfigMock;

        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        public AuthServiceUnitTest()
        {
           

                _testDatabase = new TestDatabase();
                _jwtConfigMock = new Mock<JWTConfig>();
                _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            
        }
        [Fact]

        public void Login_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var loginFormModel = new LoginFormModel
            {
                Email = "test@example.com",
                Password = "testpassword"
            };

            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Password = "hashedPassword" // Załóżmy, że hasło zostało już zahashowane
            };

            var passwordHasherMock = new Mock<IPasswordHasher<User>>();
            passwordHasherMock.Setup(h => h.VerifyHashedPassword(user, user.Password, loginFormModel.Password))
                .Returns(PasswordVerificationResult.Success);

            var authService = new AuthService(_jwtConfigMock.Object, passwordHasherMock.Object);

            // Act
            var result = authService.Login(loginFormModel, user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var loginFormModel = new LoginFormModel
            {
                Email = "test@example.com",
                Password = "testpassword"
            };

            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Password = "hashedPassword" // Załóżmy, że hasło zostało już zahashowane
            };

            var passwordHasherMock = new Mock<IPasswordHasher<User>>();
            passwordHasherMock.Setup(h => h.VerifyHashedPassword(user, user.Password, loginFormModel.Password))
                .Returns(PasswordVerificationResult.Failed);

            var authService = new AuthService(_jwtConfigMock.Object, passwordHasherMock.Object);

            // Act
            var result = authService.Login(loginFormModel, user);

            // Assert
            Assert.False(result);
        }
    }

}

