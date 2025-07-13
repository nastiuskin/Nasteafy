import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { client } from "../../api/ApiClientProvider";
import { PagedRequest, type AlbumDto, type GetTrackDto, type UserPlaylistDto } from "../../api/apiClient";
import { handleApiError } from "../../helpers/handleApiError";
import { Button } from "../../components/ui/button";
import { useAuth } from "../../hooks/useAuth";
import { MoreVertical, Pencil, Trash } from "lucide-react";
import type { AlbumFormData } from "./components/CreateUpdateAlbumModal";
import CreateUpdateAlbumModal from "./components/CreateUpdateAlbumModal";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import UploadTrackModal from "../tracks/UploadTrackModal";
import { DropdownMenu, DropdownMenuTrigger, DropdownMenuContent, DropdownMenuItem } from "../../components/ui/dropdown-menu";
import TrackList from "../tracks/TrackList";

export default function AlbumPage() {
  const { id: albumId } = useParams<{ id: string }>();
  const [album, setAlbum] = useState<AlbumDto | null>(null);
  const [editOpen, setEditOpen] = useState(false);
  const [uploadOpen, setUploadOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [refreshKey, setRefreshKey] = useState(0);
  const [trackToDelete, setTrackToDelete] = useState<string | null>(null);
  const [playlists, setPlaylists] = useState<UserPlaylistDto[]>([]);
  const { isAdmin, isUser } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!albumId) return;

    const fetchAlbum = async () => {
      try {
        const data = await client.albumsGET(albumId);
        setAlbum(data);
      } catch (error) {
        handleApiError(error);
      }
    };

    fetchAlbum();
  }, [albumId]);


  useEffect(() => {
    fetchPlaylists();
  }, []);

  const fetchPlaylists = async (pageSize = 1, pageNumber = 10) => {
    try {
      const pagedRequest = new PagedRequest();
      pagedRequest.init({
        pageNumber: pageNumber,
        pageSize: pageSize,
        filters: [],
        sortBy: null,
        sortDirection: null
      });
      const response = await client.paginatedSearch4(pagedRequest);
      setPlaylists(response.items ?? []);
    } catch (err) {
      handleApiError(err);
    }
  };

  const handleUpdateAlbum = async (data: AlbumFormData) => {
    if (!album) return;

    try {
      await client.albumsPUT(
        album.id!,
        data.coverFile ? { data: data.coverFile, fileName: data.coverFile.name } : null,
        data.releaseDate ? new Date(data.releaseDate) : undefined,
        data.title
      );

      const updated = await client.albumsGET(album.id!);
      setAlbum(updated);
      toast.success("Album updated");
      setEditOpen(false);
    } catch (error) {
      handleApiError(error);
    }
  };

  const fetchTracksForAlbum = async (page: number, pageSize: number) => {
    if (!albumId) {
      return { items: [], totalPages: 1 };
    }
    const pagedRequest = new PagedRequest();
    pagedRequest.init({
      pageNumber: page,
      pageSize: pageSize,
      filters: [],
      sortBy: null,
      sortDirection: null
    });
    try {
      const result = await client.paginatedSearch3(albumId, pagedRequest);
      return {
        items: result.items ?? [],
        totalPages: result.totalPages ?? 1,
      };
    } catch (error) {
      handleApiError(error);
      return { items: [], totalPages: 1 };
    }
  };

  const handleDeleteAlbum = async () => {
    try {
      await client.albumsDELETE(album?.id!);
      toast("Album deleted");
      navigate(-1);
    } catch (error) {
      handleApiError(error);
    } finally {
      setConfirmOpen(false);
    }
  };

  const handleAddToPlaylist = async (trackId: string, playlistId: string) => {
    try {
      await client.tracksPOST2(playlistId, trackId);
      toast.success("Track successfully added to playlist");
    } catch (err) {
      handleApiError(err);
    }
  };

  const handleDeleteTrack = async (trackId: string) => {
    try {
      await client.tracksDELETE(trackId);
      toast("Track deleted");
    } catch (error) {
      handleApiError(error);
    } finally {
      setConfirmOpen(false);
    }
  };

  const renderActions = (track: GetTrackDto) => (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" size="icon" className="hover:bg-accent">
          <MoreVertical className="w-5 h-5 text-muted-foreground" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        {isUser && (
          <>
            <div className="px-2 py-1 text-xs text-muted-foreground">Add to playlist</div>
            {playlists.map((playlist) => (
              <DropdownMenuItem
                key={playlist.id}
                onClick={() => handleAddToPlaylist(track.id!, playlist.id!)}
              >
                {playlist.title}
              </DropdownMenuItem>
            ))}
            <DropdownMenuItem className="h-px my-1 bg-border" disabled />
          </>
        )}
        {isAdmin && (
          <DropdownMenuItem
            onClick={() => setTrackToDelete(track.id!)}
            className="text-destructive focus:text-destructive"
          >
            <Trash className="w-4 h-4 mr-2" />
            Delete track
          </DropdownMenuItem>
        )}
      </DropdownMenuContent>
    </DropdownMenu>
  );

  if (!album) return <div className="p-6">Loading album...</div>;

  return (
    <div className="p-6">
      <div className="bg-gradient-to-br from-muted/40 to-background border border-border rounded-xl p-6 flex flex-col sm:flex-row items-center sm:items-start gap-6 mb-8 shadow-md">
        <div className="relative group w-36 h-36 shrink-0">
          {album.coverUrl ? (
            <img
              src={album.coverUrl}
              alt={album.title}
              className="w-36 h-36 object-cover rounded border border-border"
            />
          ) : (
            <div className="w-36 h-36 rounded bg-muted flex items-center justify-center border border-border" />
          )}

          {isAdmin && (
            <div className="absolute inset-0 flex items-center justify-center">
              <Button
                onClick={() => setEditOpen(true)}
                variant="ghost"
                className="rounded-full bg-black/50 opacity-0 group-hover:opacity-100 transition"
                size="icon"
                title="Edit album"
              >
                <Pencil className="w-5 h-5 text-white" />
              </Button>
            </div>
          )}
        </div>

        <div className="flex flex-col items-center sm:items-start text-center sm:text-left gap-4 flex-1">
          <h1 className="text-4xl font-extrabold text-foreground">{album.title}</h1>
          <p className="text-muted-foreground text-sm">
            Released: {new Date(album.releaseDate!).toLocaleDateString()}
          </p>
          {isAdmin && (
            <div className="flex gap-2 flex-wrap">
              <Button
                onClick={() => setUploadOpen(true)}
                className="bg-primary text-primary-foreground hover:brightness-90">
                Add Track
              </Button>
              <Button
                onClick={() => setConfirmOpen(true)}
                variant="destructive">
                Delete Album
              </Button>
            </div>
          )}
        </div>
      </div>

      {albumId && (
        <TrackList
          key={refreshKey}
          fetchTracks={fetchTracksForAlbum}
          renderActions={renderActions}
        />
      )}

      {isAdmin && editOpen && (
        <CreateUpdateAlbumModal
          open={editOpen}
          setOpen={setEditOpen}
          initialData={{
            title: album.title,
            coverUrl: album.coverUrl ?? undefined,
            releaseDate: album.releaseDate
              ? new Date(album.releaseDate).toISOString().slice(0, 10)
              : "",
          }}
          onSubmit={handleUpdateAlbum}
        />
      )}

      {confirmOpen && (
        <ConfirmDialog
          message="Are you sure you want to delete this album?"
          onConfirm={handleDeleteAlbum}
          onCancel={() => setConfirmOpen(false)}
          confirmText="Yes, Delete"
          cancelText="No"
        />
      )}

      {uploadOpen && (
        <UploadTrackModal
          open={uploadOpen}
          setOpen={setUploadOpen}
          albumId={album.id!}
          onUploaded={() => {
            toast.success("Track uploaded");
            setRefreshKey((k) => k + 1);
          }}
        />
      )}

      {trackToDelete && (
        <ConfirmDialog
          message="Are you sure you want to delete this track?"
          onConfirm={async () => {
            await handleDeleteTrack(trackToDelete);
            setTrackToDelete(null);
            setRefreshKey((k) => k + 1);
          }}
          onCancel={() => setTrackToDelete(null)}
          confirmText="Yes, Delete"
          cancelText="No"
        />
      )}
    </div>
  );
}
