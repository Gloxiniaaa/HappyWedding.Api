using HappyWedding.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWedding.Api.Data.Configurations;

public class WeddingConfiguration : IEntityTypeConfiguration<Wedding>
{
    public void Configure(EntityTypeBuilder<Wedding> builder)
    {
        builder.ToTable("Weddings");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasDefaultValueSql("gen_random_uuid()"); // PostgreSQL

        builder.HasIndex(w => w.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Weddings_UserId");

        builder.Property(w => w.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(w => w.Name1)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.Name2)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.Location)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(w => w.Tagline)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(w => w.Date)
            .IsRequired()
            .HasColumnType("date"); // PostgreSQL: date only, no time component

        // delete wedding → delete its milestones
        builder.HasMany(w => w.Milestones)
            .WithOne(m => m.Wedding)
            .HasForeignKey(m => m.WeddingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}