using HappyWedding.Api.Extensions;
using Scalar.AspNetCore;

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

if (app.Environment.IsDevelopment())
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