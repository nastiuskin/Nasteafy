import toast from "react-hot-toast";
import { ApiError } from "../api/apiClient";

export function handleApiError(error: unknown) {
  if (error instanceof ApiError) {
    toast.error(error.errorMessage ?? "Something went wrong");
  } else {
    toast.error("Something went wrong");
    console.error(error);
  }
}
