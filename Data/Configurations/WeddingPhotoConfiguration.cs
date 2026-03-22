using HappyWedding.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWedding.Api.Data.Configurations;


public class WeddingPhotoConfiguration : IEntityTypeConfiguration<WeddingPhoto>
{
    public void Configure(EntityTypeBuilder<WeddingPhoto> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ImageUrl).IsRequired();
        builder.Property(p => p.PublicId).IsRequired();

        builder.Property(p => p.AspectRatio)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("1:1");

        builder.Property(p => p.Caption).HasMaxLength(300);

        builder.HasOne(p => p.Wedding)
            .WithMany()
            .HasForeignKey(p => p.WeddingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}