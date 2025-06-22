import { Pause, Play } from "lucide-react";
import { useAudioPlayer } from "../../contexts/AudioPlayerContext";
import { useEffect, useRef, useState } from "react";

export default function CurrentTrackPlayer() {
  const { currentUrl, playTrack, isPlaying, trackName, artistName } = useAudioPlayer();
  const audioRef = useRef<HTMLAudioElement | null>(null);
  const [progress, setProgress] = useState(0);
  const [duration, setDuration] = useState(0);

  useEffect(() => {
    const audio = document.querySelector("audio");
    if (audio) {
      audioRef.current = audio;

      const updateProgress = () => {
        if (audioRef.current) {
          setProgress(audioRef.current.currentTime);
          setDuration(audioRef.current.duration || 0);
        }
      };

      audio.addEventListener("timeupdate", updateProgress);
      return () => {
        audio.removeEventListener("timeupdate", updateProgress);

      };
    }
  }, []);

  if (!currentUrl) return null;

  const formatTime = (sec: number) => {
    const m = Math.floor(sec / 60)
      .toString()
      .padStart(2, "0");
    const s = Math.floor(sec % 60)
      .toString()
      .padStart(2, "0");
    return `${m}:${s}`;
  };

  return (
    <div className="fixed bottom-0 left-0 right-0 bg-neutral-900 text-white border-t border-neutral-700 px-6 py-4 shadow-lg z-50">
      <div className="flex items-center justify-between w-full">
        <div className="flex flex-col truncate max-w-[30%]">
          <span className="text-base font-semibold truncate">{trackName}</span>
          <span className="text-sm text-neutral-400 truncate">{artistName}</span>
        </div>

        <div className="flex flex-col items-center w-1/3">
          <input
            type="range"
            min={0}
            max={duration}
            step={0.1}
            value={progress}
            onChange={(e) => {
              if (audioRef.current) {
                audioRef.current.currentTime = Number(e.target.value);
              }
            }}
            className="w-full h-1 bg-neutral-600 rounded-full appearance-none accent-white cursor-pointer"
          />
          <div className="flex justify-between w-full text-xs text-neutral-400 mt-1">
            <span>{formatTime(progress)}</span>
            <span>{formatTime(duration)}</span>
          </div>
        </div>

        <div className="flex items-center justify-end w-[10%]">
          <button
            onClick={() => playTrack(currentUrl)}
            className="p-2 rounded hover:bg-neutral-700"
          >
            {isPlaying? (
              <Pause className="w-6 h-6 text-white" />
            ) : (
              <Play className="w-6 h-6 text-white" />
            )}
          </button>
        </div>
      </div>
    </div>
  );
}
