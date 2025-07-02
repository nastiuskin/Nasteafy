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
  file: z
    .custom<File>((val) => val instanceof File, "Audio file is required")
    .refine((file) => file?.type.startsWith("audio/"), "Only audio files allowed"),
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

    try {
    //   await client.tracksPOST(
    //     data.title,
    //     {
    //       data: data.file,
    //       fileName: data.file.name,
    //     },
    //     [albumId]
    //   );
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
            {errors.file && <p className="text-sm text-red-500">{errors.file.message}</p>}
          </div>
          <Button type="submit" disabled={loading}>
            Upload
          </Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}
