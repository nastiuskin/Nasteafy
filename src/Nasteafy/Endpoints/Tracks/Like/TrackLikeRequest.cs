namespace Nasteafy.Endpoints.Tracks.Like
{
    public sealed record TrackLikeRequest(Guid TrackId, bool Liked);
}
