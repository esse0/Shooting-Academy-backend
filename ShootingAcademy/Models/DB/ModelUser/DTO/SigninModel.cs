using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models
{
    public class SigninModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
