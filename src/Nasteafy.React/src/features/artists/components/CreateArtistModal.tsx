import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import toast from "react-hot-toast";
import { useAuth } from "../../../hooks/useAuth";
import { handleApiError } from "../../../helpers/handleApiError";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "../../../components/ui/dialog";
import { Input } from "../../../components/ui/input";
import { Button } from "../../../components/ui/button";

type CreateAritstProps =  {
  open: boolean;
  setOpen: (open: boolean) => void;
  onCreated: () => void;
}

const CreateArtistSchema = z.object({
  name: z.string().min(1, "Name is required"),
  avatarFile: z.instanceof(File).optional(),
});

type CreateArtistFormData = z.infer<typeof CreateArtistSchema>;

export default function CreateArtistModal({ open, setOpen, onCreated }: CreateAritstProps) {
  const { accessToken } = useAuth();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
    setValue,
  } = useForm<CreateArtistFormData>({
    resolver: zodResolver(CreateArtistSchema),
  });

  const onSubmit = async (data: CreateArtistFormData) => {
    const formData = new FormData();
    formData.append("Name", data.name);
    if (data.avatarFile) {
      formData.append("ArtistPhoto", data.avatarFile);
    }

    try {
      const response = await fetch("https://localhost:7041/api/artists", {
        method: "POST",
        body: formData,
        headers: {
          Authorization: `Bearer ${accessToken || ""}`,
        },
        credentials: "include",
      });

      if (response.ok) {
        toast.success("Artist created");
        setOpen(false);
        reset();
        onCreated();
      } else {
        const errorText = await response.text();
        console.error("Create failed:", errorText);
        toast.error("Failed to create artist");
      }
    } catch (err) {
      handleApiError(err);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Create New Artist</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
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
          <Button type="submit">Create</Button>
        </form>
      </DialogContent>
    </Dialog>
  );
}
