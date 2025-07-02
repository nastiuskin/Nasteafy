import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Music } from "lucide-react";
import { Card, CardContent } from "../../../components/ui/card";
import { client } from "../../../api/ApiClientProvider";
import { handleApiError } from "../../../helpers/handleApiError";
import type { UserPlaylistDto } from "../../../api/apiClient";
import TrackList from "../../tracks/TrackList";

export default function PlaylistInfoCard() {
  const { id } = useParams(); 
  const [playlist, setPlaylist] = useState<UserPlaylistDto | null>(null);

  useEffect(() => {
    if (!id) return;

    const loadPlaylist = async () => {
      try {
        const data = await client.playlistsGET2(id); 
        setPlaylist(data);
      } catch (err) {
        handleApiError(err);
      }
    };

    loadPlaylist();
  }, [id]);

  if (!playlist) return <p className="text-white p-4">Loading...</p>;

  return (
    <Card className="bg-neutral-800 text-white border-none shadow-md">
      <CardContent className="p-6 flex flex-col gap-6">
        <div className="flex gap-6">
          {playlist.coverUrl ? (
            <img
              src={playlist.coverUrl}
              alt={playlist.title}
              className="w-28 h-28 object-cover rounded-lg"
            />
          ) : (
            <div className="w-28 h-28 bg-neutral-700 flex items-center justify-center rounded-lg">
              <Music className="w-10 h-10 text-white opacity-60" />
            </div>
          )}

          <div className="flex flex-col justify-center gap-1">
            <h2 className="text-xl font-bold">{playlist.title}</h2>
            <p className="text-sm text-neutral-400">
              {playlist.tracksCount} {playlist.tracksCount === 1 ? "track" : "tracks"}
            </p>
          </div>
        </div>

        <TrackList playlistId={playlist.id!} />
      </CardContent>
    </Card>
  );
}
