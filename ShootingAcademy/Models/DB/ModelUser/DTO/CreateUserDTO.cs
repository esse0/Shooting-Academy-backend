using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ShootingAcademy.Models.Controllers.Auth;

namespace ShootingAcademy.Models.DB.ModelUser.DTO
{
    public class CreateUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecoundName { get; set; }

        [Required]
        public int? Age { get; set; }
        public string Grade { get; set; }
        public string Country { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        public Guid RoleId { get; set; }

        public static User ToUser(AuthRequest createUserDTO)
        {
            return new User
            {
                FirstName = createUserDTO.name,
                SecoundName = createUserDTO.lastName,
                Age = null,
                Grade = null,
                Email = createUserDTO.Email,
                PasswordHash = createUserDTO.password,
                RoleId = Guid.NewGuid(),
            };
        }
    }
}