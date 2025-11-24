import { useEffect } from "react";
import { useLocation } from "react-router-dom";
import { useAudioPlayer } from "../contexts/AudioPlayerContext";

export function useResetPlayerOnLogin() {
  const { resetPlayer } = useAudioPlayer();
  const location = useLocation();

  useEffect(() => {
    if (location.pathname === "/login") {
      resetPlayer();
    }
  }, [location.pathname, resetPlayer]);
}
