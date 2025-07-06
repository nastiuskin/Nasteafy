import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Music, Trash } from "lucide-react";
import { Card, CardContent } from "../../../components/ui/card";
import { client } from "../../../api/ApiClientProvider";
import { handleApiError } from "../../../helpers/handleApiError";
import { RemoveTrackFromPlaylistCommand, type UserPlaylistDto } from "../../../api/apiClient";
import TrackList from "../../tracks/TrackList";
import { Button } from "../../../components/ui/button";
import { useAuth } from "../../../hooks/useAuth";
import toast from "react-hot-toast";
import ConfirmDialog from "../../../components/ConfirmDialog";

export default function PlaylistInfoCard() {
  const { id } = useParams();
  const [playlist, setPlaylist] = useState<UserPlaylistDto | null>(null);
  const [refreshKey, setRefreshKey] = useState(0);
  const { isUser } = useAuth();
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [trackToRemove, setTrackToRemove] = useState<string | null>(null);

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

  const handleRemoveTracksFromPlaylist = async () => {
    if (!trackToRemove || !playlist?.id) return;
    const command = new RemoveTrackFromPlaylistCommand({ trackId: trackToRemove });

    try {
      await client.tracksDELETE2(playlist.id, command);
      toast.success("Track removed from playlist");
      setRefreshKey((k) => k + 1);
    } catch (err) {
      handleApiError(err);
    } finally {
      setTrackToRemove(null);
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

        <TrackList
          key={refreshKey}
          fetchTracks={fetchTracksForPlaylist}
          renderActions={(track) =>
            isUser && (
              <Button
                variant="ghost"
                size="icon"
                title="Delete track"
                onClick={() => setTrackToRemove(track.id!)}
              >
                <Trash className="w-4 h-4 text-destructive" />
              </Button>
            )
          }
        />
        {trackToRemove && (
          <ConfirmDialog
            message="Are you sure you want to remove this track from the playlist?"
            onConfirm={handleRemoveTracksFromPlaylist}
            onCancel={() => setTrackToRemove(null)}
            confirmText="Yes, remove"
            cancelText="No"
          />
        )}
      </CardContent>
    </Card>
  );
}
