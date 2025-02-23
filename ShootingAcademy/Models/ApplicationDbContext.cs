using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.DB.ModelRole;
using ShootingAcademy.Models.DB.ModelUser;

namespace ShootingAcademy.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
