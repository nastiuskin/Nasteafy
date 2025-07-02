import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { Input } from "../../../components/ui/input";
import { Button } from "../../../components/ui/button";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "../../../components/ui/dialog";
import { toast } from "react-hot-toast";
import { handleApiError } from "../../../helpers/handleApiError";
import { ArtistDto } from "../../../api/apiClient";
import { client } from "../../../api/ApiClientProvider";
import { Label } from "../../../components/ui/label";

const AlbumSchema = z.object({
  title: z.string().min(1, "Title is required"),
  releaseDate: z.string().min(1, "Release date is required"),
  coverFile: z.instanceof(File).optional(),
});

export type AlbumFormData = z.infer<typeof AlbumSchema>;

type AlbumModalProps = {
  artistId?: string;
  open: boolean;
  setOpen: (open: boolean) => void;
  onCreated?: () => void;
  initialData?: {
    title?: string;
    releaseDate?: string;
    coverUrl?: string;
    artistIds?: string[];
  };
  onSubmit?: (data: AlbumFormData & { extraArtistIds: string[] }) => Promise<void>;
};

export default function AlbumModal({
  artistId,
  open,
  setOpen,
  onCreated,
  initialData,
  onSubmit,
}: AlbumModalProps) {
  const [loading, setLoading] = useState(false);
  const [artists, setArtists] = useState<ArtistDto[]>([]);
  const [coverFile, setCoverFile] = useState<File | null>(null);
  const [extraArtistIds, setExtraArtistIds] = useState<string[]>([]);

  const isEdit = Boolean(initialData);

  useEffect(() => {
    const fetchArtists = async () => {
      try {
        const response = await client.artistsGET(1, 100);
        setArtists(response.items ?? []);
      } catch (err) {
        handleApiError(err);
      }
    };

    if (open && artistId) fetchArtists();
  }, [open, artistId]);

  const {
    register,
    handleSubmit,
    reset,
    setValue,
    formState: { errors },
  } = useForm<AlbumFormData>({
    resolver: zodResolver(AlbumSchema),
    defaultValues: {
      title: initialData?.title ?? "",
      releaseDate: initialData?.releaseDate ?? "",
    },
  });

  useEffect(() => {
    if (initialData?.artistIds && artistId) {
      setExtraArtistIds(initialData.artistIds.filter(id => id !== artistId));
    } else if (initialData?.artistIds) {
      setExtraArtistIds(initialData.artistIds);
    }
  }, [initialData, artistId]);

  const handleCoverChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setCoverFile(file);
      setValue("coverFile", file);
    }
  };

  const handleArtistSelect = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selected = Array.from(e.target.selectedOptions).map((opt) => opt.value);
    setExtraArtistIds(selected);
  };

  const handleFormSubmit = async (data: AlbumFormData) => {
    setLoading(true);
    const allArtistIds = artistId ? [artistId, ...extraArtistIds] : extraArtistIds;

    try {
      if (onSubmit) {
        await onSubmit({ ...data, extraArtistIds });
      } else {
        await client.albumsPOST(
          data.title,
          coverFile ? { data: coverFile, fileName: coverFile.name } : null,
          data.releaseDate ? new Date(data.releaseDate) : undefined,
          allArtistIds
        );
        toast.success("Album created");
        onCreated?.();
      }

      reset();
      setCoverFile(null);
      setExtraArtistIds([]);
      setOpen(false);
    } catch (err) {
      handleApiError(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{isEdit ? "Edit Album" : "Create New Album"}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(handleFormSubmit)} className="space-y-4">
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

          {artistId && (
            <div>
              <Label htmlFor="extra-artists" className="text-sm text-neutral-400 block">
                Add other artists (optional)
              </Label>
              <select
                id="extra-artists"
                multiple
                value={extraArtistIds}
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
          )}

          <Button type="submit" disabled={loading}>
            {isEdit ? "Update" : "Create"}
          </Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}
