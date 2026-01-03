using app_ensinai.Modules.Payment.Domain.Models;
using app_ensinai.Modules.Payment.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace app_ensinai.Modules.Payment.Infrastructure.Data
{
    public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
    {
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CardConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    }
}
