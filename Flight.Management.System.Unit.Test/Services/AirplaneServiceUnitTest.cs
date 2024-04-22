using AutoMapper;
using Flight.Management.System.API.Models;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.Data.Model;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Flight.Management.System.Unit.Test.Services
{
    public class AirplaneServiceUnitTest
    {
        private readonly TestDatabase _testDatabase;

        private readonly IMapper _mapper;

        public AirplaneServiceUnitTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperInitializator>();
            });

            _testDatabase = new TestDatabase();
            _mapper = new Mapper(configuration);
        }
        [Fact]
        public async Task CreateAirplane_ValidModel_ReturnsCreatedAirplane()
        {
            // Arrange
            var airplaneModel = new AirplaneModel
            {
                AirplaneTypeId = 1
            };
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeServiceMock = new Mock<AirplaneTypeService>(_mapper,dbContext);

            var airplaneService = new AirplaneService(_mapper, dbContext, airplaneTypeServiceMock.Object);

            // Act
            var result = await airplaneService.CreateAsync(airplaneModel);

            // Assert
            Assert.NotNull(result);
           
        }
    }
}
