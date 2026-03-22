using HappyWedding.Api.Services;

namespace HappyWedding.Api.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IWeddingService, WeddingService>();
        services.AddScoped<IGuestService, GuestService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IMilestoneService, MilestoneService>();

        return services;
    }
}