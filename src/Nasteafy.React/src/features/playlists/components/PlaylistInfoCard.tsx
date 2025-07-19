import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Music, Pencil, Trash } from "lucide-react";
import { Card, CardContent } from "../../../components/ui/card";
import { client } from "../../../api/ApiClientProvider";
import { handleApiError } from "../../../helpers/handleApiError";
import { PagedRequest, type UserPlaylistDto } from "../../../api/apiClient";
import TrackList from "../../tracks/TrackList";
import { Button } from "../../../components/ui/button";
import { useAuth } from "../../../hooks/useAuth";
import toast from "react-hot-toast";
import ConfirmDialog from "../../../components/ConfirmDialog";
import type { PlaylistFormData } from "./CreateUpdatePlaylistModal";
import PlaylistModal from "./CreateUpdatePlaylistModal";

export default function PlaylistInfoCard() {
  const { id } = useParams();
  const [playlist, setPlaylist] = useState<UserPlaylistDto | null>(null);
  const [refreshKey, setRefreshKey] = useState(0);
  const { isUser } = useAuth();
  const [trackToRemove, setTrackToRemove] = useState<string | null>(null);
  const [editOpen, setEditOpen] = useState(false);

  useEffect(() => {
    if (!id) return;

    const loadPlaylist = async () => {
      try {
        const data = await client.playlistsGET(id);
        setPlaylist(data);
      } catch (err) {
        handleApiError(err);
      }
    };

    loadPlaylist();
  }, [id]);

  const fetchTracksForPlaylist = async (page: number, pageSize: number) => {
    const pagedRequest = new PagedRequest();
    pagedRequest.init({
      pageNumber: page,
      pageSize: pageSize,
    });
    try {
      if (!playlist?.id) return { items: [], totalPages: 1 };
      const result = await client.paginatedSearch(playlist.id, pagedRequest);
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
    try {
      await client.tracksDELETE2(playlist.id, trackToRemove);
      toast.success("Track removed from playlist");
      setRefreshKey((k) => k + 1);
    } catch (err) {
      handleApiError(err);
    } finally {
      setTrackToRemove(null);
    }
  };

  const handleUpdatePlaylist = async (data: PlaylistFormData) => {
    if (!playlist?.id) return;

    try {
      await client.playlistsPUT(
        playlist.id,
        data.title,
        data.coverFile
          ? { data: data.coverFile, fileName: data.coverFile.name }
          : null
      );

      const updated = await client.playlistsGET(playlist.id);
      setPlaylist(updated);
      setEditOpen(false);
    } catch (err) {
      handleApiError(err);
      toast.error("Failed to update playlist");
    }
  };

  if (!playlist) return <p className="p-4 text-muted-foreground">Loading...</p>;

  return (
    <>
      <Card className="bg-card text-card-foreground border border-border shadow-sm">
        <CardContent className="p-6 flex flex-col gap-6">
          <div className="w-full rounded-xl overflow-hidden bg-gradient-to-br from-zinc-100 via-zinc-200 to-zinc-100 p-6 sm:p-8 flex items-center gap-6 shadow-sm border border-border">
            <div className="relative w-36 h-36 sm:w-40 sm:h-40 bg-muted/30 backdrop-blur-sm rounded-lg overflow-hidden shadow-sm border border-border">
              {playlist.coverUrl ? (
                <img
                  src={playlist.coverUrl}
                  alt={playlist.title}
                  className="w-full h-full object-cover"
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center">
                  <Music className="w-10 h-10 text-muted-foreground" />
                </div>
              )}
              <div className="absolute inset-0 bg-black/40 flex items-center justify-center opacity-0 hover:opacity-100 transition-opacity">
                <Button
                  type="button"
                  variant="ghost"
                  className="text-white bg-transparent hover:bg-transparent"
                  onClick={() => setEditOpen(true)}
                >
                  <Pencil className="w-5 h-5" />
                </Button>
              </div>
            </div>

            <div className="flex flex-col justify-center gap-1 text-foreground">
              <h2 className="text-2xl sm:text-3xl font-bold text-black">
                {playlist.title}
              </h2>
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

      <PlaylistModal
        open={editOpen}
        setOpen={setEditOpen}
        initialData={{
          title: playlist.title,
          coverUrl: playlist.coverUrl,
        }}
        onSubmit={handleUpdatePlaylist}
      />
    </>
  );
}
