using Nasteafy.Extensions;
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

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseDbTransaction();
app.UseGlobalExceptionHandling();

await app.RunAsync();

