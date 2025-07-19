import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { client } from "../../api/ApiClientProvider";
import { PagedRequest, type AlbumDto, type ArtistDto } from "../../api/apiClient";
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
      const pagedRequest = new PagedRequest();
      pagedRequest.init({
        pageNumber: page,
        pageSize: pageSize,
      });
      const response = await client.paginatedSearch7(id!, pagedRequest);
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
        data.biography,
        data.avatarFile
          ? { data: data.avatarFile, fileName: data.avatarFile.name }
          : null
      );
      setEditOpen(false);
      const updated = await client.artistsGET(id!);
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
    <div className="relative min-h-screen bg-gradient-to-tr from-background via-muted/30 to-background px-2 sm:px-4 py-6">
      <div className="bg-card/80 border border-border rounded-2xl shadow-xl backdrop-blur-lg p-4 sm:p-6 flex flex-col sm:flex-row gap-6">
        <div className="relative w-44 h-44 sm:w-56 sm:h-56 rounded-full overflow-hidden shadow-lg border-4 border-primary shrink-0">
          {artist.avatarUrl ? (
            <img
              src={artist.avatarUrl}
              alt={artist.name}
              className="w-full h-full object-cover"
            />
          ) : (
            <div className="w-full h-full bg-muted flex items-center justify-center text-muted-foreground">
              <User className="w-12 h-12" />
            </div>
          )}
          {isAdmin && (
            <div className="absolute inset-0 bg-black/40 flex items-center justify-center opacity-0 hover:opacity-100 transition-opacity">
              <Button
                type="button"
                variant="ghost"
                className="text-white bg-transparent hover:bg-transparent"
                onClick={() => setEditOpen(true)}
              >
                <Pencil className="w-6 h-6" />
              </Button>
            </div>
          )}
        </div>

        <div className="flex flex-col justify-center gap-4 flex-1">
          <h1 className="text-4xl font-bold text-foreground">{artist.name}</h1>

          {artist.biography && (
            <div className="relative max-h-60 overflow-auto pr-3">
              <div className="text-sm leading-relaxed text-muted-foreground whitespace-pre-line">
                {artist.biography}
              </div>
              <div className="absolute top-0 right-0 w-2 h-full bg-gradient-to-l from-background to-transparent" />
            </div>
          )}

          {isAdmin && (
            <div className="flex gap-2 flex-wrap">
              <Button
                onClick={() => setOpen(true)}
                className="bg-primary text-primary-foreground"
              >
                Add Album
              </Button>
              <Button
                onClick={() => setConfirmOpen(true)}
                className="bg-destructive text-destructive-foreground"
              >
                Delete Artist
              </Button>
            </div>
          )}
        </div>
      </div>

      <div className="mt-6">
        <PaginatedList
          key={refreshKey}
          fetchPage={fetchAlbums}
          pageSize={8}
          emptyContent={<p className="text-muted-foreground text-sm">No albums yet</p>}
          renderItem={(album: AlbumDto) => (
            <Link
              to={`/albums/${album.id}`}
              key={album.id}
              className="bg-card border border-border p-3 rounded-xl flex flex-col h-full shadow-sm hover:shadow-md transition"
            >
              <div className="aspect-[1/1] w-full rounded overflow-hidden mb-2">
                <img
                  src={album.coverUrl || ""}
                  alt={album.title}
                  className="w-full h-full object-cover"
                />
              </div>
              <p className="text-sm mt-auto text-foreground font-medium truncate">{album.title}</p>
            </Link>
          )}
          className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4"
        />
      </div>

      <CreateAlbumModal
        open={open}
        setOpen={setOpen}
        artistId={id!}
        onSubmit={handleSubmitAlbum}
      />

      <ArtistModal
        open={editOpen}
        setOpen={setEditOpen}
        initialData={{
          name: artist.name,
          avatarUrl: artist.avatarUrl,
          biography: artist.biography,
        }}
        onSubmit={handleUpdateArtist}
      />

      {confirmOpen && (
        <ConfirmDialog
          message="Are you sure you want to delete this artist?"
          onConfirm={handleConfirmDeleteArtist}
          onCancel={handleCancelDeleteArtist}
          confirmText="Yes, Delete"
          cancelText="No"
        />
      )}
    </div>
  );
}