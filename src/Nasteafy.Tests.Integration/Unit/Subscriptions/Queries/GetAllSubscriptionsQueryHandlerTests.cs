using AutoFixture;
using Moq;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Subscriptions.Queries.GetAll;
using Nasteafy.Domain.Entities.Subscriptions;
using AutoFixture.AutoMoq;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Helpers;

namespace Nasteafy.Application.Tests.Subscriptions.Queries
{
    public class GetAllSubscriptionsQueryHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISubscriptionRepository> _subscriptionRepoMock;
        private readonly Mock<ICurrentUserProvider> _currentUserProvider;
        private readonly GetAllSubscriptionsQueryHandler _handler;
        private readonly IDateTimeService _dateTimeService;

        public GetAllSubscriptionsQueryHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _subscriptionRepoMock = new Mock<ISubscriptionRepository>();
            _currentUserProvider = new Mock<ICurrentUserProvider>();
            _dateTimeService = _fixture.Create<IDateTimeService>();

            _unitOfWorkMock.SetupGet(u => u.Subscriptions).Returns(_subscriptionRepoMock.Object);

            _handler = new GetAllSubscriptionsQueryHandler(_unitOfWorkMock.Object, _currentUserProvider.Object , _dateTimeService);
        }

        [Fact]
        public async Task ReturnsAllSubscriptions_WhenTheyExist()
        {
            // Arrange
            var subscriptions = _fixture
                .CreateMany<Subscription>(3)
                .ToList();

            var queryable = subscriptions
                .AsQueryable()
                .BuildMock();

            _subscriptionRepoMock.Setup(r => r.GetAll())
                .Returns(queryable);

            var query = new GetAllSubscriptionsQuery();

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value.Subscriptions.Count);
            Assert.All(result.Value.Subscriptions, dto =>
            {
                Assert.False(string.IsNullOrEmpty(dto.Name));
            });
        }
    }
}
