using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Airplane
{
    public class AirplaneModel
    {
        [Required(ErrorMessage = "The name of the airplane is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The registration number is required.")]
        [RegularExpression(@"^SP-[A-Z]{2}[0-9]{2}$", ErrorMessage = "Invalid registration number format. Example: SP-LR01")]
        public string RegistrationNumber { get; set; }
    }
}
