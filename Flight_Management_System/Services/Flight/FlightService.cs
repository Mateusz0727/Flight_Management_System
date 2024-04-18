using AutoMapper;
using Flight.Management.System.API.Models.Flight;
using Flight.Management.System.API.Services.Airplane;
using Flight.Management.System.API.Services.Airport;
using Flight.Management.System.Data.Model;
using Microsoft.EntityFrameworkCore;
using FlightData = Flight.Management.System.Data.Model;

namespace Flight.Management.System.API.Services.Flight
{
    public class FlightService : BaseService
    {
        private readonly AirplaneService airplaneService;
        private readonly AirportService airportService;

        public FlightService(IMapper mapper, BaseContext context, AirplaneService airplaneService, AirportService airportService) : base(mapper, context)
        {
            this.airplaneService = airplaneService;
            this.airportService = airportService;
        }
        #region get functions
        public async Task<List<FlightData.Flight>> GetAllFlights()

        {
            return Context.Flight
                .Include(x => x.Airplane)
                .Include(x => x.ArrivalPoint)
                 .ThenInclude(x => x.City).ThenInclude(x => x.Country)
                .Include(x => x.DeparturePoint)
                .ThenInclude(x => x.City).ThenInclude(x => x.Country)
                .ToList();
        }

        public async Task<FlightData.Flight> GetFlight(int id)
        {
            return Context.Flight.Where(x => x.Id == id).Include(x => x.Airplane).FirstOrDefault();
        }
        public FlightData.Flight GetFlight(string id)
        {
            return Context.Flight.Where(x => x.PublicId == id).Include(x => x.Airplane).FirstOrDefault();
        }
        #endregion
        public async Task<FlightData.Flight> CreateAsync(FlightModel flightModel)
        {
            var entity = Mapper.Map<FlightData.Flight>(flightModel);
            try
            {
                var airplane = await this.airplaneService.GetAirplane(entity.Airplane.Id);
                if(airplane==null)
                    throw new Exception("The entered aircraft doesn't exist");

                if (entity.DepartureDate < DateTime.UtcNow)
                    throw new Exception("Invalid date (cannot set date in the past)");

                if (entity.DeparturePoint.Id == entity.ArrivalPoint.Id)
                    throw new Exception("You cannot set the same departure and arrival points");

                var departurepoint = await this.airportService.GetAirport(entity.DeparturePoint.Id);
                if(departurepoint==null)
                    throw new Exception("The entered departure point doesn't exist");

                var arrivalpoint = await this.airportService.GetAirport(entity.ArrivalPoint.Id);
                if (arrivalpoint == null)
                    throw new Exception("The entered arrival point doesn't exist");
                entity.FlightNumber = flightModel.FlightNumber.AirlineCode+flightModel.FlightNumber.ToString();
                entity.DeparturePoint = departurepoint;
                entity.ArrivalPoint = arrivalpoint;
                entity.Airplane = airplane;

                entity.PublicId = Guid.NewGuid().ToString();

                Context.Add(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
               
                throw;
            }
            return entity;
        }


        public async Task DeleteById(int id)
        {
            try
            {
                var entity =await GetFlight(id);
                if (entity != null)
                {

                    Context.Remove(entity);
                    Context.SaveChanges();
                
                }
                else
                    throw new Exception("The specified aircraft doesn't exist");
            }
            catch(Exception ex)
            {
                throw;
            }
        }


    }
}
