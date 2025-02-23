using ShootingAcademy.Models.DB.ModelRole;
using ShootingAcademy.Models.DB.ModelUser.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB.ModelUser
{
    public class User
    {
        [Key]
        public Guid Id { get; set;  }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecoundName { get; set; }
        [Required]
        public string PatronymicName { get; set; }

        [Required]
        public int Age { get; set; }

        public string Grade { get; set; }
        public string Country { get; set; }

        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }

        public Guid RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        public List<GroupMember> AthleteGroups { get; set; } = new();
        public List<Course> Courses { get; set; } = new();
      
        public static GetUserDto ToGetUserDto(User user)
        {
            return new GetUserDto
            {
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                RoleId = user.RoleId,
            };
        }
    }
}
