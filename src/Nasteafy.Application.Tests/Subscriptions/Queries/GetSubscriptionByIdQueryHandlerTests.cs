using AutoFixture;
using Moq;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Application.Subscriptions.Queries.GetById;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Application.Tests.Subscriptions.Queries
{
    public class GetSubscriptionByIdQueryHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISubscriptionRepository> _subscriptionRepoMock;
        private readonly GetSubscriptionByIdQueryHandler _handler;

        public GetSubscriptionByIdQueryHandlerTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors
               .OfType<ThrowingRecursionBehavior>()
               .ToList()
               .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _subscriptionRepoMock = new Mock<ISubscriptionRepository>();

            _unitOfWorkMock.SetupGet(u => u.Subscriptions).Returns(_subscriptionRepoMock.Object);

            _handler = new GetSubscriptionByIdQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task ReturnsSubscription_WhenFound()
        {
            // Arrange
            var subscription = _fixture.Build<Subscription>()
                .With(s => s.Type, new SubscriptionType("Premium"))
                .Create();

            _subscriptionRepoMock
                .Setup(r => r.GetByIdAsync(subscription.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subscription);

            var query = new GetSubscriptionByIdQuery(subscription.Id);

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(subscription.Id, result.Value!.Id);
            Assert.Equal("Premium", result.Value.Name);
        }
    }
}