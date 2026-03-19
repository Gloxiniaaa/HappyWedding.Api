using HappyWedding.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWedding.Api.Data.Configurations;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        // Primary key
        builder.HasKey(g => g.Id);

        // Properties
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(g => g.Note)
            .HasMaxLength(500);           // nullable by default → no .IsRequired()

        builder.Property(g => g.SeatCount)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(g => g.Confirmed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(g => g.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");   // or use .HasDefaultValue(DateTime.UtcNow) if preferred

        builder.Property(g => g.UpdatedAt)
            .IsRequired(false);                    // nullable

        // Enum conversion (very important!)
        builder.Property(g => g.Side)
            .IsRequired()
            .HasConversion<string>();              // stores as "Groom" / "Bride" in DB (human-readable)

        // Relationships
        builder.HasOne(g => g.Wedding)
            .WithMany()                             // Wedding doesn't have List<Guest> navigation
            .HasForeignKey(g => g.WeddingId)
            .OnDelete(DeleteBehavior.Cascade)       // delete wedding → delete its guests
            .IsRequired();

        // Indexes (performance + uniqueness if desired)
        builder.HasIndex(g => new { g.WeddingId, g.Side })
            .HasDatabaseName("IX_Guest_WeddingId_Side");

        builder.HasIndex(g => g.Name)
            .HasDatabaseName("IX_Guest_Name");      // helps with name-based search

        // Optional: prevent duplicate names per wedding & side (if business rule)
        // builder.HasIndex(g => new { g.WeddingId, g.Side, g.Name })
        //     .IsUnique()
        //     .HasDatabaseName("UX_Guest_Wedding_Side_Name");
    }
}