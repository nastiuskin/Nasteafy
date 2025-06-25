using FluentAssertions;
using Nasteafy.Tests.Integration.Fixtures;
using System.Net;

namespace Nasteafy.Tests.Integration.Endpoints.Artists
{
    public class CreateArtistEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateArtistEndpointTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Create_Artist_When_ValidDataProvided()
        {
            // Arrange
            var client = _factory.CreateAuthorizedClient();

            var request = new MultipartFormDataContent
            {
                { new StringContent("Test Artist"), "Name" }
            };

            // Act
            var response = await client.PostAsync("api/artists", request);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine("BODY: " + content);
            Console.WriteLine("HEADERS: " + response.Headers);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Response: {content}");

            Guid.TryParse(content.Trim('"'), out var id).Should().BeTrue("Response should be a valid Guid");
            id.Should().NotBeEmpty();
        }
    }
}