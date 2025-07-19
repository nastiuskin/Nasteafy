import { useState } from "react";
import { client } from "../../api/ApiClientProvider";
import { Button } from "../../components/ui/button";
import PlaylistCard from "./components/PlaylistCard";
import { PagedRequest, UserPlaylistDto } from "../../api/apiClient";
import { handleApiError } from "../../helpers/handleApiError";
import PaginatedList from "../../components/Pagination";
import PlaylistModal, { type PlaylistFormData } from "./components/CreateUpdatePlaylistModal";
import { Music } from "lucide-react";

export default function PlaylistsPage() {
  const [open, setOpen] = useState(false);
  const [refreshKey, setRefreshKey] = useState(0);

  const fetchPlaylists = async (page: number, pageSize: number) => {
    const pagedRequest = new PagedRequest();
    pagedRequest.init({
      pageNumber: page,
      pageSize: pageSize,
    });

    try {
      const response = await client.paginatedSearch5(pagedRequest);
      return {
        items: response.items ?? [],
        totalPages: response.totalPages ?? 1,
      };
    } catch (err) {
      handleApiError(err);
      return { items: [], totalPages: 1 };
    }
  };

  const handleCreatePlaylist = async (data: PlaylistFormData) => {
    try {
      await client.playlistsPOST(
        data.title,
        data.coverFile
          ? { data: data.coverFile, fileName: data.coverFile.name }
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
        <div className="bg-gradient-to-br from-green-600 via-emerald-500 to-cyan-500 p-6 sm:p-8 flex items-center justify-between">
          <div className="flex items-center gap-4">
            <div className="bg-white/20 backdrop-blur-sm p-4 rounded-lg">
              <Music className="w-10 h-10 text-white" />
            </div>
            <div>
              <h1 className="text-3xl sm:text-4xl font-bold text-white">My Playlists</h1>
              <p className="text-sm text-white/80">Create and manage your personal playlists</p>
            </div>
          </div>

          <Button
            className="bg-white text-black hover:bg-white/80"
            onClick={() => setOpen(true)}
          >
            Create Playlist
          </Button>
        </div>
      </div>

      <PlaylistModal
        open={open}
        setOpen={setOpen}
        onSubmit={handleCreatePlaylist}
      />

      <PaginatedList
        key={refreshKey}
        fetchPage={fetchPlaylists}
        renderItem={(pl: UserPlaylistDto) => (
          <PlaylistCard key={pl.id} playlist={pl} />
        )}
        pageSize={9}
        className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4"
      />
    </div>
  );
}
