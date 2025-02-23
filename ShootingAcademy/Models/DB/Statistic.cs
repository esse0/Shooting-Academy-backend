using ShootingAcademy.Models.DB.ModelUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB
{
    public class Statistic
    {
        [Key]
        public string Id { get; set; }

        public string AthleteId { get; set; }
        [ForeignKey(nameof(AthleteId))]
        public User Athlete { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
