namespace Nasteafy.Application.Subscriptions.Queries.GetAll
{
    public class GetAllSubscriptionsResponse
    {
        public List<GetSubscriptionDto> Subscriptions { get; set; } = [];
    }

    public record GetSubscriptionDto(Guid Id, string Name, string Description, decimal Price);
}
