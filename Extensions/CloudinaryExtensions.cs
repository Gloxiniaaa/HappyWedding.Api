using HappyWedding.Api.Services;
using Microsoft.AspNetCore.Http.Features;

namespace HappyWedding.Api.Extensions;

public static class CloudinaryExtensions
{
    public static IServiceCollection AddCloudinarySettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(
            configuration.GetSection("Cloudinary"));

        services.Configure<FormOptions>(o =>
        {
            o.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
        });

        return services;
    }
}