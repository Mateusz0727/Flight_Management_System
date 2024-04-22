using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Flight.Management.System.API.Models.Airplane
{
    public class AirplaneTypeFormModel
    {
        [Required(ErrorMessage = "The name of the airplane type is required.")]
        [NotNull]
        public string Name { get; set; }

        [NotNull]
        [Required(ErrorMessage = "The Symbol In Registration Number of the airplane type is required.")]
        public char SymbolInRegistrationNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "NumberOfSeats must be greater than 0.")]
        [Required(ErrorMessage = "The Number Of Seats of the airplane type is required.")]
        [NotNull]
        public uint NumberOfSeats { get; set; }
    }
   
}
