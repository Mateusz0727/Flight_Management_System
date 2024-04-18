using AutoMapper;
using Flight.Management.System.API.Controllers;
using Flight.Management.System.API.Models;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Models.Flight;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.API.Services.Airport;
using Flight.Management.System.API.Services.Flight;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.Unit.Test.Controller
{
    public class AirplaneControllerUnitTest
    {
        private readonly TestDatabase _testDatabase;

        private readonly IMapper _mapper;
        public AirplaneControllerUnitTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperInitializator>();
            });

            _testDatabase = new TestDatabase();
            _mapper = new Mapper(configuration);
        }
        [Fact]
        public async Task CreateAsync_ValidModel_ReturnsNewAirplane()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();
            var _airplaneService = new AirplaneService(_mapper, dbContext);

            var controller = new AirplaneController(_airplaneService);

            var airplane = new AirplaneModel
            {
                Name = "plane 737"
            };

            var result = await controller.CreateAirplane(airplane);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.IsType<Airplane>(createdResult.Value);

        }
        [Fact]
        public async Task CreateAirplane_InvalidModel_ReturnsBadRequestWithModelStateErrors()
        {
            // Arrange
            var invalidModel = new AirplaneModel();

            var airplaneServiceMock = new Mock<AirplaneService>(MockBehavior.Strict, null, null);
            var controller = new AirplaneController(airplaneServiceMock.Object);
            controller.ModelState.AddModelError("Name", "The name of the airplane is required.");

            // Act
            var result = await controller.CreateAirplane(invalidModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(errors.ContainsKey("Name"));
        }
    }
}