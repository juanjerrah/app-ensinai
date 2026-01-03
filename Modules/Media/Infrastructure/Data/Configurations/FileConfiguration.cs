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

            builder.Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(f => f.FileSize)
                .IsRequired();

            builder.Property(f => f.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Bucket)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.FileType)
                .IsRequired();
        }
    }
}
