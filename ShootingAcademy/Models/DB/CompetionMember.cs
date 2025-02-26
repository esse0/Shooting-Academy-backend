using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShootingAcademy.Models.DB.ModelUser;

namespace ShootingAcademy.Models.DB
{
    public class CompetionMember
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CompetionId { get; set; }
        [ForeignKey(nameof(CompetionId))]
        public Competion Competion { get; set; }

        public Guid AthleteId { get; set; }
        [ForeignKey(nameof(AthleteId))]
        public User Athlete { get; set; }

        public float? Result { get; set; }
    }
}
