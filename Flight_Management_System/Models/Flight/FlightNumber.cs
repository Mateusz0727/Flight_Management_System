using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Flight
{
    public class FlightNumber
    {
        [MaxLength(2)]
        public string AirlineCode { get; set; }
        [MaxLength(4)]
        public int FlightNumberCode { get; set; }
    }
}
