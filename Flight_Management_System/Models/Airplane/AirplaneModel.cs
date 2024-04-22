using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Airplane
{
    public class AirplaneModel
    {

        [Required(AllowEmptyStrings =false)]
        [Range(1, int.MaxValue, ErrorMessage = "The name of the airplane type is required.")]
        public int AirplaneTypeId { get; set; }
    }
}
