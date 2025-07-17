import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { client } from "../../api/ApiClientProvider";
import { AlbumDto, ArtistDto, GetTrackDto, PagedRequest } from "../../api/apiClient";

import { Card, CardContent, CardHeader, CardTitle } from "../../components/ui/card";
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "../../components/ui/carousel";
import { BarChart, Music, Users } from "lucide-react";
import { useAuth } from "../../hooks/useAuth";
import { motion } from "framer-motion";
import { handleApiError } from "../../helpers/handleApiError";

export default function HomePage() {
  const [artists, setArtists] = useState<ArtistDto[]>([]);
  const [albums, setAlbums] = useState<AlbumDto[]>([]);

  const [artistPage, setArtistPage] = useState(1);
  const [albumPage, setAlbumPage] = useState(1);

  const [artistTotalPages, setArtistTotalPages] = useState<number | null>(null);
  const [albumTotalPages, setAlbumTotalPages] = useState<number | null>(null);

  const { isAuthenticated } = useAuth();
  const MotionCard = motion.create(Card);

  useEffect(() => {
    (async () => {
      const request = new PagedRequest();
      request.init({ pageNumber: artistPage, pageSize: 5 });

      try {
        const res = await client.paginatedSearch5(request);
        setArtists(res.items ?? []);
        setArtistTotalPages(res.totalPages ?? null);
      } catch (err) {
        handleApiError(err);
      }
    })();
  }, [artistPage]);

  useEffect(() => {
    (async () => {
      const request = new PagedRequest();
      request.init({ pageNumber: albumPage, pageSize: 5, sortBy: "ReleaseDate" });

      try {
        const res = await client.paginatedSearch7(request);
        setAlbums(res.items ?? []);
        setAlbumTotalPages(res.totalPages ?? null);
      } catch (err) {
        handleApiError(err);
      }
    })();
  }, [albumPage]);

  return (
    <div className="flex-1 w-full overflow-y-auto">
      <div className="max-w-7xl mx-auto px-6 py-10 space-y-12">

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          <Link to="/artists" className="block">
            <Card className="hover:shadow-md transition cursor-pointer h-full">
              <CardHeader className="flex flex-row items-center justify-between pb-2">
                <CardTitle className="text-sm font-medium">Top Artists</CardTitle>
                <Users className="h-5 w-5 text-muted-foreground" />
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">Explore Now</div>
                <p className="text-xs text-muted-foreground">See what’s trending globally</p>
              </CardContent>
            </Card>
          </Link>

          <Link to="/subscriptions" className="block">
            <Card className="hover:shadow-md transition cursor-pointer h-full">
              <CardHeader className="flex flex-row items-center justify-between pb-2">
                <CardTitle className="text-sm font-medium">Become an Artist</CardTitle>
                <Music className="h-5 w-5 text-muted-foreground" />
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">Start Uploading</div>
                <p className="text-xs text-muted-foreground">Share your music with the world</p>
              </CardContent>
            </Card>
          </Link>

          {isAuthenticated ? (
            <Link to="/playlists" className="block">
              <Card className="hover:shadow-md transition cursor-pointer h-full">
                <CardHeader className="flex flex-row items-center justify-between pb-2">
                  <CardTitle className="text-sm font-medium">Your Playlists</CardTitle>
                  <BarChart className="h-5 w-5 text-muted-foreground" />
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold">Manage</div>
                  <p className="text-xs text-muted-foreground">Customize your music</p>
                </CardContent>
              </Card>
            </Link>
          ) : (
            <Link to="/register" className="block">
              <Card className="hover:shadow-md transition cursor-pointer h-full">
                <CardHeader className="flex flex-row items-center justify-between pb-2">
                  <CardTitle className="text-sm font-medium">Join Now</CardTitle>
                  <BarChart className="h-5 w-5 text-muted-foreground" />
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold">Create Playlists</div>
                  <p className="text-xs text-muted-foreground">
                    Sign up to save tracks and build your collection
                  </p>
                </CardContent>
              </Card>
            </Link>
          )}

        </div>

        <section className="mb-10">
          <h2 className="text-2xl font-semibold mb-4 px-2 sm:px-0">Latest releases</h2>
          <div className="relative">
             <Carousel key={`albums-${albumPage}`}  opts={{ align: "start" }} className="w-full">
              <CarouselContent className="-ml-2">
                {albums.map((album, index) => (
                  <CarouselItem
                    key={album.id}
                    className="pl-2 basis-3/4 sm:basis-1/2 md:basis-1/3 lg:basis-1/5"
                  >
                    <Link to={`/albums/${album.id}`}>
                      <MotionCard
                        className="group bg-muted hover:bg-muted/70 rounded-lg p-4 transition shadow hover:shadow-lg h-full flex flex-col items-center"
                        whileHover={{ scale: 1.05 }}
                        whileInView={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.3, delay: index * 0.01 }}
                      >
                        <div className="w-full aspect-square overflow-hidden rounded-md mb-3">
                          <img
                            src={album.coverUrl || ""}
                            alt={album.title}
                            className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
                          />
                        </div>
                        <p className="text-base font-semibold text-foreground truncate w-full text-center">
                          {album.title}
                        </p>
                      </MotionCard>
                    </Link>
                  </CarouselItem>
                ))}
              </CarouselContent>

              <CarouselPrevious
                onClick={() => setAlbumPage((p) => Math.max(p - 1, 1))}
                disabled={albumPage === 1}
                className="absolute -left-6 top-1/2 -translate-y-1/2 z-10 opacity-80 hover:opacity-100 disabled:opacity-30 disabled:cursor-not-allowed w-10 h-10"
              />
              <CarouselNext
                onClick={() =>
                  setAlbumPage((p) =>
                    albumTotalPages ? Math.min(p + 1, albumTotalPages) : p + 1
                  )
                }
                disabled={albumTotalPages !== null && albumPage >= albumTotalPages}
                className="absolute -right-6 top-1/2 -translate-y-1/2 z-10 opacity-80 hover:opacity-100 disabled:opacity-30 disabled:cursor-not-allowed w-10 h-10"
              />
            </Carousel>
          </div>
        </section>

        <section className="mb-10">
          <h2 className="text-2xl font-semibold mb-4 px-2 sm:px-0">Popular Artists</h2>
          <div className="relative">
            <Carousel key={`artists-${artistPage}`}  opts={{ align: "start" }} className="w-full">
              <CarouselContent className="-ml-2">
                {artists.map((artist, index) => (
                  <CarouselItem
                    key={artist.id}
                    className="pl-2 basis-3/4 sm:basis-1/2 md:basis-1/3 lg:basis-1/5"
                  >
                    <Link to={`/artists/${artist.id}`}>
                      <MotionCard
                        className="group bg-muted hover:bg-muted/70 rounded-lg p-4 transition shadow hover:shadow-lg h-full flex flex-col items-center"
                        whileHover={{ scale: 1.05 }}
                        whileInView={{ opacity: 1, y: 0 }}
                        viewport={{ once: true, amount: 0.3 }}  
                        transition={{ duration: 0.3, delay: index * 0.01 }}
                      >
                        <div className="w-full aspect-square overflow-hidden rounded-md mb-3">
                          <img
                            src={artist.avatarUrl || ""}
                            alt={artist.name}
                            className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
                          />
                        </div>
                        <p className="text-base font-semibold text-foreground truncate w-full text-center">
                          {artist.name}
                        </p>
                      </MotionCard>
                    </Link>
                  </CarouselItem>
                ))}
              </CarouselContent>

              <CarouselPrevious
                onClick={() => setArtistPage((p) => Math.max(p - 1, 1))}
                disabled={artistPage === 1}
                className="absolute -left-6 top-1/2 -translate-y-1/2 z-10 opacity-80 hover:opacity-100 disabled:opacity-30 disabled:cursor-not-allowed w-10 h-10"
              />
              <CarouselNext
                onClick={() =>
                  setArtistPage((p) =>
                    artistTotalPages ? Math.min(p + 1, artistTotalPages) : p + 1
                  )
                }
                disabled={artistTotalPages !== null && artistPage >= artistTotalPages}
                className="absolute -right-6 top-1/2 -translate-y-1/2 z-10 opacity-80 hover:opacity-100 disabled:opacity-30 disabled:cursor-not-allowed w-10 h-10"
              />
            </Carousel>
          </div>
        </section>
      </div>
    </div>
  );
}
