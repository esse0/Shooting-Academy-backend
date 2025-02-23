using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelUser.DTO
{
    public class UpdateUserDTO
    {

        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        [Required]
        public required string SportsCategory { get; set; }
        public string RoleId { get; set; }

        public static User ToUser(UpdateUserDTO updateUserDTO)
        {
            return new User
            {
                Email = updateUserDTO.Email,
                PasswordHash = updateUserDTO.PasswordHash,
                SportsCategory = updateUserDTO.SportsCategory,
                RoleId = updateUserDTO.RoleId,
            };
        }
    }
}
