import { CreatePlaylistCommand } from "../../api/apiClient";
import { client } from "../../api/ApiClientProvider";
import { Button } from "../../components/ui/button";
import PlaylistCard from "./components/PlaylistCard";
import { UserPlaylistDto } from "../../api/apiClient";
import { handleApiError } from "../../helpers/handleApiError";
import PaginatedList from "../../components/Pagination";

export default function PlaylistsPage() {
  const fetchPlaylists = async (page: number, pageSize: number) => {
    try {
      const response = await client.playlistsGET(page, pageSize);
      return {
        items: response.items ?? [],
        totalPages: response.totalPages ?? 1,
      };
    } catch (err) {
      handleApiError(err);
      return { items: [], totalPages: 1 };
    }
  };

  const handleCreatePlaylist = async () => {
    try {
      const command = new CreatePlaylistCommand({ title: "My Playlist" });
      await client.playlistsPOST(command);
    } catch (err) {
      handleApiError(err);
    }
  };

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">My Playlists</h1>
      <div className="flex justify-end mb-4">
        <Button onClick={handleCreatePlaylist}>Create Playlist</Button>
      </div>
      <PaginatedList
        fetchPage={fetchPlaylists}
        renderItem={(pl: UserPlaylistDto) => (
          <PlaylistCard key={pl.id} playlist={pl} />
        )}
        pageSize={9}
        className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4"
      />
    </div>
  );
}
