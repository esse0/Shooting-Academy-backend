using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.Controllers.Auth
{
    public class LoginForm
    {

        [Required(ErrorMessage = "UserEmail is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
