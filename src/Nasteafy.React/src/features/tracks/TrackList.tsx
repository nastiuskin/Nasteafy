import { useCallback, useState } from "react";
import PaginatedList from "../../components/Pagination";
import { type GetTrackDto } from "../../api/apiClient";
import { useAudioPlayer } from "../../contexts/AudioPlayerContext";
import { Music, Pause, Play, Heart, HeartOff } from "lucide-react";
import { Button } from "../../components/ui/button";
import { useAuth } from "../../hooks/useAuth";
import { client } from "../../api/ApiClientProvider";
import toast from "react-hot-toast";
import { Tooltip, TooltipContent, TooltipTrigger } from "../../components/ui/tooltip";

type TrackListProps = {
  fetchTracks: (page: number, pageSize: number) => Promise<{
    items: GetTrackDto[];
    totalPages: number;
  }>;
  title?: string;
  renderActions?: (track: GetTrackDto) => React.ReactNode;
  query?: string;
  noResults?: boolean;
};

export default function TrackList({ fetchTracks, renderActions, query, noResults }: TrackListProps) {
  const { playTrack, currentUrl, isPlaying } = useAudioPlayer();
  const { isAuthenticated } = useAuth();

  const [likedTracks, setLikedTracks] = useState<Record<string, boolean>>({});

  const handleToggleLike = async (trackId: string, liked: boolean) => {
    try {
      await client.like(trackId, liked);
      setLikedTracks((prev) => ({ ...prev, [trackId]: liked }));
      if (liked)
        toast.success("Track successfully added to liked songs")
    } catch (err) {
      console.error("Failed to toggle like", err);
    }
  };

  const handlePageLoad = useCallback(async (page: number, size: number) => {
    const data = await fetchTracks(page, size);

    const newLikes: Record<string, boolean> = {};
    data.items.forEach((t) => {
      newLikes[t.id!] = t.isLiked ?? false;
    });
    setLikedTracks((prev) => ({ ...prev, ...newLikes }));

    return data;
  }, [fetchTracks]);


  const renderLikeButton = (trackId: string, isLiked: boolean) => (
    <Button
      variant="ghost"
      className="p-2"
      onClick={() => handleToggleLike(trackId, !isLiked)}
    >
      {isLiked ? (
        <Heart className="w-5 h-5 text-red-500" fill="red" />
      ) : (
        <HeartOff className="w-5 h-5 text-muted-foreground" />
      )}
    </Button>
  );

  const renderPlayButton = (track: GetTrackDto, isCurrent: boolean, index: number) => {
  const { user, isAuthenticated, isGuest } = useAuth();

  const canPlayAny = isAuthenticated && user?.subscriptionType !== "Free";
  const isFirst = index === 0;

  const handlePlayClick = () => {
    if (!canPlayAny && !isFirst) return; 
    playTrack(track.pathUrl!, track.title!, track.artistName);
  };

  const tooltipMessage =
    !canPlayAny && !isFirst
      ? "Only first track can be played in Free mode"
      : "";

  const playBtn = (
    <Button
      className="p-2 rounded hover:bg-accent"
      variant="ghost"
      onClick={handlePlayClick}
    >
      {isCurrent && isPlaying ? (
        <Pause className="w-6 h-6 text-primary" />
      ) : (
        <Play className="w-6 h-6 text-foreground" />
      )}
    </Button>
  );

return !canPlayAny && !isFirst ? (
  <Tooltip>
    <TooltipTrigger asChild>
      <Button
        variant="ghost"
        className="p-2"
        onClick={(e) => e.preventDefault()}
      >
        <Play className="w-6 h-6 text-muted-foreground" />
      </Button>
    </TooltipTrigger>
    <TooltipContent side="top">
      Only the first track can be played in Free mode
    </TooltipContent>
  </Tooltip>
) : (
  playBtn
);
};

  return (
    <div className="mt-4">
      <PaginatedList
        fetchPage={handlePageLoad}
        className="space-y-2"
        emptyContent={
          noResults && query ? (
            <p className="text-muted-foreground text-sm text-center">
              No tracks found for "{query}"
            </p>
          ) : (
            <p className="text-muted-foreground text-sm text-center">No tracks yet</p>
          )
        }
        renderItem={(track: GetTrackDto, index: number) => {
          const isCurrent = currentUrl === track.pathUrl;
          const isLiked = likedTracks[track.id!] ?? false;

          return (
            <div
              key={track.id}
              className={`flex items-center justify-between px-4 py-3 rounded-lg transition group border border-border bg-card ${isCurrent ? "bg-muted" : "hover:bg-muted"
                }`}
            >
              <div className="flex items-center gap-4">
                <span className="w-6 text-sm text-muted-foreground">{index + 1}</span>
                {track.albumCover ? (
                  <img
                    src={track.albumCover}
                    alt="Cover"
                    className="w-10 h-10 object-cover rounded"
                  />
                ) : (
                  <div className="w-10 h-10 bg-muted rounded flex items-center justify-center">
                    <Music className="w-4 h-4 text-muted-foreground" />
                  </div>
                )}

                <div className="flex flex-col">
                  <span className="text-sm font-medium text-foreground">{track.title}</span>
                  <span className="text-xs text-muted-foreground">{track.artistName}</span>
                </div>
              </div>

              <div className="flex items-center gap-2">
                <span className="text-xs text-muted-foreground">
                  {track.duration ? formatDuration(track.duration) : "00:00"}
                </span>

                {renderPlayButton(track, isCurrent, index)}

                {isAuthenticated && renderLikeButton(track.id!, isLiked)}

                {renderActions?.(track)}
              </div>
            </div>
          );
        }}
        pageSize={10}
      />
    </div>
  );
}

function formatDuration(duration: string): string {
  const parts = duration.split(":");
  const minutes = parseInt(parts[1]);
  const seconds = Math.floor(parseFloat(parts[2]));
  return `${minutes}:${seconds.toString().padStart(2, "0")}`;
}
