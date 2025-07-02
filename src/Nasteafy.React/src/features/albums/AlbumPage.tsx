import { useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import { client } from "../../api/ApiClientProvider";
import type { AlbumDto, GetTrackDto } from "../../api/apiClient";
import { handleApiError } from "../../helpers/handleApiError";
import { Button } from "../../components/ui/button";
import { useAuth } from "../../hooks/useAuth";
import { Music, Pencil } from "lucide-react";
import type { AlbumFormData } from "./components/CreateUpdateAlbumModal";
import CreateUpdateAlbumModal from "./components/CreateUpdateAlbumModal";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import UploadTrackModal from "../tracks/UploadTrackModal";

export default function AlbumPage() {
  const { id } = useParams<{ id: string }>();
  const [album, setAlbum] = useState<AlbumDto | null>(null);
  const [tracks, setTracks] = useState<GetTrackDto[]>([]);
  const [open, setOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [editOpen, setEditOpen] = useState(false);
  const [uploadOpen, setUploadOpen] = useState(false);
  const { isAdmin } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!id) return;

    const fetchAlbum = async () => {
      try {
        const data = await client.albumsGET2(id);
        setAlbum(data);
      } catch (error) {
        handleApiError(error);
      }
    };

    const fetchTracks = async (page: number, pageSize: number) => {
      try {
        const response = await client.tracksGET(id!, page, pageSize);
        setTracks(response.items ?? []);
      } catch (error) {
        handleApiError(error);
      }
    };

    fetchAlbum();
    fetchTracks(1, 100);
  }, [id]);

  const handleUpdateAlbum = async (data: AlbumFormData) => {
    if (!album) return;

    try {
      await client.albumsPUT(
        album.id!,
        data.coverFile ? { data: data.coverFile, fileName: data.coverFile.name } : null,
        data.releaseDate ? new Date(data.releaseDate) : undefined,
        data.title
      );

      const updated = await client.albumsGET2(album.id!);
      setAlbum(updated);
      toast.success("Album updated");
      setEditOpen(false);
    } catch (error) {
      handleApiError(error);
    }
  };

  const handleDeleteAlbum = async () => {
    try {
      await client.albumsDELETE(album?.id!);
      toast("Album deleted");
      setAlbum(null);
      window.history.back();
    } catch (error) {
      handleApiError(error);
    } finally {
      setConfirmOpen(false);
    }
  };

  const handleCancelDeleteAlbum = () => {
    setConfirmOpen(false);
  };


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

      <div className="space-y-2">
        {tracks.length === 0 ? (
          <p className="text-muted-foreground flex items-center gap-2">
            <Music className="w-4 h-4" /> No tracks found.
          </p>
        ) : (
          tracks.map((track) => (
            <div
              key={track.id}
              className="border border-border rounded p-3 text-foreground bg-card shadow-sm"
            >
              {track.title}
            </div>
          ))
        )}
      </div>

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
          onCancel={handleCancelDeleteAlbum}
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
            setOpen(false);
            client.tracksGET(album.id!, 1, 100).then((res) => {
              setTracks(res.items ?? []);
            });
          }}
        />
      )}

    </div>
  );
}
