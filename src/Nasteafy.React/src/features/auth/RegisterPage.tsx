import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-hot-toast";

import { Label } from "../../components/ui/label";
import { Input } from "../../components/ui/input";
import { Button } from "../../components/ui/button";
import { client } from "../../api/ApiClientProvider";
import { handleApiError } from "../../helpers/handleApiError";
import { RegisterRequest } from "../../api/apiClient";

const registerSchema = z.object({
    email: z.string().email("Invalid email"),
    password: z.string().min(6, "Password must be at least 6 characters"),
    repeatPassword: z.string(),
  })
  .refine((data) => data.password === data.repeatPassword, {
    message: "Passwords do not match",
    path: ["repeatPassword"],
  });

type RegisterFormData = z.infer<typeof registerSchema>;

export default function Register() {
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  const onSubmit = async (data: RegisterFormData) => {
    try {
      const command = new RegisterRequest({ email: data.email, password: data.password });
      await client.register(command);
      toast.success("Registration successful");
      navigate("/login");
    } catch (err) {
      handleApiError(err);
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-black text-white px-4">
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="bg-neutral-900 p-8 rounded-2xl w-full max-w-md shadow-lg space-y-6"
      >
        <h2 className="text-2xl font-bold text-center">Create your account</h2>

        <div className="space-y-2">
          <Label htmlFor="email">Email</Label>
          <Input
            id="email"
            type="email"
            placeholder="you@example.com"
            {...register("email")}
          />
          {errors.email && (
            <p className="text-red-500 text-sm">{errors.email.message}</p>
          )}
        </div>

        <div className="space-y-2">
          <Label htmlFor="password">Password</Label>
          <Input
            id="password"
            type="password"
            placeholder="••••••••"
            {...register("password")}
          />
          {errors.password && (
            <p className="text-red-500 text-sm">{errors.password.message}</p>
          )}
        </div>

        <div className="space-y-2">
          <Label htmlFor="repeat-password">Repeat Password</Label>
          <Input
            id="repeat-password"
            type="password"
            placeholder="••••••••"
            {...register("repeatPassword")}
          />
          {errors.repeatPassword && (
            <p className="text-red-500 text-sm">{errors.repeatPassword.message}</p>
          )}
        </div>

        <Button type="submit" className="w-full">
          Register
        </Button>

        <div className="text-sm text-center text-neutral-400">
          Already have an account?{" "}
          <Link to="/login" className="text-blue-400 hover:underline">
            Login
          </Link>
        </div>
      </form>
    </div>
  );
}
