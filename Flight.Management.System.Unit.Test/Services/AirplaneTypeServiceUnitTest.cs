using AutoMapper;
using Flight.Management.System.API.Models;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Services.Airplane;

namespace Flight.Management.System.Unit.Test.Services
{
    public class AirplaneTypeServiceUnitTest
    {
        private readonly TestDatabase _testDatabase;

        private readonly IMapper _mapper;
        public AirplaneTypeServiceUnitTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperInitializator>();
            });

            _testDatabase = new TestDatabase();
            _mapper = new Mapper(configuration);
        }
        [Fact]
        public async Task CreateAirplaneType_ValidModel_ReturnsCreatedAirplaneType()
        {
            var airplaneTypeFormModel = new AirplaneTypeFormModel
            {
              SymbolInRegistrationNumber='Z',
              Name="test airplanetype",
              NumberOfSeats=100,
            };
            var dbContext = await _testDatabase.GetDatabaseContext();
            var airplaneTypeService = new AirplaneTypeService(_mapper, dbContext);

            // Act
            var result = await airplaneTypeService.CreateAsync(airplaneTypeFormModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(airplaneTypeFormModel.Name, result.Name);
            Assert.Equal(airplaneTypeFormModel.NumberOfSeats, result.NumberOfSeats);
            Assert.Equal(airplaneTypeFormModel.SymbolInRegistrationNumber, result.SymbolInRegistrationNumber);

        }

    }
}
