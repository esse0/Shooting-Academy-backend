using ShootingAcademy.Models.DB.ModelUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB
{
    public class Competion
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        public string MaxMembersCount { get; set; } 

        public string Venue { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public string Exsecise { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public User Organization { get; set; }

        public string Status { get; set; }

        public List<User> Members { get; set; } = [];
    }
}
