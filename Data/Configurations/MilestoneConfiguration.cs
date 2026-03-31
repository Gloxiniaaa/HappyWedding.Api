using HappyWedding.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWedding.Api.Data.Configurations;

public class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
{
    public void Configure(EntityTypeBuilder<Milestone> builder)
    {
        builder.ToTable("Milestones");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasDefaultValueSql("gen_random_uuid()"); // PostgreSQL

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Subtitle)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(m => m.Emoji)
            .IsRequired(false)
            .HasMaxLength(10);

        builder.Property(m => m.Date)
            .IsRequired()
            .HasColumnType("date"); // PostgreSQL: date only

        builder.Property(m => m.Completed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.WeddingId)
            .IsRequired();

        builder.HasIndex(m => m.WeddingId)
            .HasDatabaseName("IX_Milestones_WeddingId");
    }
}