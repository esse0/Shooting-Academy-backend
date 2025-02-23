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
        public Guid RoleId { get; set; }

        public static User ToUser(GetUserDto getUserDto)
        {
            return new User
            {
                Email = getUserDto.Email,
                PasswordHash = getUserDto.PasswordHash,
                RoleId = getUserDto.RoleId,
            };
        }
    }
}
