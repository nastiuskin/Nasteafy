using Microsoft.AspNetCore.Builder;
using Nasteafy.Application.Extensions;
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

builder.Services
    .AddPersistence(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGenWithAuth();

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();
}

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();

