import { useState } from "react";
import { client } from "../../api/ApiClientProvider";
import { handleApiError } from "../../helpers/handleApiError";
import { ArtistDto, PagedRequest } from "../../api/apiClient";
import PaginatedList from "../../components/Pagination";
import ArtistCard from "./components/ArtistCard";
import { useAuth } from "../../hooks/useAuth";
import CreateArtistModal, { type ArtistFormData } from "./components/CreateUpdateArtistModal";
import { Button } from "../../components/ui/button";
import { motion } from "framer-motion";
import { Mic } from "lucide-react";

export default function ArtistsPage() {
  const { isAdmin } = useAuth();
  const [open, setOpen] = useState(false);
  const [refreshKey, setRefreshKey] = useState(0);

  const fetchArtists = async (page: number, pageSize: number) => {
    const pagedRequest = new PagedRequest();
    pagedRequest.init({
      pageNumber: page,
      pageSize,
      filters: [],
      sortBy: null,
      sortDirection: null,
    });

    try {
      const response = await client.paginatedSearch6(pagedRequest);
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
        data.biography,
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
      <div className="relative w-full rounded-xl overflow-hidden mb-6">
        <div className="bg-gradient-to-br from-indigo-600 via-purple-500 to-pink-500 p-6 sm:p-8 flex items-center justify-between">
          <div className="flex items-center gap-4">
            <div className="bg-white/20 backdrop-blur-sm p-4 rounded-lg">
              <Mic className="w-10 h-10 text-white" />
            </div>
            <div>
              <h1 className="text-3xl sm:text-4xl font-bold text-white">Artists</h1>
              <p className="text-sm text-white/80">Browse and manage your favorite artists</p>
            </div>
          </div>

          {isAdmin && (
            <Button
              className="bg-white text-black hover:bg-white/80"
              onClick={() => setOpen(true)}
            >
              Add Artist
            </Button>
          )}
        </div>
      </div>

      {isAdmin && (
        <CreateArtistModal
          open={open}
          setOpen={setOpen}
          onSubmit={handleCreateArtist}
        />
      )}

      <PaginatedList
        key={refreshKey}
        fetchPage={fetchArtists}
        renderItem={(artist: ArtistDto, i) => (
          <motion.div
            key={artist.id}
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: i * 0.03 }}
          >
            <ArtistCard artist={artist} />
          </motion.div>
        )}
        pageSize={18}
        className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 gap-4"
      />
    </div>
  );
}
