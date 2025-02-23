using ShootingAcademy.Models.DB.ModelUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB
{
    public class GroupMember
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AthleteId { get; set; }
        [ForeignKey(nameof(AthleteId))]
        public User Athlete { get; set; }

        [Required]
        public Guid AthleteGroupId { get; set; }
        [ForeignKey(nameof(AthleteId))]
        public AthleteGroup AthleteGroup { get; set; }
    }
}
