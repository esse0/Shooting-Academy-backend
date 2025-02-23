using ShootingAcademy.Models.DB.ModelUser;
using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public User Athlete { get; set; }

        [Required]
        public AthleteGroup AthleteGroup { get; set; }
    }
}
