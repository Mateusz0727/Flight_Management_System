using AutoMapper;
using Flight.Management.System.API.Models.Flight;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.Data.Model;
using FlightData = Flight.Management.System.Data.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Flight.Management.System.API.Services.Airport;

namespace Flight.Management.System.API.Services.Flight
{
    public class FlightService : BaseService
    {
        private readonly AirplaneService airplaneService;
        private readonly AirportService airportService;

        public FlightService( IMapper mapper,BaseContext context,AirplaneService airplaneService,AirportService airportService) : base(mapper,context)
        {
            this.airplaneService = airplaneService;
            this.airportService = airportService;
        }
        #region get functions
        public List<FlightData.Flight> GetAllFlights()
            
        {
            return Context.Flight
                .Include(x=>x.Airplane)
                .Include(x=>x.ArrivalPoint)
                 .ThenInclude(x => x.City).ThenInclude(x => x.Country)
                .Include(x=>x.DeparturePoint)
                .ThenInclude(x=>x.City).ThenInclude(x=>x.Country)
                .ToList();
        }

        public FlightData.Flight GetFlight(int id)
        {
            return Context.Flight.Where(x => x.Id == id).Include(x => x.Airplane).FirstOrDefault();
        }
        public FlightData.Flight GetFlight(string id)
        {
            return Context.Flight.Where(x=>x.PublicId==id).Include(x=>x.Airplane).FirstOrDefault();
        }
        #endregion
        public async Task<FlightData.Flight> CreateAsync(FlightModel flightModel)
        {
            var entity = Mapper.Map<FlightData.Flight>(flightModel);
            try
            {
                var airplane = await this.airplaneService.GetAirplane(entity.Airplane.Id);
                if(entity.DepartureDate<DateTime.UtcNow)
                    throw new Exception("Nie można ustawić lotu w przeszłości");
                if (entity.DeparturePoint.Id == entity.ArrivalPoint.Id)
                    throw new Exception("Nie można ustawić miejsca odlotu i przylotu tego samego");
                var departurepoint = await this.airportService.GetAirport(entity.DeparturePoint.Id);
                var arrivalpoint = await this.airportService.GetAirport(entity.ArrivalPoint.Id);

                entity.DeparturePoint = departurepoint;
                entity.ArrivalPoint = arrivalpoint;
                entity.Airplane = airplane;

                entity.PublicId = Guid.NewGuid().ToString();

                Context.Add(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Airplane"))
                    throw new Exception("Podany samolot nie istnieje");
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_Flight_Airport_ArrivalPointId"))
                    throw new Exception("Podany punkt przylotu nie istnieje");
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_Flight_Airport_DeparturePointId"))
                    throw new Exception("Podany punkt odlotu nie istnieje");
                throw; 
            }
            return entity;
        }


        public void DeleteById(int id)
        {
            var entity = GetFlight(id);
            Context.Remove(entity);
            Context.SaveChanges();
        }

    }
}
