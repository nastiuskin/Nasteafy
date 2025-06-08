namespace Nasteafy.Domain.Entities.Subscriptions
{
    public class SubscriptionType(string name)
    {
        public string Name { get; set; } = name;

        public static SubscriptionType Free = new(nameof(Free)); // cannot create playlists, only listening tracks, each 5 track is followed by ad
        public static SubscriptionType Trial = new(nameof(Trial)); // 15 days of free Premium, available only once
        public static SubscriptionType Premium = new(nameof(Premium));  // without adds, can create playlists, maybe tracks downloading
        public static SubscriptionType Artist = new(nameof(Artist)); // can upload tracks

        public override bool Equals(object? obj)
        {
            if (obj is not SubscriptionType other)
                return false;

            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }
        public override int GetHashCode() => Name.ToLowerInvariant().GetHashCode();

        public static bool operator !=(SubscriptionType? a, SubscriptionType? b) => !(a == b);

        public static bool operator ==(SubscriptionType? a, SubscriptionType? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }
    }
}
