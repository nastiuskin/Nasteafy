using Nasteafy.Extensions;
using Nasteafy.Persistence.Database.Extensions;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console();
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.AddServices();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();
//builder.Services.AddAntiforgery();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

await app.SeedData();

if (app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();
}

//var logger = app.Services.GetRequiredService<IResultLogger>();
//Result.Setup(settings =>
//{
//    settings.Logger = logger;
//});

app.UseRouting();                  
app.UseCors();                      
app.UseGlobalExceptionHandling();  
app.UseRequestTimingMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseDbTransaction();
}

app.UseFluentResultsLogger();

await app.RunAsync();

public partial class Program { }

