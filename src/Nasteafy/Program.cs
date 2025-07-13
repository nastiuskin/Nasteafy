using Nasteafy.Extensions;
using Nasteafy.Persistence.Database.Extensions;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddUserSecretsConfiguration();

builder.Host.AddSerilog(builder.Configuration);

builder.AddServices();

builder.Services
    .AddCustomAuthorization()
    .AddCustomCors()
    .AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

await app.SeedData();

if (app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    app.UseSwaggerWithUi();
    app.ApplyMigrations();
}

app.UseRouting();
app.UseCors();
app.UseGlobalExceptionHandling();
app.UseRequestTimingMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

app.UseFluentResultsLogger();

await app.RunAsync();

public partial class Program { }
