using ShootingAcademy.Models.DB.ModelRole;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelUser.DTO
{
    public class CreateUserDTO
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        [Required]
        public required string SportsCategory { get; set; }
        public string RoleId { get; set; }

        public static User ToUser(CreateUserDTO createUserDTO)
        {
            return new User
            {
                Email = createUserDTO.Email,
                PasswordHash = createUserDTO.PasswordHash,
                SportsCategory = createUserDTO.SportsCategory,
                RoleId = createUserDTO.RoleId,
            }
        }
    }
}
