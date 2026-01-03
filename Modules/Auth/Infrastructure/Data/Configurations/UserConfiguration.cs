using app_ensinai.Modules.Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app_ensinai.Modules.Auth.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "auth");

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(u => u.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        builder.Property(u => u.ProfileType)
            .HasColumnName("profile_type")
            .IsRequired();

        builder.Property(u => u.ShortDescription)
            .HasColumnName("short_description")
            .HasMaxLength(200);

        builder.Property(u => u.DetailedDescription)
            .HasColumnName("detailed_description")
            .HasMaxLength(1000);

        builder.Property(u => u.Active)
            .HasColumnName("active")
            .IsRequired();
        builder.Property(u => u.Salt)
            .HasColumnName("salt")
            .IsRequired();

        builder.Property(u => u.RefreshToken)
            .HasColumnName("refresh_token")
            .IsRequired();

        builder.Property(u => u.ExpiryTime)
            .HasColumnName("expiry_time")
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Active);

        builder.HasIndex(u => u.ProfileType);

    }
}
