using AutoMapper;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Flight.Management.System.API.Services.Airplane
{
    public class AirplaneTypeService:BaseService
    {
       

        public AirplaneTypeService(IMapper mapper, BaseContext context) : base(mapper, context)
        {
        }
        public async Task<AirplaneType> GetAirplaneType(int id)
        {
            return Context.AirplaneType.Where(x => x.Id == id).FirstOrDefault();
        }
        public char GetNextAlphabetLetterForAirplaneType(int id)
        {
            // Pobierz wszystkie samoloty danego typu
            var airplanesOfType = Context.AirplaneType.Where(a => a.Id == id).ToList();

            // Oblicz kolejną literę alfabetu
            char nextLetter = (char)('A' + airplanesOfType.Count);

            return nextLetter;
        }
        public async Task<AirplaneType> CreateAsync(AirplaneTypeFormModel airplaneTypeFormModel)
        {
            var entity = Mapper.Map<AirplaneType>(airplaneTypeFormModel);
            try
            {
                entity.PublicId = Guid.NewGuid().ToString();
                Context.AirplaneType.Add(entity);
                Context.SaveChanges();
                return entity;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
