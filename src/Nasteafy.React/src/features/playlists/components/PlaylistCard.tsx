import { Link } from "react-router-dom";
import { Music } from "lucide-react";
import type { UserPlaylistDto } from "../../../api/apiClient";
import { Card, CardContent } from "../../../components/ui/card";

type PlaylistCardProps = {
  playlist: UserPlaylistDto;
}

export default function PlaylistCard({ playlist }: PlaylistCardProps) {
  return (
    <Link to={`/playlists/${playlist.id}`} className="hover:scale-[1.02] transition-transform">
      <Card className="bg-muted text-foreground border border-border hover:bg-muted/80 cursor-pointer">
        <CardContent className="p-4 flex gap-4 items-center">
          {playlist.coverUrl ? (
            <img
              src={playlist.coverUrl}
              alt={playlist.title}
              className="w-14 h-14 object-cover rounded"
            />
          ) : (
            <div className="w-14 h-14 bg-muted flex items-center justify-center rounded">
              <Music className="text-foreground opacity-60 w-6 h-6" />
            </div>
          )}
          <div className="flex flex-col justify-center">
            <div className="font-semibold">{playlist.title}</div>
            <div className="text-sm text-muted-foreground">
              {playlist.tracksCount} {playlist.tracksCount === 1 ? "track" : "tracks"}
            </div>
          </div>
        </CardContent>
      </Card>
    </Link>
  );
}
