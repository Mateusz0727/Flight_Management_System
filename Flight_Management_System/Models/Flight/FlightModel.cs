﻿using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Flight
{
    public class FlightModel
    {
        [MaxLength(6)]
        [Required(ErrorMessage = "Flight numeber is required")]
        public FlightNumber FlightNumber { get; set; }

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
