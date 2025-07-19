import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "react-hot-toast";
import { Pencil } from "lucide-react";

import { useAuth } from "../../hooks/useAuth";
import { client } from "../../api/ApiClientProvider";
import { Input } from "../../components/ui/input";
import { Button } from "../../components/ui/button";
import { Label } from "../../components/ui/label";

const schema = z.object({
  email: z.string().email("Invalid email"),
  userName: z.string().min(1, "Username is required"),
});

type ProfileFormData = z.infer<typeof schema>;

export default function ProfilePage() {
  const { user, setUser } = useAuth();
  const [avatarUrl, setAvatarUrl] = useState<string | null>(user?.avatarUrl || null);
  const [avatarFile, setAvatarFile] = useState<File | null>(null);
  const [loading, setLoading] = useState(!user);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ProfileFormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      email: user?.email || "",
      userName: user?.userName || "",
    },
  });

  useEffect(() => {
    if (user == null) {
      const loadProfile = async () => {
        try {
          const profile = await client.profileGET();
          if (profile) {
            reset({
              email: profile.email || "",
              userName: profile.userName || "",
            });
            setAvatarUrl(profile.avatarUrl || "");
          }
        } finally {
          setLoading(false);
        }
      };
      loadProfile();
    }
  }, [user]);

  const handleAvatarChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files?.[0]) {
      const file = e.target.files[0];
      setAvatarFile(file);
      const reader = new FileReader();
      reader.onload = () => {
        if (typeof reader.result === "string") {
          setAvatarUrl(reader.result);
        }
      };
      reader.readAsDataURL(file);
    }
  };

  const onSubmit = async (data: ProfileFormData) => {
    try {
      await client.profilePUT(
        data.email,
        data.userName,
        avatarFile ? { data: avatarFile, fileName: avatarFile.name } : null
      );
      toast.success("Profile updated successfully");
      setUser({
        ...user!,
        email: data.email,
        userName: data.userName,
        avatarUrl: avatarFile ? avatarUrl : user!.avatarUrl,
      });
    } catch {
      toast.error("Failed to update profile");
    }
  };

  if (loading) return <div className="text-center py-10">Loading...</div>;

 return (
  <section className="min-h-screen w-full bg-background text-foreground px-4 py-12">
    <div className="max-w-3xl mx-auto">
      {/* Cover */}
      <div className="relative w-full h-48 bg-gradient-to-r from-primary to-primary/80 rounded-xl shadow-inner flex items-center justify-center text-center px-4">
        <div>
          <h1 className="text-2xl font-bold text-white tracking-tight">Manage Your Profile</h1>
          <p className="text-sm text-white/80 mt-2">Customize your info and make your presence unique.</p>
        </div>

        {/* Avatar */}
        <div className="absolute -bottom-20 left-1/2 transform -translate-x-1/2">
          <div className="relative w-36 h-36 rounded-full overflow-hidden border-4 border-background shadow-lg bg-muted group">
            {avatarUrl ? (
              <img
                src={avatarUrl}
                alt="Avatar"
                className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
              />
            ) : (
              <div className="flex items-center justify-center w-full h-full text-3xl font-bold">
                {user?.email?.[0]?.toUpperCase() || "U"}
              </div>
            )}
            <input
              type="file"
              accept="image/*"
              onChange={handleAvatarChange}
              className="absolute inset-0 opacity-0 z-10 cursor-pointer"
            />
            <div className="absolute bottom-1 right-1 bg-white text-black rounded-full p-1 shadow">
              <Pencil className="w-4 h-4" />
            </div>
          </div>
        </div>
      </div>

      {/* Form */}
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="mt-28 bg-muted/10 border border-border rounded-xl p-6 shadow-sm space-y-6"
      >
        <div>
          <Label htmlFor="email">Email address</Label>
          <Input id="email" type="email" {...register("email")} className="mt-2" />
          {errors.email && (
            <p className="text-sm text-destructive mt-1">{errors.email.message}</p>
          )}
        </div>

        <div>
          <Label htmlFor="userName">Username</Label>
          <Input id="userName" type="text" {...register("userName")} className="mt-2" />
          {errors.userName && (
            <p className="text-sm text-destructive mt-1">{errors.userName.message}</p>
          )}
        </div>

        <div className="text-right">
          <Button type="submit" className="rounded-full px-6 py-2">
            Save Changes
          </Button>
        </div>
      </form>
    </div>
  </section>
);
}