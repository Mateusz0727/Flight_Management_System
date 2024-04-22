using AutoMapper;
using Flight.Management.System.API.Models;
using Flight.Management.System.API.Services.Airport;
using Flight.Management.System.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Management.System.Unit.Test.Services
{
    public class AirportServiceUnitTest
    {
        private readonly TestDatabase _testDatabase;

        private readonly IMapper _mapper;
        public AirportServiceUnitTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperInitializator>();
            });
            _testDatabase = new TestDatabase();
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public async Task GetAirport_ValidId_ReturnsAirport()
        {
            // Arrange
            var dbContext = await _testDatabase.GetDatabaseContext();

            var _airportService = new AirportService(_mapper, dbContext);
            var expectedAirport = new Airport
            {
                PublicId = Guid.NewGuid().ToString(),
                Id = 20,
                AirportName = "Test Airport",
                City = new City
                {
                    PublicId = Guid.NewGuid().ToString(),
                    Id = 20,
                    CityName = "Test City",
                    Country = new Country
                    {
                        PublicId= Guid.NewGuid().ToString(),
                        Id = 20,
                        CountryName = "Test Country"
                    }
                }
            };
          
            
           

            dbContext.Airports.Add(expectedAirport);
            dbContext.SaveChanges();
            // Act
            var result = await _airportService.GetAirport(20);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAirport.Id, result.Id);
            Assert.Equal(expectedAirport.AirportName, result.AirportName);
            Assert.NotNull(result.City);
            Assert.Equal(expectedAirport.City.Id, result.City.Id);
            Assert.Equal(expectedAirport.City.CityName, result.City.CityName);
            Assert.NotNull(result.City.Country);
            Assert.Equal(expectedAirport.City.Country.Id, result.City.Country.Id);
            Assert.Equal(expectedAirport.City.Country.CountryName, result.City.Country.CountryName);
        }
    }
}
