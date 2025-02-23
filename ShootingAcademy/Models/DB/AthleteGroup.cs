using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShootingAcademy.Models.DB.ModelUser;

namespace ShootingAcademy.Models.DB
{
    public class AthleteGroup
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CoatchId { get; set; }
        [ForeignKey(nameof(CoatchId))]

        public string OrganizationName { get; set; }

        public User Coatch { get; set; }

        public List<GroupMember> Athletes { get; set; } = [];
    }
}
