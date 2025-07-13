import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { client } from "../../api/ApiClientProvider";
import type { AlbumDto, ArtistDto } from "../../api/apiClient";
import { handleApiError } from "../../helpers/handleApiError";
import PaginatedList from "../../components/Pagination";
import { Button } from "../../components/ui/button";
import CreateAlbumModal, { type AlbumFormData } from "../albums/components/CreateUpdateAlbumModal";
import ConfirmDialog from "../../components/ConfirmDialog";
import toast from "react-hot-toast";
import { Pencil, User } from "lucide-react";
import ArtistModal, { type ArtistFormData } from "./components/CreateUpdateArtistModal";
import { useAuth } from "../../hooks/useAuth";

export default function ArtistProfilePage() {
  const { id } = useParams();
  const [artist, setArtist] = useState<ArtistDto | null>(null);
  const [open, setOpen] = useState(false);
  const [editOpen, setEditOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [refreshKey, setRefreshKey] = useState(0);
  const navigate = useNavigate();
  const { isAdmin } = useAuth();

  useEffect(() => {
    const fetchArtist = async () => {
      try {
        const data = await client.artistsGET(id!);
        setArtist(data);
      } catch (error) {
        handleApiError(error);
      }
    };

    fetchArtist();
  }, [id]);

  const fetchAlbums = async (page: number, pageSize: number) => {
    try {
      const response = await client.paginatedSearch2(id!, page, pageSize);
      return {
        items: response.items ?? [],
        totalPages: response.totalPages ?? 1,
      };
    } catch (error) {
      handleApiError(error);
      return { items: [], totalPages: 1 };
    }
  };

  const handleConfirmDeleteArtist = async () => {
    try {
      await client.artistsDELETE(artist?.id!);
      toast("Artist deleted");
      setArtist(null);
      navigate("/artists");
    } catch (error) {
      handleApiError(error);
    } finally {
      setConfirmOpen(false);
    }
  };

  const handleCancelDeleteArtist = () => {
    setConfirmOpen(false);
  };

  const handleUpdateArtist = async (data: ArtistFormData) => {
    try {
      await client.artistsPUT(
        artist?.id!,
        data.name,
        data.avatarFile
          ? { data: data.avatarFile, fileName: data.avatarFile.name }
          : null
      );
      setEditOpen(false);
      const updated = await client.artistsGET2(id!);
      setArtist(updated);
    } catch (err) {
      handleApiError(err);
    }
  };

  const handleSubmitAlbum = async (data: AlbumFormData & { extraArtistIds: string[] }) => {
    if (!id) return;
    try {
      await client.albumsPOST(
        data.title,
        data.coverFile
          ? { data: data.coverFile, fileName: data.coverFile.name }
          : null,
        new Date(data.releaseDate),
        [id, ...data.extraArtistIds]
      );
      toast.success("Album created");
      setRefreshKey((prev) => prev + 1);
      setOpen(false);
    } catch (err) {
      handleApiError(err);
    }
  };

  if (!artist) return <div className="p-6">Loading...</div>;

  return (
    <div className="p-6">
      <div className="bg-gradient-to-br from-muted/40 to-background border border-border rounded-xl p-6 flex flex-col sm:flex-row items-center sm:items-start gap-6 mb-8 shadow-md">
        <div className="relative group w-36 h-36 shrink-0">
          {artist.avatarUrl ? (
            <img
              src={artist.avatarUrl}
              alt={artist.name}
              className="w-36 h-36 rounded-full object-cover border border-border"
            />
          ) : (
            <div className="w-36 h-36 rounded-full bg-muted flex items-center justify-center border border-border">
              <User className="w-10 h-10 text-muted-foreground" />
            </div>
          )}

          <div className="absolute inset-0 rounded-full bg-black/50 opacity-0 group-hover:opacity-100 transition flex items-center justify-center cursor-pointer">
            <Button
              type="button"
              variant="ghost"
              className="p-2 h-auto w-auto bg-transparent hover:bg-transparent"
              onClick={() => setEditOpen(true)}
            >
              <Pencil className="w-6 h-6 text-white" />
            </Button>
          </div>
        </div>

        <div className="flex flex-col items-center sm:items-start text-center sm:text-left gap-4 flex-1">
          <h1 className="text-4xl font-extrabold text-foreground">{artist.name}</h1>

          {isAdmin && (<div className="flex gap-2 flex-wrap justify-center sm:justify-start">
            <Button
              onClick={() => setOpen(true)}
              className="bg-primary text-primary-foreground hover:brightness-90"
            >
              Add Album
            </Button>
            <Button
              onClick={() => setConfirmOpen(true)}
              className="bg-destructive text-destructive-foreground hover:brightness-90"
            >
              Delete Artist
            </Button>
          </div>)}
        </div>
      </div>

      {confirmOpen && (
        <ConfirmDialog
          message="Are you sure you want to delete this artist?"
          onConfirm={handleConfirmDeleteArtist}
          onCancel={handleCancelDeleteArtist}
          confirmText="Yes, Delete"
          cancelText="No"
        />
      )}

      <PaginatedList
        key={refreshKey}
        fetchPage={fetchAlbums}
        pageSize={8}
        emptyContent={<p className="text-muted-foreground text-sm">No albums yet</p>}
        renderItem={(album: AlbumDto) => (
          <Link
            to={`/albums/${album.id}`}
            key={album.id}
            className="bg-card border border-border p-3 rounded-lg flex flex-col h-full shadow-sm hover:shadow-md transition"
          >
            <img
              src={album.coverUrl || ""}
              alt={album.title}
              className="w-full h-32 object-cover rounded mb-2"
            />
            <p className="text-sm mt-auto text-foreground font-medium">{album.title}</p>
          </Link>
        )}
        className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4" />

      <CreateAlbumModal
        open={open}
        setOpen={setOpen}
        artistId={id!}
        onSubmit={handleSubmitAlbum} />

      <ArtistModal
        open={editOpen}
        setOpen={setEditOpen}
        initialData={{
          name: artist.name,
          avatarUrl: artist.avatarUrl,
        }}
        onSubmit={handleUpdateArtist}
      />
    </div>
  );
}

