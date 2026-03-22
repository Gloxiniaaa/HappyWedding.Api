using HappyWedding.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HappyWeddingDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")!));

        return services;
    }
}