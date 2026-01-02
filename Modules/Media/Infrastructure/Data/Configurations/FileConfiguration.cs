using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FileEntity = app_ensinai.Modules.Media.Domain.Models.File;

namespace app_ensinai.Modules.Media.Infrastructure.Data.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.ToTable("files", "media");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasColumnName("id");

            builder.Property(f => f.FileName)
                .HasColumnName("file_name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(f => f.FileSize)
                .HasColumnName("file_size")
                .IsRequired();

            builder.Property(f => f.ContentType)
                .HasColumnName("content_type")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Bucket)
                .HasColumnName("bucket")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.FileType)
                .HasColumnName("file_type")
                .IsRequired();

            builder.Property(f => f.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(f => f.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        }
    }
}
