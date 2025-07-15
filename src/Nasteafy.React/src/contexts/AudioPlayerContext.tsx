import { createContext, useContext, useRef, useState } from "react";
import AdPopup from "../components/AdPopup"; 

type AudioPlayerContextProps = {
  currentUrl: string | null;
  playTrack: (
    url: string,
    name?: string,
    artistName?: string,
  ) => void;
  trackName: string | null;
  artistName: string | null;
  isPlaying: boolean;
}

const AudioPlayerContext = createContext<AudioPlayerContextProps>({
  currentUrl: null,
  playTrack: () => {},
  trackName: null,
  artistName: null,
  isPlaying: false,
});

export const useAudioPlayer = () => useContext(AudioPlayerContext);

export function AudioPlayerProvider({ children }: { children: React.ReactNode }) {
  const audioRef = useRef<HTMLAudioElement>(null);
  const [currentUrl, setCurrentUrl] = useState<string | null>(null);
  const [isPlaying, setIsPlaying] = useState(false);
  const [trackName, setTrackName] = useState<string | null>(null);
  const [artistName, setArtistName] = useState<string | null>(null);

  const playTrack = (url: string, name?: string, artistNameValue?: string) => {
  const isSameTrack = url === currentUrl; 
    if (audioRef.current) {
      if (isSameTrack && !audioRef.current.paused) {
        audioRef.current.pause();
        setIsPlaying(false);
    } else if (isSameTrack && audioRef.current.paused) {
        audioRef.current.play();
        setIsPlaying(true);
    } else {
        setCurrentUrl(url);
        setTrackName(name ?? null);
        setArtistName(artistNameValue ?? null);
        setIsPlaying(true);
        setTimeout(() => {  
          if (audioRef.current) {
            audioRef.current.play();
            setIsPlaying(true);
          }
        }, 0);
      }
    }
  };

  return (
   <AudioPlayerContext.Provider
      value={{
        currentUrl,
        playTrack,
        trackName,
        artistName,
        isPlaying,
      }}
    >
      {children}
      <audio ref={audioRef} src={currentUrl ?? undefined} />
    </AudioPlayerContext.Provider>
  );
}
