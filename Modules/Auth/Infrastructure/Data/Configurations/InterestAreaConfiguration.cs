using app_ensinai.Modules.Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app_ensinai.Modules.Auth.Infrastructure.Data.Configurations
{
    public class InterestAreaConfiguration : IEntityTypeConfiguration<InterestArea>
    {
        public void Configure(EntityTypeBuilder<InterestArea> builder)
        {
            builder.ToTable("interest_areas", "auth");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}