namespace Nasteafy.Domain.Entities.Subscriptions
{
    public class SubscriptionType(string name)
    {
        public string Name { get; set; } = name;

        public static SubscriptionType Free = new(nameof(Free)); // cannot create playlists, only listening tracks, each 5 track is followed by ad
        public static SubscriptionType Trial = new(nameof(Trial)); // 15 days of free Premium, available only once
        public static SubscriptionType Premiun = new(nameof(Premiun));  // without adds, can create playlists, maybe tracks downloading
        public static SubscriptionType Artist = new(nameof(Artist)); // can upload tracks
    }
}
