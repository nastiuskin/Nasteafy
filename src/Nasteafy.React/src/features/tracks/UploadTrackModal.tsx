import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "../../components/ui/dialog";
import { Input } from "../../components/ui/input";
import { Button } from "../../components/ui/button";
import { useState } from "react";
import { client } from "../../api/ApiClientProvider";

const schema = z.object({
  title: z.string().min(1, "Track title is required"),
  file: z.any().refine(
      (files) => files instanceof FileList && files.length > 0,
      "Audio file is required"
    )
    .refine(
      (files) => files[0]?.type?.startsWith("audio/"),
      "Only audio files allowed"
    ),
});

type FormData = z.infer<typeof schema>;

type Props = {
  open: boolean;
  setOpen: (open: boolean) => void;
  albumId: string;
  onUploaded?: () => void;
};

export default function UploadTrackModal({ open, setOpen, albumId, onUploaded }: Props) {
  const [loading, setLoading] = useState(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const onSubmit = async (data: FormData) => {
    setLoading(true);
    const file = data.file[0];
    const duration = await getAudioDuration(file);
    try {
      await client.tracksPOST(
        {
          data: file,
          fileName: file.name,
        },
        data.title,
        duration,
        albumId,
        //PASS ARTISTS TO THE REQUEST
      );
      reset();
      setOpen(false);
      onUploaded?.();
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Upload New Track</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <Input type="text" placeholder="Track title" {...register("title")} />
            {errors.title && <p className="text-sm text-red-500">{errors.title.message}</p>}
          </div>
          <div>
            <Input type="file" accept="audio/*" {...register("file")} />
            {typeof errors.file?.message === "string" && (
              <p className="text-sm text-red-500">{errors.file.message}</p>
            )}
          </div>
          <Button type="submit" disabled={loading}>
            Upload
          </Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}

async function getAudioDuration(file: File): Promise<string> {
  return new Promise((resolve) => {
    const audio = new Audio(URL.createObjectURL(file));
    audio.addEventListener("loadedmetadata", () => {
      const duration = audio.duration;
      const hours = Math.floor(duration / 3600);
      const minutes = Math.floor((duration % 3600) / 60);
      const seconds = Math.floor(duration % 60);
      resolve(
        [hours, minutes, seconds]
          .map((n) => String(n).padStart(2, "0"))
          .join(":")
      );
    });
  });
}
