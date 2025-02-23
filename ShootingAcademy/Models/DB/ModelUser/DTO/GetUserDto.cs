using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelUser.DTO
{
    public class GetUserDto
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        [Required]
        public required string SportsCategory { get; set; }
        public string RoleId { get; set; }

        public static User ToUser(GetUserDto getUserDto)
        {
            return new User
            {
                Email = getUserDto.Email,
                PasswordHash = getUserDto.PasswordHash,
                SportsCategory = getUserDto.SportsCategory,
                RoleId = getUserDto.RoleId,
            };
        }
    }
}
