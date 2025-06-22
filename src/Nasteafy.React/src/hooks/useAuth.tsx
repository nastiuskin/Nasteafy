import { useContext } from "react";
import { AuthContext } from "../contexts/AuthContext";

export function useAuth() {
  const context = useContext(AuthContext);
  const { user, isAuthenticated } = context;

  const role = user?.userRole;
  const isAdmin = role === "Admin";
  const isArtist = role === "Artist";
  const isUser = role === "User";
  const isGuest = !isAuthenticated;

  return {
    ...context,
    isAdmin,
    isArtist,
    isUser,
    isGuest,
  };
}
