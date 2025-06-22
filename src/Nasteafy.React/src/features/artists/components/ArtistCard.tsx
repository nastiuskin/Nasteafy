import { Link } from "react-router-dom";
import { Card, CardContent } from "../../../components/ui/card";
import type { ArtistDto } from "../../../api/apiClient";
import { Verified } from "lucide-react";

interface ArtistCardProps {
  artist: ArtistDto;
}

export default function ArtistCard({ artist }: ArtistCardProps) {

  return (
    <Link to={`/artists/${artist.id}`}>
      <Card className="hover:shadow-md transition h-full">
        <CardContent className="p-4 flex flex-col items-center">
          <img
            src={artist.avatarUrl || ""}
            alt={artist.name}
            className="w-24 h-24 rounded-full object-cover mb-3"
          />
          <p className="text-sm font-medium text-center flex items-center justify-center gap-1">
          <span>{artist.name}</span>
          {artist.isVerified && (
            <Verified className="w-4 h-4 text-blue-500" />
          )}
        </p>
        </CardContent>
      </Card>
    </Link>
  );
}
