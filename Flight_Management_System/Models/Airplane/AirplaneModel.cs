using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Airplane
{
    public class AirplaneModel
    {

        [Required(ErrorMessage = "The name of the airplane type is required.")]
        public int AirplaneTypeId { get; set; }
    }
}
