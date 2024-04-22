using AutoMapper;
using Flight.Management.System.API.Controllers.Airplane;
using Flight.Management.System.API.Models;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Management.System.Unit.Test.Controller
{
    public  class AirplaneTypeControllerUnitTest
    {
        private readonly TestDatabase _testDatabase;

        private readonly IMapper _mapper;
        public AirplaneTypeControllerUnitTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperInitializator>();
            });

            _testDatabase = new TestDatabase();
            _mapper = new Mapper(configuration);
        }
        [Fact]
      
        public async Task GetAirplaneType_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var airplaneTypeId = 1;
            var dbContext = await _testDatabase.GetDatabaseContext();

            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);


            var controller = new AirplaneTypeController(airplaneTypeService);

            // Act
            var result = await controller.GetAirplaneById(airplaneTypeId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]

        public async Task GetAirplaneType_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var airplaneTypeId = 999;
            var dbContext = await _testDatabase.GetDatabaseContext();

            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);


            var controller = new AirplaneTypeController(airplaneTypeService);

            // Act
            var result = await controller.GetAirplaneById(airplaneTypeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task CreateAirplaneType_ValidModel_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var airplaneTypeFormModel = new AirplaneTypeFormModel
            {
                Name = "Test Airplane Type",
                SymbolInRegistrationNumber = 'T',
                NumberOfSeats = 150
            };

            var createdAirplaneType = new AirplaneType
            {
                Id = 1,
                Name = "Test Airplane Type",
                SymbolInRegistrationNumber = 'T',
                NumberOfSeats = 150
            };
            var dbContext = await _testDatabase.GetDatabaseContext();

            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
          
            var controller = new AirplaneTypeController(airplaneTypeService);

            // Act
            var result = await controller.CreateAirplaneType(airplaneTypeFormModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(AirplaneTypeController.GetAirplaneById), createdAtActionResult.ActionName);
        }
       /* [Theory]
        [InlineData(null, 'A', 150)] // Niepoprawna nazwa
        [InlineData("Test Airplane Type", null, 150)] // Brak symbolu
        [InlineData("Test Airplane Type", 'A', null)] // Niepoprawna liczba miejsc
        public async Task CreateAirplaneType_ValidModel_ReturnsExpectedResult(string name, char symbol, uint numberOfSeats)
        {
            // Arrange
            var airplaneTypeFormModel = new AirplaneTypeFormModel
            {
                Name = name,
                SymbolInRegistrationNumber = symbol,
                NumberOfSeats = numberOfSeats
            };
            var dbContext = await _testDatabase.GetDatabaseContext();
            var mockService = new Mock<AirplaneTypeService>(_mapper, dbContext);
          
            var controller = new AirplaneTypeController(mockService.Object);

            // Act
            var result = await controller.CreateAirplaneType(airplaneTypeFormModel);

            // Assert
            if (name != null && symbol != default(char) && numberOfSeats > 0)
            {
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("GetAirplaneById", createdAtActionResult.ActionName);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errors = Assert.IsType<SerializableError>(badRequestResult.Value);

                if (name == null)
                    Assert.Contains("Name", errors.Keys);

                if (symbol == default(char))
                    Assert.Contains("SymbolInRegistrationNumber", errors.Keys);

                if (numberOfSeats <= 0)
                    Assert.Contains("NumberOfSeats", errors.Keys);
            }
        }*/
    }
}

