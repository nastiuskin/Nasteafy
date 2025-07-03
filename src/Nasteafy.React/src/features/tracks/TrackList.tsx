import PaginatedList from "../../components/Pagination";
import type { GetTrackDto } from "../../api/apiClient";
import { useAudioPlayer } from "../../contexts/AudioPlayerContext";
import { Music, Pause, Play } from "lucide-react";
import { Button } from "../../components/ui/button";

type TrackListProps = {
  fetchTracks: (page: number, pageSize: number) => Promise<{
    items: GetTrackDto[];
    totalPages: number;
  }>;
  title?: string;
  renderActions?: (track: GetTrackDto) => React.ReactNode;
};

export default function TrackList({ fetchTracks, renderActions }: TrackListProps) {
  const { playTrack, currentUrl, isPlaying } = useAudioPlayer();

  return (
    <div className="mt-4">
      <PaginatedList
        fetchPage={fetchTracks}
        className="space-y-2"
        emptyContent={<p className="text-muted-foreground text-sm">No tracks yet</p>}
        renderItem={(track: GetTrackDto, index: number) => {
          const isCurrent = currentUrl === track.pathUrl;
          
          return (
            <div className={`flex items-center justify-between px-4 py-3 rounded-lg transition group border border-border bg-card ${isCurrent ? "bg-muted" : "hover:bg-muted"}`}
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
                <Button
                  className="p-2 rounded hover:bg-accent"
                  variant="ghost"
                  onClick={() =>
                    playTrack(track.pathUrl!, track.title!, track.artistName)
                  }
                >
                  {isCurrent && isPlaying ? (
                    <Pause className="w-6 h-6 text-primary" />
                  ) : (
                    <Play className="w-6 h-6 text-foreground" />
                  )}
                </Button>
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
