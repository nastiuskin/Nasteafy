using AutoFixture;
using Moq;
using Nasteafy.Application.Albums.Commands.Delete;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Tests.Albums.Commands
{
    public class DeleteAlbumCommandHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAlbumRepository> _albumRepoMock;
        private readonly Mock<IFileStorageService> _fileStorageMock;
        private readonly DeletePlaylistCommandHandler _handler;

        public DeleteAlbumCommandHandlerTests()
        {
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _albumRepoMock = new Mock<IAlbumRepository>();
            _fileStorageMock = new Mock<IFileStorageService>();

            _unitOfWorkMock.Setup(u => u.Albums).Returns(_albumRepoMock.Object);

            _handler = new DeletePlaylistCommandHandler(_unitOfWorkMock.Object, _fileStorageMock.Object);
        }

        [Fact]
        public async Task ReturnsFail_WhenAlbumNotFound()
        {
            // Arrange
            _albumRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Album)null!);

            var command = new DeleteAlbumCommand(Guid.NewGuid());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains(result.Errors, e => e.Message.Contains("Album not found"));
        }

        [Fact]
        public async Task DeleteAlbumAndCover_WhenAlbumExistsWithCover()
        {
            // Arrange
            var album = _fixture.Build<Album>()
                .With(a => a.CoverUrl, "cover.jpg")
                .With(a => a.AlbumArtists, [])
                .With(a => a.Tracks, [])
                .Create();

            _albumRepoMock
                .Setup(r => r.GetByIdAsync(album.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(album);

            var command = new DeleteAlbumCommand(album.Id);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);

            _fileStorageMock.Verify(fs =>
                fs.DeleteFileAsync(FileType.AlbumCover, "cover.jpg"), Times.Once);
            _albumRepoMock.Verify(r =>
                r.DeleteAsync(album, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DeletesAlbumWithoutCover_WhenCoverUrlEmpty()
        {
            // Arrange
            var album = _fixture.Build<Album>()
                .With(a => a.CoverUrl, null as string)
                .With(a => a.AlbumArtists, [])
                .With(a => a.Tracks, [])
                .Create();

            _albumRepoMock
                .Setup(r => r.GetByIdAsync(album.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(album);

            var command = new DeleteAlbumCommand(album.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            _fileStorageMock.Verify(fs =>
                fs.DeleteFileAsync(It.IsAny<FileType>(), It.IsAny<string>()), Times.Never);
            _albumRepoMock.Verify(r =>
                r.DeleteAsync(album, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
