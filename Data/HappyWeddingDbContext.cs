using HappyWedding.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Data;

public class HappyWeddingDbContext(DbContextOptions<HappyWeddingDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wedding> Weddings => Set<Wedding>();
    public DbSet<Milestone> Milestones => Set<Milestone>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
    public DbSet<ExpenseItem> ExpenseItems => Set<ExpenseItem>();
    public DbSet<WeddingPhoto> WeddingPhotos => Set<WeddingPhoto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(HappyWeddingDbContext).Assembly
        );
    }
}