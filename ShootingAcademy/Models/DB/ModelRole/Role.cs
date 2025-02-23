using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelRole
{
    public class Role
    {
        [Key]
        public string Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        [Required]
        public required string Name;
    }
}
