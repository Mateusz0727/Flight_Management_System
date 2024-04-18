using System.ComponentModel.DataAnnotations;

namespace Flight.Management.System.API.Models.Auth
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Given name is required")]
        public string GivenName { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is requirde")]
        public string Password { get; set; }
    }
}
