using HappyWedding.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappyWedding.Api.Data.Configurations;

public class ExpenseItemConfiguration : IEntityTypeConfiguration<ExpenseItem>
{
    public void Configure(EntityTypeBuilder<ExpenseItem> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.EstimateCost)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.ActualCost)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.Paid)
            .IsRequired()
            .HasDefaultValue(false);
    }
}