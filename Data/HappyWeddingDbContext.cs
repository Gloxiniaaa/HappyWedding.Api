using HappyWedding.Api.Data.Configurations;
using HappyWedding.Api.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Data;

public class HappyWeddingDbContext(DbContextOptions<HappyWeddingDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wedding> Weddings => Set<Wedding>();
    public DbSet<Milestone> Milestones => Set<Milestone>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(HappyWeddingDbContext).Assembly
        );
    }
}