using AutoMapper;
using Flight.Management.System.Data.Model;
using Microsoft.EntityFrameworkCore;
using FlightData= Flight.Management.System.Data.Model;

namespace Flight.Management.System.API.Services.Airport
{
    public class AirportService:BaseService
    {
        public AirportService(IMapper mapper ,BaseContext baseContext):base(mapper,baseContext)
        {

        }
        public async Task<FlightData.Airport> GetAirport(int id)
        {
            return Context.Airports.Where(x=>x.Id==id).Include(x=>x.City).ThenInclude(x=>x.Country).FirstOrDefault();
        }
    }
}
