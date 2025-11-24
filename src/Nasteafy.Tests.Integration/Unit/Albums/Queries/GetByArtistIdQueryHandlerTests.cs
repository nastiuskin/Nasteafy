using AutoFixture;
using AutoFixture.AutoMoq;
using FluentResults;
using Moq;
using Nasteafy.Application.Albums.Queries.GetByArtistId;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain.Entities;
using Nasteafy.Domain;
using Nasteafy.Application.Common.Abstractions.Auth;

namespace Nasteafy.Application.Tests.Albums.Queries
{
    public class GetByArtistIdQueryHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly GetByArtistIdQueryHandler _handler;
        private readonly ICurrentUserProvider _currentUserProvider;

        public GetByArtistIdQueryHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors
               .OfType<ThrowingRecursionBehavior>()
               .ToList()
               .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _currentUserProvider = Mock.Of<ICurrentUserProvider>(provider => provider.GetUserId() == Guid.NewGuid());

            _handler = new GetByArtistIdQueryHandler(_unitOfWorkMock.Object, _fileStorageServiceMock.Object, _currentUserProvider);
        }

        [Fact]
        public async Task ReturnsPagedResult_WithCoverUrlAndArtistNames()
        {
            // Arrange
            var artist = _fixture.Build<Artist>()
                .With(a => a.Name, "Artist 1")
                .With(a => a.AlbumArtists, [])
                .With(a => a.ArtistTracks, [])
                .Create();

            var albumId = Guid.NewGuid();

            var album = _fixture.Build<Album>()
                .With(a => a.Id, albumId)
                .With(a => a.AlbumArtists, new List<AlbumArtist>
                {
                new AlbumArtist
                {
                    Artist = artist,
                    ArtistId = artist.Id,
                    AlbumId = albumId
                }
                })
                .Create();

            var pagedAlbums = new PagedResult<Album>
            {
                Items = [album],
                TotalItems = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _unitOfWorkMock.Setup(u =>
                u.Albums.GetAlbumsByArtistIdAsync(It.IsAny<Guid>(), It.IsAny<PagedRequest>(), default))
                .ReturnsAsync(pagedAlbums);

            _fileStorageServiceMock.Setup(f =>
                f.GetFileUrlAsync(FileType.AlbumCover, album.CoverUrl))
                .ReturnsAsync(Result.Ok("urlcik"));

            var query = new GetAlbumsByArtistIdQuery(Guid.NewGuid(), new PagedRequest { PageNumber = 1, PageSize = 10 });

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value.Items);

            var dto = result.Value.Items.First();

            Assert.Equal(album.Id, dto.Id);
            Assert.Equal(album.Title, dto.Title);
            Assert.Equal("urlcik", dto.CoverUrl);
        }
    }
}
