import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { Input } from "../../../components/ui/input";
import { Button } from "../../../components/ui/button";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "../../../components/ui/dialog";
import { toast } from "react-hot-toast";
import { useAuth } from "../../../hooks/useAuth";
import { handleApiError } from "../../../helpers/handleApiError";
import { ArtistDto } from "../../../api/apiClient";
import { client } from "../../../api/ApiClientProvider";
import { Label } from "../../../components/ui/label";

const schema = z.object({
  title: z.string().min(1, "Title is required"),
  releaseDate: z.string().min(1, "Release date is required"),
});

type FormData = z.infer<typeof schema>;

interface Props {
  artistId: string;
  open: boolean;
  onClose: () => void;
  onCreated: () => void;
}

export default function CreateAlbumModal({ artistId, open, onClose, onCreated }: Props) {
  const [loading, setLoading] = useState(false);
  const { accessToken } = useAuth();
  const [artists, setArtists] = useState<ArtistDto[]>([]);
  const [coverFile, setCoverFile] = useState<File | null>(null);
  const [extraArtistIds, setExtraArtistIds] = useState<string[]>([]);

  useEffect(() => {
    const fetchArtists = async () => {
      try {
        const response = await client.artistsGET(1, 100);
        setArtists(response.items ?? []);
      } catch (err) {
        handleApiError(err);
      }
    };

    if (open) fetchArtists();
  }, [open]);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const onSubmit = async (data: FormData) => {
    const allArtistIds = [artistId, ...extraArtistIds];
    const formData = new FormData();

    formData.append("title", data.title);
    formData.append("releaseDate", new Date(data.releaseDate).toISOString());
    allArtistIds.forEach((id) => formData.append("artists", id));
    if (coverFile) {
      formData.append("coverFile", coverFile);
    }

    try {
      const response = await fetch("https://localhost:7041/api/albums", {
        method: "POST",
        body: formData,
        headers: {
          Authorization: `Bearer ${accessToken || ""}`,
        },
        credentials: "include",
      });

      if (response.ok) {
        toast.success("Album created");
        reset();
        setCoverFile(null);
        setExtraArtistIds([]);
        onCreated();
        onClose();
      } else {
        const text = await response.text();
        toast.error("Failed to create album: " + text);
      }
    } catch (err) {
      handleApiError(err);
    } finally {
      setLoading(false);
    }
  };

  const handleCoverChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files?.[0]) {
      setCoverFile(e.target.files[0]);
    }
  };

  const handleArtistSelect = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selected = Array.from(e.target.selectedOptions).map((opt) => opt.value);
    setExtraArtistIds(selected);
  };

  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Create New Album</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <Input placeholder="Album title" {...register("title")} />
            {errors.title && <p className="text-sm text-red-500">{errors.title.message}</p>}
          </div>

          <div>
            <Input type="date" {...register("releaseDate")} />
            {errors.releaseDate && (
              <p className="text-sm text-red-500">{errors.releaseDate.message}</p>
            )}
          </div>

          <div>
            <Input type="file" accept="image/*" onChange={handleCoverChange} />
          </div>

          <div>
            <Label htmlFor="extra-artists" className="text-sm text-neutral-400 block">
              Add other artists (optional)
            </Label>
            <select
              id="extra-artists"
              multiple
              onChange={handleArtistSelect}
              className="w-full bg-neutral-800 text-white border-none rounded px-2 py-2"
            >
              {artists
                .filter((a) => a.id !== artistId)
                .map((artist) => (
                  <option key={artist.id} value={artist.id}>
                    {artist.name}
                  </option>
                ))}
            </select>
          </div>

          <Button type="submit" disabled={loading}>
            Create
          </Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}
