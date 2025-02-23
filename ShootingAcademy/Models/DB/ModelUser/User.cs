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
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        public List<GroupMember> AthleteGroups { get; set; } = [];
        public List<Course> Courses { get; set; } = [];
      
        public static GetUserDto ToGetUserDto(User user)
        {
            return new GetUserDto
            {
                FirstName = user.FirstName,
                SecoundName = user.SecoundName,
                PatronymicName = user.PatronymicName,
                Age = user.Age,
                Grade = user.Grade,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                RoleId = user.RoleId,
            };
        }
    }
}
