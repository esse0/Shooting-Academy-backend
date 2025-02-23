using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelUser.DTO
{
    public class GetUserDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public int Age { get; set; }

        public string Grade { get; set; }
        public string Country { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        public Guid RoleId { get; set; }

        public static User ToUser(GetUserDto getUserDto)
        {
            return new User
            {
                FullName = getUserDto.FullName,
                Age = getUserDto.Age,
                Grade = getUserDto.Grade,
                Email = getUserDto.Email,
                PasswordHash = getUserDto.PasswordHash,
                RoleId = getUserDto.RoleId,
            };
        }
    }
}
