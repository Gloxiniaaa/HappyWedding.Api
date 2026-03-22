namespace HappyWedding.Api.Extensions;

public static class CorsExtensions
{
    private const string PolicyName = "_myAllowSpecificOrigins";

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:8080",
                        "https://happy-wedding-gules.vercel.app")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        => app.UseCors(PolicyName);
}