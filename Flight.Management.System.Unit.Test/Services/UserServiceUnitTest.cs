using AutoMapper;
using Flight.Management.System.API.Models;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.API.Services.Airport;
using Flight.Management.System.API.Services.Flight;
using Flight.Management.System.API.Services.User;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Flight.Management.System.Unit.Test.Services
{
    public class UserServiceUnitTest
    {
        private readonly TestDatabase _testDatabase;
        private readonly IMapper _mapper;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;

        public UserServiceUnitTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperInitializator>();
            });
            _testDatabase = new TestDatabase();
            _mapper = new Mapper(configuration);
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        }

        [Fact]
        public async Task GetUser_ReturnsUser()
        {
            var dbContext = await _testDatabase.GetDatabaseContext();
            var _airplaneService = new AirplaneService(_mapper, dbContext);
            var _airportService = new AirportService(_mapper, dbContext);
            var _userService = new UserService(_mapper, _passwordHasherMock.Object, dbContext);

            var user = new User
            {
                Id = 1,
                DateCreatedUtc = DateTime.Now,
                DateModifiedUtc = DateTime.Now,
                Surname = "user " + 1,
                Email = $"user1@admin.pl",
                GivenName = "user " + 1,
                IsAdmin = true,
                PublicId = Guid.NewGuid().ToString(),
                UserName = "user " + 1
            };

            _passwordHasherMock.Setup(m => m.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(user.Password);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var result = await _userService.GetByEmailAsync(user.Email);

            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(user.Id, result.Id);
        }
    }
}
