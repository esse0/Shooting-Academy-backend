using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelUser.DTO
{
    public class FullUserModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string SecoundName { get; set; }
        public string PatronymicName { get; set; }

        public int Age { get; set; }

        public string Grade { get; set; }
        public string Country { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
