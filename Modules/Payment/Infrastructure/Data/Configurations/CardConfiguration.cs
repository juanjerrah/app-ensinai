using app_ensinai.Modules.Payment.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app_ensinai.Modules.Payment.Infrastructure.Data.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("cards", "payment");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
            .HasColumnName("id");

            builder.Property(c => c.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(c => c.Token)
                .HasColumnName("token")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Brand)
                .HasColumnName("brand")
                .IsRequired();

            builder.Property(c => c.LastFour)
                .HasColumnName("last_four")
                .IsRequired()
                .HasMaxLength(4);

            builder.Property(c => c.ExpirationMonth)
                .HasColumnName("expiration_month")
                .IsRequired();

            builder.Property(c => c.ExpirationYear)
                .HasColumnName("expiration_year")
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
            
            builder.HasIndex(c => c.UserId);
        }
    }
}
