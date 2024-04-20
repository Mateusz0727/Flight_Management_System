using AutoMapper;
using Flight.Management.System.API.Controllers;
using Flight.Management.System.API.Models;
using Flight.Management.System.API.Models.Flight;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.API.Services.Airport;
using Flight.Management.System.API.Services.Flight;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Flight.Management.System.Unit.Test.Controller
{
    public class FlightControllerUnitTest
    {
        private readonly TestDatabase _testDatabase;

        private readonly IMapper _mapper;
        public FlightControllerUnitTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperInitializator>();
            });

            _testDatabase = new TestDatabase();
            _mapper = new Mapper(configuration);
        }
        [Fact]
        public async void FlightController_GetAllFlight_ReturnOK()
        {
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var _airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var _airportService = new AirportService(_mapper, dbContext);
            var _flightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);
            var controller = new FlightController(_flightService);

            var result = await controller.GetAllFlight();

            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async void FlightController_GetAllFlight_ReturnNoContent()
        {
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var _airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var _airportService = new AirportService(_mapper, dbContext);
            var emptyFlightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);

          
            dbContext.Flight.RemoveRange(await dbContext.Flight.ToListAsync());
            await dbContext.SaveChangesAsync();

            var controller = new FlightController(emptyFlightService);

            var result = await controller.GetAllFlight();

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void FlightController_GetFlightById_ReturnsOkWithCorrectFlight()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var _airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var _airportService = new AirportService(_mapper, dbContext);
            var _flightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);
            var controller = new FlightController(_flightService);

            var airport1 = new Airport
            {
                AirportName = "Airport 1",
                PublicId = Guid.NewGuid().ToString()
            };

            var airport2 = new Airport
            {
                AirportName = "Airport 2",
                PublicId = Guid.NewGuid().ToString()
            };

          
            var airplane = new Airplane
            {
                PublicId = Guid.NewGuid().ToString()
            };

           
            dbContext.Airports.Add(airport1);
            dbContext.Airports.Add(airport2);
            dbContext.Airplane.Add(airplane);
            dbContext.SaveChanges();
            
            var sampleFlight = new Data.Model.Flight
            {
                Id = 11, 
                PublicId = Guid.NewGuid().ToString(), 
                ArrivalPoint = airport1,
                DepartureDate = DateTime.Now.AddDays(1), 
                DeparturePoint = airport2, 
                Airplane = airplane 
            };
            dbContext.Flight.Add(sampleFlight);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await controller.GetById(sampleFlight.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFlight = Assert.IsAssignableFrom<Flight.Management.System.Data.Model.Flight>(okResult.Value);
            Assert.Equal(sampleFlight.Id, returnedFlight.Id);
           
        }
        [Fact]
        public async void FlightController_GetFlightById_ReturnsNoContentWhenFlightNotFound()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var _airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var _airportService = new AirportService(_mapper, dbContext);
            var _emptyFlightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);
            var controller = new FlightController(_emptyFlightService);

          
            int nonExistingFlightId = 999;

            // Act
            var result = await controller.GetById(nonExistingFlightId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task FlightController_CreateFlight_ReturnsCreatedWithValidModel()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var _airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var _airportService = new AirportService(_mapper, dbContext);
            var _emptyFlightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);
            var controller = new FlightController(_emptyFlightService);

            var flightModel = new FlightModel
            {
              
                DepartureDate = DateTime.UtcNow.AddDays(1), 
                DeparturePointId = 1, 
                ArrivalPointId = 2,
                AirplaneId = 1 
            };

            // Act
            var result = await controller.CreateFlight(flightModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.IsType<Data.Model.Flight>(createdResult.Value);
        }
       
      
    }
}
