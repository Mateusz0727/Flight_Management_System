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
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var _airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var _airportService = new AirportService(_mapper, dbContext);
            var _flightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);
            // Arrange
            var expectedCount = 10;

            // Act

            var result = await _flightService.GetAllFlights();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Data.Model.Flight>>(result);
            Assert.Equal(expectedCount, result.Count());
        }
        [Fact]
        public async Task GetFlight_ReturnsFlight()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var _airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var _airportService = new AirportService(_mapper, dbContext);
            var _flightService = new FlightService(_mapper, dbContext, _airplaneService, _airportService);

            var country = new Country
            {
                CountryName = "country 1",
                PublicId = Guid.NewGuid().ToString()
            };
            var city = new City
            {
                CityName = "A",
                Country = country,
                PublicId = Guid.NewGuid().ToString()
            };
            var city2 = new City
            {
                CityName = "B",
                Country = country,
                PublicId = Guid.NewGuid().ToString()
            };
            var airport1 = new Airport
            {
                AirportName = "Airport 1",
                PublicId = Guid.NewGuid().ToString(),
                City=city
            };

            var airport2 = new Airport
            {
                City = city2,
                AirportName = "Airport 2",
                PublicId = Guid.NewGuid().ToString()
            };
            var airplaneType = new AirplaneType
            {
                PublicId = Guid.NewGuid().ToString(),
                SymbolInRegistrationNumber = 'Z',
                NumberOfSeats = 100,
                Name = "Boeing737"
            };
          
            var airplane = new Airplane
            {
                RegistrationNumber = "SP-L" +airplaneType.SymbolInRegistrationNumber+"A",
                AirplaneType=airplaneType,
                PublicId = Guid.NewGuid().ToString()
            };

         
            dbContext.Airports.Add(airport1);
            dbContext.Airports.Add(airport2);
            dbContext.AirplaneType.Add(airplaneType);
            dbContext.Airplane.Add(airplane);
            dbContext.SaveChanges();

            var newFlight = new Data.Model.Flight
            {

                FlightNumber = "AA999",
                Id = 11, 
                PublicId = Guid.NewGuid().ToString(), 
                ArrivalPoint = airport1,
                DepartureDate = DateTime.Now.AddDays(1),
                DeparturePoint = airport2,
                Airplane = airplane 
            };
            dbContext.Flight.Add(newFlight);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await _flightService.GetFlight(newFlight.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Data.Model.Flight>(result); 
            Assert.Equal(newFlight.Id, result.Id);
        }

        [Fact]
        public async Task CreateAsync_ValidFlightModel_ReturnsNewFlight()
        {
            // Arrange
            var flightModel = new FlightModel
            {
                FlightNumber = "AA999",
                DepartureDate = DateTime.UtcNow.AddDays(1), 
                DeparturePointId = 1, 
                ArrivalPointId = 2,
                AirplaneId = 1 
            };

            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var airportService = new AirportService(_mapper, dbContext);
            var flightService = new FlightService(_mapper, dbContext, airplaneService, airportService);


            // Act
            var result = await flightService.CreateAsync(flightModel);

            // Assert
            var createdResult = Assert.IsType<Data.Model.Flight>(result);
            Assert.Equal(flightModel.DeparturePointId, createdResult.DeparturePoint.Id); 
            Assert.Equal(flightModel.ArrivalPointId, createdResult.ArrivalPoint.Id);
            Assert.Equal(flightModel.AirplaneId, createdResult.Airplane.Id); 
                                                                             
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
            var airplaneTypeService = new AirplaneTypeService(_mapper,dbContext);
            var airplaneService = new AirplaneService(_mapper, dbContext,airplaneTypeService);
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
            int existingFlightId = 1; 
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
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
            int nonExistingFlightId = 999; 
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);
            var airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeService);
            var airportService = new AirportService(_mapper, dbContext);
            var flightService = new FlightService(_mapper, dbContext, airplaneService, airportService);
            var controller = new FlightController(flightService);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await flightService.DeleteById(nonExistingFlightId));
        }
    }





}
