using Nasteafy.Extensions;
using Nasteafy.Persistence.Database.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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

