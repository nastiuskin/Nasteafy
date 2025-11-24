using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Nasteafy.Application.Albums.Commands.Create;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities;
using Nasteafy.Domain.Entities.Tracks;

public class CreateAlbumCommandHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly CreateAlbumCommandHandler _handler;

    public CreateAlbumCommandHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock =  new Mock<IUnitOfWork>();
        _fileStorageServiceMock = new Mock<IFileStorageService>();

        _handler = new CreateAlbumCommandHandler(_unitOfWorkMock.Object, _fileStorageServiceMock.Object);
    }

    [Fact]
    public async Task ReturnsSuccess_WhenAlbumCreated()
    {
        // Arrange
        var artist = _fixture.Build<Artist>()
           .With(a => a.AlbumArtists, [])
           .With(a => a.ArtistTracks, [])
           .Create();

        var command = _fixture.Build<CreateAlbumCommand>()
           .With(c => c.Artists, new List<Guid> { artist.Id })
           .Create();

        var artists = new List<Artist> { artist }.BuildMock().BuildMockDbSet().Object;

        _unitOfWorkMock
            .Setup(u => u.Artists.FindAllByIds(It.IsAny<List<Guid>>()))
            .Returns(artists);

        _unitOfWorkMock.Setup(u => u.Albums.AddAsync(It.IsAny<Album>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ReturnsFail_WhenSomeArtistsNotFound()
    {
        // Arrange
        var artistId = Guid.NewGuid();

        var artists = new List<Artist> { }.BuildMock().BuildMockDbSet().Object;

        _unitOfWorkMock
            .Setup(u => u.Artists.FindAllByIds(It.IsAny<List<Guid>>()))
            .Returns(artists);

        var command = _fixture.Build<CreateAlbumCommand>()
           .With(c => c.Artists, new List<Guid> { artistId })
            .Create();
        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("not found"));
    }
}
