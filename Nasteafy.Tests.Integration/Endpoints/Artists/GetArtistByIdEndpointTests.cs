using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Infrastructure.Persistence.Contexts;
using Nasteafy.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Nasteafy.Tests.Integration.Endpoints.Artists
{
    public class GetArtistByIdEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetArtistByIdEndpointTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Return_Artist_When_Id_Exists()
        {
            // Arrange
            var client = _factory.CreateClient();

            Artist? artist;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                artist = db.Artists.FirstOrDefault();

                if (artist is null)
                {
                    artist = new Artist
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test Artist"
                    };

                    db.Artists.Add(artist);
                    db.SaveChanges();
                }
            }          

            // Act
            var response = await client.GetAsync($"api/artists/{artist.Id}");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ArtistDto>();
            result.Should().NotBeNull();
            result!.Id.Should().Be(artist.Id);
            result.Name.Should().Be(artist.Name);
        }

        [Fact]
        public async Task Should_Return_400_When_Artist_Not_Found()
        {
            // Arrange
            var client = _factory.CreateClient();
            var nonExistentId = Guid.NewGuid();

            // Act
            var response = await client.GetAsync($"api/artists/{nonExistentId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}