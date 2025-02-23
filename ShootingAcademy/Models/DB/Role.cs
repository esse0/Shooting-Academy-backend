using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        [Required]
        public required string Name;
    }
}
