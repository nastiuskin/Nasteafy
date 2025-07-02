import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "react-hot-toast";

import { Card, CardContent } from "../../components/ui/card";
import { Avatar, AvatarFallback, AvatarImage } from "../../components/ui/avatar";
import { Label } from "../../components/ui/label";
import { Input } from "../../components/ui/input";
import { Button } from "../../components/ui/button";
import { useAuth } from "../../hooks/useAuth";
import { client } from "../../api/ApiClientProvider";

const schema = z.object({
  email: z.string().email("Invalid email"),
  userName: z.string().min(1, "UserName is required"),
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
      avatarFile
        ? { data: avatarFile, fileName: avatarFile.name }
        : null
    );
      toast.success("Profile updated successfully");

      setUser({
      ...user!,
      email: data.email,
      userName: data.userName,
      avatarUrl: avatarFile ? avatarUrl : user!.avatarUrl,
  });
  } catch (err) {
    toast.error("Failed to update profile");
    //handleApiError(err);
  }
};

  if (loading)
    return (
      <div className="flex justify-center items-center h-64 text-white">
        Loading profile...
      </div>
    );

  return (
   <div className="flex items-center justify-center min-h-[60vh] px-4">
  <Card className="w-full max-w-xl rounded-2xl shadow-2xl border bg-card text-card-foreground border-border">
    <CardContent className="p-8 space-y-8">
      <h1 className="text-2xl font-semibold text-center">Profile Details</h1>

      <div className="flex flex-col sm:flex-row items-center gap-6">
        <Avatar className="w-28 h-28 border-4 border-border shadow-md">
          <AvatarImage src={avatarUrl || ""} alt="Avatar" className="object-cover" />
          <AvatarFallback className="text-xl">
            {user?.email?.[0]?.toUpperCase() || "U"}
          </AvatarFallback>
        </Avatar>

        <div className="w-full">
          <Label htmlFor="avatar-upload" className="text-sm text-muted-foreground">
            Upload new photo
          </Label>
          <div className="relative mt-2">
            <input
              id="avatar-upload"
              type="file"
              accept="image/*"
              onChange={handleAvatarChange}
              className="absolute inset-0 w-full h-full opacity-0 z-10 cursor-pointer"/>
            <label
              htmlFor="avatar-upload"
              className="block bg-secondary text-secondary-foreground py-2 px-4 rounded cursor-pointer text-center">
              Choose a file
            </label>
          </div>
        </div>
      </div>

      <div>
        <Label htmlFor="email" className="text-sm text-muted-foreground">
          Email address
        </Label>
        <Input
          id="email"
          type="email"
          {...register("email")}
          className="mt-2 bg-input text-foreground border"/>
        {errors.email && <p className="text-sm text-destructive">{errors.email.message}</p>}
      </div>

      <div>
        <Label htmlFor="userName" className="text-sm text-muted-foreground">
          UserName
        </Label>
        <Input
          id="userName"
          type="text"
          {...register("userName")}
          className="mt-2 bg-input text-foreground border"/>
        {errors.userName && <p className="text-sm text-destructive">{errors.userName.message}</p>}
      </div>

      <div className="flex justify-end">
        <Button
          className="bg-primary text-primary-foreground hover:brightness-90 px-6 py-2 text-sm rounded-full transition-all"
          onClick={handleSubmit(onSubmit)}>
          Save changes
        </Button>
      </div>
    </CardContent>
  </Card>
</div>
  );
}
