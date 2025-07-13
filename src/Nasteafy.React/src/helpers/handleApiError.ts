import toast from "react-hot-toast";
import { ApiError } from "../api/apiClient";

export function handleApiError(error: unknown) {
  if (error instanceof ApiError) {
    toast.error(error.errorMessage ?? "Something went wrong.Please try again later");
  } else {
    toast.error("Something went wrong.Please try again later");
  }
}
