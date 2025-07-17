import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import toast from "react-hot-toast";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "../../../components/ui/dialog";
import { Input } from "../../../components/ui/input";
import { Button } from "../../../components/ui/button";

const ArtistSchema = z.object({
  name: z.string().min(1, "Name is required"),
  avatarFile: z.instanceof(File).optional(),
  biography: z.string().max(1000, "Biography is too long").optional(),
});

export type ArtistFormData = z.infer<typeof ArtistSchema>;

type ArtistModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  initialData?: {
    name?: string;
    avatarUrl?: string;
    biography?: string;
  };
  onSubmit: (data: ArtistFormData) => Promise<void>;
};

export default function ArtistModal({
  open,
  setOpen,
  initialData,
  onSubmit,
}: ArtistModalProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
    setValue,
  } = useForm<ArtistFormData>({
    resolver: zodResolver(ArtistSchema),
    defaultValues: {
      name: initialData?.name || "",
      biography: initialData?.biography || "",
    },
  });
  
  const handleFormSubmit = async (data: ArtistFormData) => {
    try {
      await onSubmit(data);
      toast.success(initialData ? "Artist updated" : "Artist created");
      setOpen(false);
      reset();
    } catch (err) {
      console.error(err);
      toast.error("Operation failed");
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{initialData ? "Edit Artist" : "Create New Artist"}</DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit(handleFormSubmit)} className="space-y-4">
          <div>
            <Input placeholder="Artist name" {...register("name")} />
            {errors.name && (
              <p className="text-red-500 text-xs mt-1">{errors.name.message}</p>
            )}
          </div>
          <div>
            <Input
              type="file"
              accept="image/*"
              onChange={(e) => {
                const file = e.target.files?.[0];
                if (file) setValue("avatarFile", file, { shouldValidate: true });
              }}
            />
            {errors.avatarFile && (
              <p className="text-red-500 text-xs mt-1">{errors.avatarFile.message}</p>
            )}
          </div>
          <div>
            <textarea
              placeholder="Short biography"
              {...register("biography")}
              className="w-full border border-input rounded-md p-2 text-sm resize-y min-h-[80px] bg-background text-foreground placeholder:text-muted-foreground"
            />
            {errors.biography && (
              <p className="text-red-500 text-xs mt-1">{errors.biography.message}</p>
            )}
          </div>
          <Button type="submit">{initialData ? "Update" : "Create"}</Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}
