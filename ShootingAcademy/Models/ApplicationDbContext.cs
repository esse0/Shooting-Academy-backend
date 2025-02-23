using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.DB.ModelRole;
using ShootingAcademy.Models.DB.ModelUser;

namespace ShootingAcademy.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }


    }
}
