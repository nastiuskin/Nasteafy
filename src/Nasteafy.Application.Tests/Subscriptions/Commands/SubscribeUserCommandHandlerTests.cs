using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Subscriptions.Commands;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Tests.Subscriptions.Commands
{
    public class SubscribeUserCommandHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICurrentUserProvider> _userIdProviderMock;
        private readonly SubscribeUserCommandHandler _handler;
        public SubscribeUserCommandHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();
            _userIdProviderMock = _fixture.Freeze<Mock<ICurrentUserProvider>>();
            _handler = new SubscribeUserCommandHandler(_unitOfWorkMock.Object, _userIdProviderMock.Object);
        }

        [Fact]
        public async Task ReturnsFail_WhenUserIdIsNull()
        {
            _userIdProviderMock.Setup(p => p.GetUserId()).Returns((Guid?)null);

            var command = new SubscribeUserCommand(Guid.NewGuid());

            var result = await _handler.Handle(command, default);

            Assert.True(result.IsFailed);
            Assert.Contains(result.Errors, e => e.Message.Contains("UserId not found"));
        }

        [Fact]
        public async Task ReturnsFail_WhenTrialAlreadyActivated()
        {
            var userId = Guid.NewGuid();
            var subscription = _fixture
                .Build<Subscription>()
                .With(s => s.Type, SubscriptionType.Trial)
                .Create();

            var existingTrial = _fixture
                .Build<UserSubscription>()
                .With(us => us.Subscription, subscription)
                .Create();

            var user = _fixture.Build<User>()
                .With(u => u.Id, userId)
                .With(u => u.UserSubscriptions, new List<UserSubscription> { existingTrial })
                .Create();

            _userIdProviderMock.Setup(p => p.GetUserId())
                .Returns(userId);

            _unitOfWorkMock.Setup(u => u.Users
                .GetByIdWithSubscriptionsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _unitOfWorkMock.Setup(u => u.Subscriptions
                .GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(subscription);

            var command = new SubscribeUserCommand(subscription.Id);

            var result = await _handler.Handle(command, default);

            Assert.True(result.IsFailed);
            Assert.Contains(result.Errors, e => e.Message.Contains("Trial subscription can be activated only once."));
        }

        [Fact]
        public async Task CreatesArtist_WhenArtistSubscription_AndUserNotArtist()
        {
            var userId = Guid.NewGuid();
            var subscription = _fixture
                .Build<Subscription>()
                .With(s => s.Type, SubscriptionType.Artist)
                .Create();

            var user = _fixture.Build<User>()
                .With(u => u.Id, userId)
                .With(u => u.UserSubscriptions, [])
                .Create();

            _userIdProviderMock.Setup(p => p.GetUserId())
                .Returns(userId);

            _unitOfWorkMock.Setup(u => u.Users.GetByIdWithSubscriptionsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _unitOfWorkMock.Setup(u => u.Subscriptions.GetByIdAsync(subscription.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subscription);

            _unitOfWorkMock.Setup(u => u.Artists.ExistsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(u => u.Artists.AddAsync(It.IsAny<Artist>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new SubscribeUserCommand(subscription.Id);

            var result = await _handler.Handle(command, default);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task AddsNewSubscription_AndEndsPreviousOnes()
        {
            var userId = Guid.NewGuid();
            var subscription = _fixture.Build<Subscription>()
                .With(s => s.Type, SubscriptionType.Premium)
                .With(s => s.DurationInDays, 30)
                .Create();

            var oldSubscription = _fixture.Build<UserSubscription>()
                .Create();

            var user = _fixture.Build<User>()
                .With(u => u.Id, userId)
                .With(u => u.UserSubscriptions, new List<UserSubscription> { oldSubscription })
                .Create();

            _userIdProviderMock.Setup(p => p.GetUserId())
                .Returns(userId);

            _unitOfWorkMock.Setup(u => u.Users.GetByIdWithSubscriptionsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _unitOfWorkMock.Setup(u => u.Subscriptions.GetByIdAsync(subscription.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subscription);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new SubscribeUserCommand(subscription.Id);

            var result = await _handler.Handle(command, default);

            var newSubscription = user.UserSubscriptions.FirstOrDefault(s => s.SubscriptionId == subscription.Id);  

            Assert.True(result.IsSuccess);
            Assert.All(user.UserSubscriptions, s => Assert.True(s.EndDate <= DateTime.UtcNow.AddDays(30)));
            Assert.NotNull(newSubscription);
            Assert.Equal(subscription.DurationInDays, (newSubscription.EndDate - newSubscription.StartDate).Days);
        }
    }
}
