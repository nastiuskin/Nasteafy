using FluentAssertions;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Common.Models;
using Nasteafy.Tests.Integration.Fixtures;
using System.Net;
using System.Text.Json;

namespace Nasteafy.Tests.Integration.Endpoints.Artists
{
    public class GetAllArtistsEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GetAllArtistsEndpointTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_Return_ListOfArtists()
        {
            // Act
            var response = await _client.GetAsync("/api/artists?pageNumber=1&pageSize=10");

            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"body: {content}");

            var result = JsonSerializer.Deserialize<PagedResult<ArtistDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.Should().NotBeNull("Response body was null or not deserializable");
            result.Items.Should().NotBeNull("Items list was null");
            result.Items.Should().NotBeEmpty("Items list was empty");
        }
    }
}