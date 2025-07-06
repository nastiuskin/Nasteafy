import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import toast from "react-hot-toast";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "../../../components/ui/dialog";
import { Input } from "../../../components/ui/input";
import { Button } from "../../../components/ui/button";

const PlaylistSchema = z.object({
  title: z.string().min(1, "Title is required"),
  coverFile: z.instanceof(File).optional(),
});

export type PlaylistFormData = z.infer<typeof PlaylistSchema>;

type PlaylistModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  initialData?: {
    title?: string;
    coverUrl?: string;
  };
  onSubmit: (data: PlaylistFormData) => Promise<void>;
};

export default function PlaylistModal({
  open,
  setOpen,
  initialData,
  onSubmit,
}: PlaylistModalProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
    setValue,
  } = useForm<PlaylistFormData>({
    resolver: zodResolver(PlaylistSchema),
    defaultValues: {
      title: initialData?.title || "",
    },
  });

  const handleFormSubmit = async (data: PlaylistFormData) => {
    try {
      await onSubmit(data);
      toast.success(initialData ? "Playlist updated" : "Playlist created");
      setOpen(false);
      reset();
    } catch (err) {
      toast.error("Operation failed");
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{initialData ? "Edit Playlist" : "Create New Playlist"}</DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit(handleFormSubmit)} className="space-y-4">
          <div>
            <Input placeholder="Playlist title" {...register("title")} />
            {errors.title && (
              <p className="text-red-500 text-xs mt-1">{errors.title.message}</p>
            )}
          </div>
          <div>
            <Input
              type="file"
              accept="image/*"
              onChange={(e) => {
                const file = e.target.files?.[0];
                if (file) setValue("coverFile", file, { shouldValidate: true });
              }}
            />
            {errors.coverFile && (
              <p className="text-red-500 text-xs mt-1">{errors.coverFile.message}</p>
            )}
          </div>
          <Button type="submit">{initialData ? "Update" : "Create"}</Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}
