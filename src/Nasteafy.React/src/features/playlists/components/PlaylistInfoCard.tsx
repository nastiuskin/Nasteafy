import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Music } from "lucide-react";
import { Card, CardContent } from "../../../components/ui/card";
import { client } from "../../../api/ApiClientProvider";
import { handleApiError } from "../../../helpers/handleApiError";
import type { UserPlaylistDto } from "../../../api/apiClient";
import TrackList from "../../tracks/TrackList";

export default function PlaylistInfoCard() {
  const { id } = useParams();
  const [playlist, setPlaylist] = useState<UserPlaylistDto | null>(null);
  const [refreshKey, setRefreshKey] = useState(0);

  useEffect(() => {
    if (!id) return;

    const loadPlaylist = async () => {
      try {
        const data = await client.playlistsGET2(id);
        setPlaylist(data);
      } catch (err) {
        handleApiError(err);
      }
    };

    loadPlaylist();
  }, [id]);

  const fetchTracksForPlaylist = async (page: number, pageSize: number) => {
    try {
      if (!playlist?.id) return { items: [], totalPages: 1 };
      const result = await client.tracksGET4(playlist.id, page, pageSize);
      return {
        items: result.items ?? [],
        totalPages: result.totalPages ?? 1,
      };
    } catch (error) {
      handleApiError(error);
      return { items: [], totalPages: 1 };
    }
  };

  if (!playlist) return <p className="p-4 text-muted-foreground">Loading...</p>;

  return (
    <Card className="bg-card text-card-foreground border border-border shadow-sm">
      <CardContent className="p-6 flex flex-col gap-6">
        <div className="flex gap-6">
          {playlist.coverUrl ? (
            <img
              src={playlist.coverUrl}
              alt={playlist.title}
              className="w-28 h-28 object-cover rounded-lg"
            />
          ) : (
            <div className="w-28 h-28 bg-muted flex items-center justify-center rounded-lg">
              <Music className="w-10 h-10 text-muted-foreground" />
            </div>
          )}

          <div className="flex flex-col justify-center gap-1">
            <h2 className="text-xl font-bold">{playlist.title}</h2>
            <p className="text-sm text-muted-foreground">
              {playlist.tracksCount} {playlist.tracksCount === 1 ? "track" : "tracks"}
            </p>
          </div>
        </div>

        <TrackList key={refreshKey} fetchTracks={fetchTracksForPlaylist} />
      </CardContent>
    </Card>
  );
}
