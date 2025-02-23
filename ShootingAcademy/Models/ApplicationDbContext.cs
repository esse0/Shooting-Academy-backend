using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.DB;
using ShootingAcademy.Models.DB.ModelCompetition;
using ShootingAcademy.Models.DB.ModelCourse;
using ShootingAcademy.Models.DB.ModelFeatures;
using ShootingAcademy.Models.DB.ModelFraq;
using ShootingAcademy.Models.DB.ModelLesson;
using ShootingAcademy.Models.DB.ModelModule;
using ShootingAcademy.Models.DB.ModelRole;
using ShootingAcademy.Models.DB.ModelUser;

namespace ShootingAcademy.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<AthleteGroup> AthleteGroups { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Fraq> Fraqs { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Competion> Competions { get; set; }
        public DbSet<CompetionMember> CompetionMembers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
