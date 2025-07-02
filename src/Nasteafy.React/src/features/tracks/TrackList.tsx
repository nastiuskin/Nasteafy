import PaginatedList from "../../components/Pagination";
import type { GetTrackDto } from "../../api/apiClient";
import { client } from "../../api/ApiClientProvider";
import { useAudioPlayer } from "../../contexts/AudioPlayerContext";
import { Music, Pause, Play } from "lucide-react";

export default function TrackList({ playlistId }: { playlistId: string }) {
  const { playTrack, currentUrl, isPlaying } = useAudioPlayer();

  const fetchTracks = async (page: number, pageSize: number) => {
    const result = await client.tracksGET4(playlistId, page, pageSize);
    return {
      items: result.items ?? [],
      totalPages: result.totalPages ?? 1,
    };
  };

  return (
    <div className="mt-4">
      <h3 className="text-base font-semibold mb-2">Tracks:</h3>
      <PaginatedList
        fetchPage={fetchTracks}
        renderItem={(track: GetTrackDto, index: number) => {
          const isCurrent = currentUrl === track.pathUrl;

          return (
            <div className="flex items-center justify-between px-4 py-3 bg-neutral-800 hover:bg-neutral-700 rounded-lg transition group">
              <div className="flex items-center gap-4">
                <span className="w-6 text-sm text-neutral-400">{index + 1}</span>

                {track.albumCover ? (
                  <img
                    src={track.albumCover}
                    alt="Cover"
                    className="w-10 h-10 object-cover rounded"
                  />
                ) : (
                  <div className="w-10 h-10 bg-neutral-700 rounded flex items-center justify-center">
                    <Music className="w-4 h-4 text-neutral-400" />
                  </div>
                )}

                <div className="flex flex-col">
                  <span className="text-sm font-medium text-white">{track.title}</span>
                  <span className="text-xs text-neutral-400">{track.artistName}</span>
                </div>
              </div>

              <div className="flex items-center gap-4">
                <span className="text-xs text-neutral-400">
                  {track.duration ? formatDuration(track.duration) : "00:00"}
                </span>
                <button
                  className="p-2 rounded hover:bg-neutral-600"
                  onClick={() =>
                    playTrack(track.pathUrl!, track.title!, track.artistName)
                  }
                >
                  {isCurrent && isPlaying ? (
                    <Pause className="w-6 h-6 text-green-400" />
                  ) : (
                    <Play className="w-6 h-6 text-white" />
                  )}
                </button>
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
