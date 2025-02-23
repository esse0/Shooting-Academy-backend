using ShootingAcademy.Models.DB.ModelUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB.ModelModule
{
    public class Module
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Lesson> Lessons { get; set; } = [];
    }
}
