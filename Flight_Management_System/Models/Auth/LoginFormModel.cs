using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Auth
{
    public class LoginFormModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
