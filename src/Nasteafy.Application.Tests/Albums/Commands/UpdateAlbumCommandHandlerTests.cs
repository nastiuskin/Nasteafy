using AutoFixture;
using AutoFixture.AutoMoq;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Moq;
using Nasteafy.Application.Albums.Commands.Update;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Domain;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Tests.Albums.Commands
{
    public class UpdateAlbumCommandHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAlbumRepository> _albumRepoMock;
        private readonly Mock<IFileStorageService> _fileStorageMock;
        private readonly UpdateAlbumCommandHandler _handler;

        public UpdateAlbumCommandHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _albumRepoMock = new Mock<IAlbumRepository>();
            _fileStorageMock = new Mock<IFileStorageService>();

            _unitOfWorkMock.Setup(x => x.Albums).Returns(_albumRepoMock.Object);

            _handler = new UpdateAlbumCommandHandler(_unitOfWorkMock.Object, _fileStorageMock.Object);
        }

        [Fact]
        public async Task ReturnsFail_WhenAlbumNotFound()
        {
            // Arrange
            var command = _fixture.Create<UpdateAlbumCommand>();

            _albumRepoMock
                .Setup(r => r.GetByIdAsync(command.AlbumId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Album)null!);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains(result.Errors, e => e.Message.Contains("Album not found"));
        }

        [Fact]
        public async Task UpdatesTitle_WhenProvided()
        {
            // Arrange
            var album = _fixture.Build<Album>()
                .With(a => a.Title, "Old Title")
                .Create();

            var command = _fixture.Build<UpdateAlbumCommand>()
                .With(a => a.AlbumId, album.Id)
                .With(a => a.Title, "New Title")
                .Create();

            _albumRepoMock
                .Setup(r => r.GetByIdAsync(command.AlbumId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(album);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.Albums.UpdateAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("New Title", album.Title);
        }

        [Fact]
        public async Task UpdatesCover_WhenFileProvided()
        {
            // Arrange
            var album = _fixture.Build<Album>().Create();

            var fileMock = new Mock<IFormFile>();
            var stream = new MemoryStream();
            var fileName = "cover.jpg";
            var contentType = "image/jpeg";

            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.ContentType).Returns(contentType);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            var command = new UpdateAlbumCommand(album.Id, null, fileMock.Object);

            _albumRepoMock
                .Setup(r => r.GetByIdAsync(command.AlbumId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(album);

            _fileStorageMock
                .Setup(fs => fs.UploadFileAsync(stream, fileName, contentType, FileType.AlbumCover, null))
                .ReturnsAsync(Result.Ok("new-cover-url"));

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.Albums.UpdateAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("new-cover-url", album.CoverUrl);
        }
    }
}
