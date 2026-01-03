using app_ensinai.Modules.Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app_ensinai.Modules.Auth.Infrastructure.Data.Configurations
{
    public class UserInterestAreaConfiguration : IEntityTypeConfiguration<UserInterestArea>
    {
        public void Configure(EntityTypeBuilder<UserInterestArea> builder)
        {
            builder.ToTable("user_interest_areas", "auth");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.InterestAreaId)
                .HasColumnName("interest_area_id")
                .IsRequired();
            

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserInterestAreas)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.InterestArea)
                .WithMany()
                .HasForeignKey(x => x.InterestAreaId);

            //Index
            builder.HasIndex(x => new { x.UserId, x.InterestAreaId });
        }
    }
}