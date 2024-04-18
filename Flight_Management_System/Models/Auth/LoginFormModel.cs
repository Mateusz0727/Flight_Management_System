using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Auth
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is requirde")]
        public string Password { get; set; }
    }
}
