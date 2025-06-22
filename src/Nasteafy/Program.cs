using Nasteafy.Extensions;
using Nasteafy.Persistence.Database.Extensions;
using System.Reflection;
using Serilog;
using Google.Cloud.Firestore.V1;
using FluentResults;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console(new Serilog.Formatting.Compact.CompactJsonFormatter());
    //loggerConfiguration.WriteTo.Console();
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

// Add application also
// Add persistence layer and here   
// Add fluent validation https://docs.fluentvalidation.net/en/latest/
// Add auth
// Add global exception handler
// Add transactions
// Check/Add Migrations
// Divide entities by schemas


builder.AddServices();
builder.Services.AddAntiforgery();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();
//builder.Services.AddAntiforgery();

var logger = builder.Services.BuildServiceProvider().GetRequiredService<IResultLogger>();

Result.Setup(settings =>
{
    settings.Logger = logger;
});

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

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();
}

app.UseCors();

app.UseAuthentication();
app.UseAntiforgery();
app.UseAuthorization();
//app.UseAntiforgery();
app.UseDbTransaction();
app.UseGlobalExceptionHandling();
app.UseRequestTimingMiddleware();

await app.RunAsync();

