using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.DB.Role;
using ShootingAcademy.Models.DB.User;

namespace ShootingAcademy.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<DB.User.User> users { get; set; }
        public DbSet<DB.Role.Role> roles { get; set; }


    }
}
