import { Heart } from "lucide-react";
import TrackList from "../../features/tracks/TrackList";
import { client } from "../../api/ApiClientProvider";
import { GetTrackDto, PagedRequest } from "../../api/apiClient";

export default function LikedSongsPage() {
  const fetchLikedTracks = async (page: number, pageSize: number): Promise<{
    items: GetTrackDto[];
    totalPages: number;
  }> => {
    const pagedRequest = new PagedRequest();
    pagedRequest.init({ pageNumber: page, pageSize });
    const res = await client.paginatedSearch2(pagedRequest);
    return {
      items: res.items ?? [],
      totalPages: res.totalPages ?? 0,
    };
  };

  return (
    <div className="p-6">
      <div className="relative w-full rounded-xl overflow-hidden mb-6">
        <div className="bg-gradient-to-br from-purple-600 via-pink-500 to-red-500 p-6 sm:p-8 flex items-center gap-6">
          <div className="bg-white/20 backdrop-blur-sm p-4 rounded-lg">
            <Heart className="w-12 h-12 text-white fill-white" />
          </div>
          <div>
            <h1 className="text-3xl sm:text-4xl font-bold text-white">Liked Songs</h1>
            <p className="text-sm sm:text-base text-white/80 mt-1">All the songs you’ve liked</p>
          </div>
        </div>
      </div>

      <TrackList fetchTracks={fetchLikedTracks} />
    </div>
  );
}
