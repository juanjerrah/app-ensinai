using app_ensinai.Modules.Auth.Domain.Models;
using app_ensinai.Modules.Auth.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace app_ensinai.Modules.Auth.Infrastructure.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new UserFilesConfiguration());
            modelBuilder.ApplyConfiguration(new InterestAreaConfiguration());
            modelBuilder.ApplyConfiguration(new UserInterestAreaConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
