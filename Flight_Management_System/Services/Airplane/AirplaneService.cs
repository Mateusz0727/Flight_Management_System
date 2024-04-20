using AutoMapper;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Models.Flight;
using Flight.Management.System.Data.Model;
using Microsoft.EntityFrameworkCore;
using FlightData = Flight.Management.System.Data.Model;

namespace Flight.Management.System.API.Services.Airplane
{
    public class AirplaneService:BaseService
    {
        private readonly AirplaneTypeService airplaneTypeService;

        public AirplaneService(IMapper mapper, BaseContext context,AirplaneTypeService airplaneTypeService) : base(mapper, context)
        {
            this.airplaneTypeService = airplaneTypeService;
        }
        public async Task<FlightData.Airplane> GetAirplane(int id)
        {
            return Context.Airplane.Where(x => x.Id == id).Include(x=>x.AirplaneType).FirstOrDefault();
        }
        public async Task<FlightData.Airplane> CreateAsync(AirplaneModel airplaneModel)
        {
            var entity = Mapper.Map<FlightData.Airplane>(airplaneModel);
            try
            {
                entity.AirplaneType = await this.airplaneTypeService.GetAirplaneType(airplaneModel.AirplaneTypeId);
                if (entity.AirplaneType == null)
                    throw new Exception("This airplane type doesn't exist");
              
                entity.RegistrationNumber= await CreateRegistrationNumber(entity);
                entity.AirplaneType = await this.airplaneTypeService.GetAirplaneType(airplaneModel.AirplaneTypeId);
                entity.PublicId = Guid.NewGuid().ToString();
                Context.Add(entity);
                Context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {              
                throw new Exception($"Error occurred while creating airplane: {ex.Message}");
                throw; 
            }

        }
        public async Task<string> CreateRegistrationNumber(FlightData.Airplane entity)
        {

           return "SP-L" + entity.AirplaneType.SymbolInRegistrationNumber.ToString() +  this.airplaneTypeService.GetNextAlphabetLetterForAirplaneType(entity.AirplaneType.Id);
        }
       
    }
}
