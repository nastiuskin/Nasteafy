import { Link } from "react-router-dom";
import type { UserPlaylistDto } from "../../../api/apiClient";
import { Card } from "../../../components/ui/card";
import { Music } from "lucide-react";
import { motion } from "framer-motion";

type PlaylistCardProps = {
  playlist: UserPlaylistDto;
};

const MotionCard = motion(Card);

export default function PlaylistCard({ playlist }: PlaylistCardProps) {
  return (
<Link to={`/playlists/${playlist.id}`} className="block w-full">
  <MotionCard
    className="group bg-muted hover:bg-muted/70 rounded-lg p-2 transition shadow hover:shadow-lg w-full max-h-[300px] max-w-[300px] aspect-[3/4] flex flex-col items-center mx-auto"
    whileHover={{ scale: 1.03 }}
    whileInView={{ opacity: 1, y: 0 }}
    viewport={{ once: true, amount: 0.3 }}
    transition={{ duration: 0.3 }}
  >
    <div className="w-full aspect-[1/1.1] overflow-hidden rounded-md">
      {playlist.coverUrl ? (
        <img
          src={playlist.coverUrl}
          alt={playlist.title}
          className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
        />
      ) : (
        <div className="w-full h-full bg-muted flex items-center justify-center rounded">
          <Music className="text-muted-foreground w-8 h-8" />
        </div>
      )}
    </div>
    <div className="mt-1 text-center w-full">
      <p className="text-sm font-semibold text-foreground truncate">
        {playlist.title}
      </p>
      <p className="text-xs text-muted-foreground">
        {playlist.tracksCount} {playlist.tracksCount === 1 ? "track" : "tracks"}
      </p>
    </div>
  </MotionCard>
</Link>
  );
}
