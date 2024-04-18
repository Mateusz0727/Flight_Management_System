using AutoMapper;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Models.Flight;
using Flight.Management.System.Data.Model;
using FlightData = Flight.Management.System.Data.Model;

namespace Flight.Management.System.API.Services.Airplane
{
    public class AirplaneService:BaseService
    {
        public AirplaneService(IMapper mapper, BaseContext context) : base(mapper, context)
        {
           
        }
        public async Task<FlightData.Airplane> GetAirplane(int id)
        {
            return Context.Airplane.FirstOrDefault(x => x.Id == id);
        }
        public async Task<FlightData.Airplane> CreateAsync(AirplaneModel airplaneModel)
        {
            var entity = Mapper.Map<FlightData.Airplane>(airplaneModel);
            try
            {
                entity.PublicId = Guid.NewGuid().ToString();
                Context.Add(entity);
                Context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {              
                Console.WriteLine($"Error occurred while creating airplane: {ex.Message}");
                throw; 
            }

        }

       
    }
}
