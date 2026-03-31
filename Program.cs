using HappyWedding.Api.Extensions;
using HappyWedding.Api.Data;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCorsPolicy()
    .AddCloudinarySettings(builder.Configuration)
    .AddApplicationServices()
    .AddDatabase(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Apply pending migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HappyWeddingDbContext>();
    await dbContext.Database.MigrateAsync();
}

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCorsPolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();