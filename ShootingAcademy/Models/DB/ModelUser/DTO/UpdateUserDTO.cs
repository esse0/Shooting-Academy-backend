using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelUser.DTO
{
    public class UpdateUserDTO
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

        public static User ToUser(UpdateUserDTO updateUserDTO)
        {
            return new User
            {
                FullName = updateUserDTO.FullName,
                Age = updateUserDTO.Age,
                Grade = updateUserDTO.Grade,
                Email = updateUserDTO.Email,
                PasswordHash = updateUserDTO.PasswordHash,
                RoleId = updateUserDTO.RoleId,
            };
        }
    }
}
