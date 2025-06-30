import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { client } from "../../api/ApiClientProvider";
import type { AlbumDto, ArtistDto } from "../../api/apiClient";
import { handleApiError } from "../../helpers/handleApiError";
import PaginatedList from "../../components/Pagination";
import { Button } from "../../components/ui/button";
import CreateAlbumModal from "./components/CreateAlbumModal";
import ConfirmDialog from "../../components/ConfirmDialog";
import toast from "react-hot-toast";

export default function ArtistProfilePage() {
  const { id } = useParams();
  const [artist, setArtist] = useState<ArtistDto | null>(null);
  const [open, setOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [refreshKey, setRefreshKey] = useState(0); 
  const navigate = useNavigate();

  useEffect(() => {
    const fetchArtist = async () => {
      try {
        const data = await client.artistsGET2(id!);
        setArtist(data);
      } catch (error) {
        handleApiError(error);
      }
    };

    fetchArtist();
  }, [id]);

  const fetchAlbums = async (page: number, pageSize: number) => {
    try {
      const response = await client.albumsGET(id!, page, pageSize);
      return {
        items: response.items ?? [],
        totalPages: response.totalPages ?? 1,
      };
    } catch (error) {
      handleApiError(error);
      return { items: [], totalPages: 1 };
    }
  };

  const handleConfirmDeleteArtist = async() => {
    try{
      await client.artistsDELETE(artist?.id!);
      toast("Artist deleted");
      setArtist(null);
      navigate("/artists");
    }catch(error)
    {
      handleApiError(error);
    }
    finally
    {
      setConfirmOpen(false);
    }
  };

  const handleCancelDeleteArtist = () => {
    setConfirmOpen(false);
  };

  if (!artist) return <div className="p-6">Loading...</div>;

  return (
    <div className="p-6">
      <div className="flex items-center gap-6 mb-8">
        <img
          src={artist.avatarUrl || ""}
          alt={artist.name}
          className="w-32 h-32 object-cover rounded-full"/>
        <div className="flex flex-col space-y-2	">
          <h1 className="text-3xl font-bold">{artist.name}</h1>
        <div className="flex justify-between items-center mb-6 gap-2">
        <Button onClick={() => setOpen(true)} className="bg-blue-600 hover:bg-blue-700 text-white">
          Add Album
        </Button>
        <Button onClick={() => setConfirmOpen(true)} className="bg-red-600 hover:bg-red-700 text-white">
          Delete Artist
        </Button>
        {confirmOpen && (
        <ConfirmDialog
          message="Are you sure you want to delete this artist?"
          onConfirm={handleConfirmDeleteArtist}
          onCancel={handleCancelDeleteArtist}
          confirmText="Yes, Delete"
          cancelText="No"/>
      )}
        </div>
      </div>
    </div>

      <PaginatedList
        key={refreshKey} 
        fetchPage={fetchAlbums}
        pageSize={8}
        renderItem={(album: AlbumDto) => (
        <div
          key={album.id}
          className="bg-neutral-800 p-3 rounded-lg flex flex-col h-full">
          <img
            src={album.coverUrl || ""}
            alt={album.title}
            className="w-full h-32 object-cover rounded mb-2"/>
          <p className="text-sm mt-auto">{album.title}</p>
        </div>
      )}
        className="grid grid-cols-2 md:grid-cols-4 gap-4"/>

      <CreateAlbumModal
        artistId={id!}
        open={open}
        onClose={() => setOpen(false)}
        onCreated={() => setRefreshKey((prev) => prev + 1)}/>
    </div>
  );
}
