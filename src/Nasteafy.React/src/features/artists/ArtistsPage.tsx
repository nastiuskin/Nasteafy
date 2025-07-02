import { useState } from "react";
import { client } from "../../api/ApiClientProvider";
import { handleApiError } from "../../helpers/handleApiError";
import { ArtistDto } from "../../api/apiClient";
import PaginatedList from "../../components/Pagination";
import ArtistCard from "./components/ArtistCard";
import { useAuth } from "../../hooks/useAuth";
import CreateArtistModal, { type ArtistFormData } from "./components/CreateUpdateArtistModal";
import { Button } from "../../components/ui/button";

export default function ArtistsPage() {
  const { isAdmin } = useAuth();
  const [open, setOpen] = useState(false);
  const [refreshKey, setRefreshKey] = useState(0);

  const fetchArtists = async (page: number, pageSize: number) => {
    try {
      const response = await client.artistsGET(page, pageSize);
      return {
        items: response.items ?? [],
        totalPages: response.totalPages ?? 1,
      };
    } catch (err) {
      handleApiError(err);
      return { items: [], totalPages: 1 };
    }
  };

  const handleCreateArtist = async (data: ArtistFormData) => {
    try {
      await client.artistsPOST(
        data.name,
        data.avatarFile
          ? { data: data.avatarFile, fileName: data.avatarFile.name }
          : null
      );
      setRefreshKey((k) => k + 1);
    } catch (err) {
      handleApiError(err);
    }
  };

 return (
  <div className="p-6">
    <div className="flex justify-between items-center mb-4 h-full">
      <h1 className="text-2xl font-bold">Artists</h1>

      {isAdmin && (
        <div className="flex items-center gap-4">
          <Button className="bg-blue-600 hover:bg-blue-700 text-white" onClick={() => setOpen(true)}>Add Artist</Button>

          <CreateArtistModal
            open={open}
            setOpen={setOpen}
            onSubmit={handleCreateArtist}
          />
        </div>
      )}
    </div>

    <PaginatedList
      key={refreshKey}
      fetchPage={fetchArtists}
      renderItem={(artist: ArtistDto) => <ArtistCard artist={artist} />}
      pageSize={18}
      className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 gap-4"/>
  </div>
);
}
