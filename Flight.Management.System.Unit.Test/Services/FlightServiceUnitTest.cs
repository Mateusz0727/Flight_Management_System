using AutoMapper;
using Flight.Management.System.API.Controllers;
using Flight.Management.System.API.Models;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Models.Flight;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.API.Services.Airport;
using Flight.Management.System.API.Services.Flight;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Flight.Management.System.Unit.Test.Services
{
    public class FlightServiceUnitTest
    {
        private readonly TestDatabase _testDatabase;

        private readonly IMapper _mapper;

        public FlightServiceUnitTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperInitializator>();
            });

            _testDatabase = new TestDatabase();
            _mapper = new Mapper(configuration);




        }
        [Fact]
        public async Task GetAllFlight_ReturnsListOfFlights()
        {
            var dbContext = await _testDatabase.GetDatabaseContext();
            var _airplaneService = new AirplaneService(_mapper, dbContext);
            var _airportService = new AirportService(_mapper, dbContext);
            var _flightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);
            // Arrange
            var expectedCount = 10; // Za³o¿enie: W bazie danych znajduje siê 10 lotów

            // Act

            var result = await _flightService.GetAllFlights();

            // Assert
            Assert.NotNull(result); // Upewnij siê, ¿e wynik nie jest pusty
            Assert.IsType<List<Data.Model.Flight>>(result); // Upewnij siê, ¿e wynik jest list¹ lotów
            Assert.Equal(expectedCount, result.Count()); // Upewnij siê, ¿e liczba lotów w wyniku jest zgodna z oczekiwan¹ liczb¹
        }
        [Fact]
        public async Task GetFlight_ReturnsFlight()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();
            var _airplaneService = new AirplaneService(_mapper, dbContext);
            var _airportService = new AirportService(_mapper, dbContext);
            var _flightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);

            // Tworzymy przyk³adowy lot i dodajemy go do bazy danych
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

            var newFlight = new Data.Model.Flight
            {
                Id = 11, // Identyfikator lotu
                PublicId = Guid.NewGuid().ToString(), // Unikatowy identyfikator lotu
                ArrivalPoint = airport1, // Lotnisko docelowe
                DepartureDate = DateTime.Now.AddDays(1), // Data wylotu (jutro)
                DeparturePoint = airport2, // Lotnisko wylotu
                Airplane = airplane // Samolot
            };
            dbContext.Flight.Add(newFlight);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await _flightService.GetFlight(newFlight.Id);

            // Assert
            Assert.NotNull(result); // Upewnij siê, ¿e wynik nie jest pusty
            Assert.IsType<Data.Model.Flight>(result); // Upewnij siê, ¿e wynik jest typu Flight
            Assert.Equal(newFlight.Id, result.Id); // Upewnij siê, ¿e identyfikator lotu w wyniku jest zgodny z oczekiwanym identyfikatorem
        }

        [Fact]
        public async Task CreateAsync_ValidFlightModel_ReturnsNewFlight()
        {
            // Arrange
            var flightModel = new FlightModel
            {
                // Ustaw w³aœciwoœci modelu lotu
                DepartureDate = DateTime.UtcNow.AddDays(1), // Data w przysz³oœci
                DeparturePointId = 1, // Identyfikator punktu wylotu
                ArrivalPointId = 2, // Identyfikator punktu przylotu
                AirplaneId = 1 // Identyfikator samolotu
            };

            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneService = new AirplaneService(_mapper, dbContext);
            var airportService = new AirportService(_mapper, dbContext);
            var flightService = new FlightService(_mapper, dbContext, airplaneService, airportService);


            // Act
            var result = await flightService.CreateAsync(flightModel);

            // Assert
            var createdResult = Assert.IsType<Data.Model.Flight>(result);
            Assert.Equal(flightModel.DeparturePointId, createdResult.DeparturePoint.Id); // Sprawdzamy poprawnoœæ punktu wylotu
            Assert.Equal(flightModel.ArrivalPointId, createdResult.ArrivalPoint.Id); // Sprawdzamy poprawnoœæ punktu przylotu
            Assert.Equal(flightModel.AirplaneId, createdResult.Airplane.Id); // Sprawdzamy poprawnoœæ samolotu
                                                                             // Mo¿esz równie¿ sprawdziæ inne w³aœciwoœci, jeœli s¹ istotne dla testowanego przypadku
        }
        public static IEnumerable<object[]> InvalidFlightData =>
           new List<object[]>
           {
                // Nieprawid³owa data - przesz³aœæ
                new object[] { new FlightModel { DepartureDate = DateTime.UtcNow.AddDays(-1), DeparturePointId = 1, ArrivalPointId = 2, AirplaneId = 1 }, "Invalid date (cannot set date in the past)" },
                // Ten sam punkt wylotu i przylotu
                new object[] { new FlightModel { DepartureDate = DateTime.UtcNow.AddDays(1), DeparturePointId =   1 , ArrivalPointId = 1 , AirplaneId = 1  }, "You cannot set the same departure and arrival points" },
                // Dodaj inne przypadki testowe w razie potrzeby
                  new object[] { new FlightModel { DepartureDate = DateTime.UtcNow.AddDays(1), DeparturePointId =  1 , ArrivalPointId =  2 , AirplaneId  = 999  }, "The entered aircraft doesn't exist" },
                // Przypadek: Brak punktu przylotu
                new object[] { new FlightModel { DepartureDate = DateTime.UtcNow.AddDays(1), DeparturePointId =  1 , ArrivalPointId =  999, AirplaneId  = 1  }, "The entered arrival point doesn't exist" },
                // Przypadek: Brak punktu wylotu
                new object[] { new FlightModel { DepartureDate = DateTime.UtcNow.AddDays(1), DeparturePointId =  999 , ArrivalPointId =  2 , AirplaneId  = 1 }, "The entered departure point doesn't exist" }
           };

        [Theory]
        [MemberData(nameof(InvalidFlightData))]
        public async Task CreateFlight_InvalidModel_ReturnsBadRequestWithExceptionMessage(FlightModel invalidFlightModel, string expectedExceptionMessage)
        {
            // Arrange


            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneService = new AirplaneService(_mapper, dbContext);
            var airportService = new AirportService(_mapper, dbContext);
            var flightService = new FlightService(_mapper, dbContext, airplaneService, airportService);


            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () => await flightService.CreateAsync(invalidFlightModel));

            // Assert
            Assert.Equal(expectedExceptionMessage, exception.Message);

        }
        [Fact]
        public async Task DeleteById_ExistingFlight_ReturnsNoException()
        {
            // Arrange
            int existingFlightId = 1; // Za³ó¿my, ¿e istnieje lot o tym ID
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneService = new AirplaneService(_mapper, dbContext);
            var airportService = new AirportService(_mapper, dbContext);
            var flightService = new FlightService(_mapper, dbContext, airplaneService, airportService);

            // Act & Assert
            try
            {
                await flightService.DeleteById(existingFlightId);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Expected no exception, but got: {ex.Message}");
            }
        }
        [Fact]
        public async Task DeleteFlightById_NonExistingFlight_ThrowsException()
        {
            // Arrange
            int nonExistingFlightId = 999; // Za³ó¿my, ¿e nie istnieje lot o tym ID
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneService = new AirplaneService(_mapper, dbContext);
            var airportService = new AirportService(_mapper, dbContext);
            var flightService = new FlightService(_mapper, dbContext, airplaneService, airportService);
            var controller = new FlightController(flightService);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await flightService.DeleteById(nonExistingFlightId));
        }
    }





}
