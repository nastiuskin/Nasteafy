import { Link } from "react-router-dom";
import { Card, CardContent } from "../../../components/ui/card";
import type { ArtistDto } from "../../../api/apiClient";
import { Verified, User } from "lucide-react";

type ArtistCardProps = {
  artist: ArtistDto;
}

export default function ArtistCard({ artist }: ArtistCardProps) {
  return (
    <Link to={`/artists/${artist.id}`} className="group">
      <Card className="flex flex-col h-full overflow-hidden rounded-xl shadow hover:shadow-lg transition-all bg-muted/40 border border-border">
        <div className="w-full aspect-[3/4] bg-muted relative overflow-hidden">
          {artist.avatarUrl ? (
            <img
              src={artist.avatarUrl}
              alt={artist.name}
              className="absolute inset-0 w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
            />
          ) : (
            <div className="absolute inset-0 flex items-center justify-center">
              <User className="w-10 h-10 text-muted-foreground" />
            </div>
          )}
        </div>

        <CardContent className="p-3 text-center flex-1 flex flex-col justify-center">
          <p className="text-sm font-semibold text-foreground flex justify-center items-center gap-1">
            {artist.name}
            {artist.isVerified && <Verified className="w-4 h-4 text-blue-500" />}
          </p>
        </CardContent>
      </Card>
    </Link>
  );
}
