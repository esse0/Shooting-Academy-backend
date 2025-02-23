using ShootingAcademy.Models.DB.ModelUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB
{
    public class AthleteGroup
    {
        [Key]
        public string Id { get; set; }

        public string CoatchId { get; set; }
        [ForeignKey(nameof(CoatchId))]
        public User Coatch { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<GroupMember> Athletes { get; set; }
    }
}
