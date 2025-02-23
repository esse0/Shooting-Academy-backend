using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelRole
{
    [Table("Role")]
    public class Role
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        [Required]
        public required string Name;
    }
}
