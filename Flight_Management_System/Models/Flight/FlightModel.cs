using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Flight
{
    public class FlightModel
    {
        [RegularExpression(@"^[A-Za-z]{1,2}\d{2,4}$", ErrorMessage = "Flight number must consist of at least one but not more than two letters and at least two but not more than four digits.")]
        [Required(ErrorMessage = "Flight number is required")]
        [MaxLength(6)]
        public string FlightNumber { get; set; }

        [Required(ErrorMessage = "Departure date is required", AllowEmptyStrings = false)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Departure point is required")]
        public int DeparturePointId { get; set; }

        [Required(ErrorMessage = "Arrival point is required")]
        public int ArrivalPointId { get; set; }

        [Required(ErrorMessage = "Airplane is required")]
        public int AirplaneId { get; set; }
    }
}
