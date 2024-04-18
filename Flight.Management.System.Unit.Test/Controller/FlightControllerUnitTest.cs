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
            var _airplaneService = new AirplaneService(_mapper, dbContext);
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
            var _airplaneService = new AirplaneService(_mapper, dbContext);
            var _airportService = new AirportService(_mapper, dbContext);
            var emptyFlightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);

            // Wypełnij bazę danych pustą listą lotów
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
            var _airplaneService = new AirplaneService(_mapper, dbContext);
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

            // Tworzymy nowy samolot
            var airplane = new Airplane
            {
                Name = "New Airplane",
                PublicId = Guid.NewGuid().ToString()
            };

            // Dodajemy lotniska i samolot do bazy danych
            dbContext.Airports.Add(airport1);
            dbContext.Airports.Add(airport2);
            dbContext.Airplane.Add(airplane);
            dbContext.SaveChanges();
            // Dodaj przykładowy lot do bazy danych
            var sampleFlight = new Data.Model.Flight
            {
                Id = 11, // Identyfikator lotu
                PublicId = Guid.NewGuid().ToString(), // Unikatowy identyfikator lotu
                ArrivalPoint = airport1, // Lotnisko docelowe
                DepartureDate = DateTime.Now.AddDays(1), // Data wylotu (jutro)
                DeparturePoint = airport2, // Lotnisko wylotu
                Airplane = airplane // Samolot
            };
            dbContext.Flight.Add(sampleFlight);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await controller.GetById(sampleFlight.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFlight = Assert.IsAssignableFrom<Flight.Management.System.Data.Model.Flight>(okResult.Value);
            Assert.Equal(sampleFlight.Id, returnedFlight.Id);
            // Tutaj można dodać dodatkowe asercje, aby sprawdzić, czy zwracany lot zawiera poprawne dane
        }
        [Fact]
        public async void FlightController_GetFlightById_ReturnsNoContentWhenFlightNotFound()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();
            var _airplaneService = new AirplaneService(_mapper, dbContext);
            var _airportService = new AirportService(_mapper, dbContext);
            var _emptyFlightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);
            var controller = new FlightController(_emptyFlightService);

            // Identyfikator lotu, który nie istnieje w bazie danych
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
            var _airplaneService = new AirplaneService(_mapper, dbContext);
            var _airportService = new AirportService(_mapper, dbContext);
            var _emptyFlightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);
            var controller = new FlightController(_emptyFlightService);

            var flightModel = new FlightModel
            {
                // Ustaw właściwości modelu lotu
                DepartureDate = DateTime.UtcNow.AddDays(1), // Data w przyszłości
                DeparturePointId = 1, // Identyfikator punktu wylotu
                ArrivalPointId = 2, // Identyfikator punktu przylotu
                AirplaneId = 1 // Identyfikator samolotu
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
