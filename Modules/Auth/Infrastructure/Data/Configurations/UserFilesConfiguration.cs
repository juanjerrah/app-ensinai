using app_ensinai.Modules.Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app_ensinai.Modules.Auth.Infrastructure.Data.Configurations
{
    public class UserFilesConfiguration : IEntityTypeConfiguration<UserFiles>
    {
        public void Configure(EntityTypeBuilder<UserFiles> builder)
        {
            builder.ToTable("user_files", "auth");

            builder.HasKey(uf => uf.Id);

            builder.Property(t => t.Id)
            .HasColumnName("id")
            .IsRequired();

            builder.Property(uf => uf.Purpose)
                .HasColumnName("purpose")
                .IsRequired();

            builder.Property(uf => uf.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(uf => uf.FileId)
                .HasColumnName("file_id")
                .IsRequired();

            builder.Property(uf => uf.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(uf => uf.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.HasOne(x => x.User)
            .WithMany(u => u.UserFiles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            // ✅ FileId é apenas uma coluna - sem FK para outro módulo
            // A relação com o módulo Media é apenas lógica, não de banco

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("idx_auth_user_files_user");

            builder.HasIndex(x => new { x.UserId, x.Purpose })
                .IsUnique()
                .HasDatabaseName("idx_auth_user_files_user_purpose");


        }
    }
}