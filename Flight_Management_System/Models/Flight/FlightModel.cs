using Flight.Management.System.Data.Model;

namespace Flight.Management.System.API.Models.Flight
{
    public class FlightModel
    {
        public uint FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public int DeparturePointId { get; set; }
        public int ArrivalPointId { get; set; }
        public int AirplaneId { get; set; }
    }
}
