using HappyWedding.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HappyWedding.Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddDbContext<HappyWeddingDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.ConfigureWarnings(w =>
                 w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        return services;
    }
}