using ShootingAcademy.Models.DB.ModelRole;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB.ModelUser
{
    public class User
    {
        [Key]
        public string Id { get; set;  }

        [Column(TypeName = "varchar(30)")]
        [Required]
        public required string Email { get; set; }
        [Column(TypeName = "varchar(30)")]
        [Required]
        public required string PasswordHash { get; set; }
        [Column(TypeName = "varchar(30)")]
        [Required]
        public required string SportsCategory { get; set; }
        public string RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }



        public List<GroupMember> AthleteGroups { get; set; } = new();
        public List<Statistic> Statistics { get; set; } = new();
        public List<Course> Courses { get; set; } = new();
    }
}
