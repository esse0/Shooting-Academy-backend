using ShootingAcademy.Models.DB.ModelRole;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB.ModelUser
{
    [Table("User")]
    public class User
    {
        public int Id { get; set;  }

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
        public Role Role { get; set; }

    }
}
