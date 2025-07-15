import { useState, useEffect } from "react";
import { Dialog, DialogContent } from "../components/ui/dialog";
import { useAuth } from "../hooks/useAuth";
import { useAudioPlayer } from "../contexts/AudioPlayerContext";

export default function AdPopup() {
    const { user, isAuthenticated } = useAuth();
    const [showAd, setShowAd] = useState(false);
    const [wasPlaying, setWasPlaying] = useState(false);
    const { currentUrl, playTrack } = useAudioPlayer();

    useEffect(() => {
        const shouldShowAds = !isAuthenticated || user?.subscriptionType === "Free";
        if (!shouldShowAds) return;

        const timer = setInterval(() => {
            const audio = document.querySelector("audio");
            if (!audio?.paused) {
                setWasPlaying(true);
                setShowAd(true);
            }
        }, 5 * 1000);

        return () => clearInterval(timer);
    }, [isAuthenticated, user?.subscriptionType]);

    useEffect(() => {
        const audio = document.querySelector("audio");
        if (showAd) {
            audio?.pause();
        } else if (wasPlaying && currentUrl) {
            playTrack(currentUrl);
            setWasPlaying(false);
        }
    }, [showAd]);

    return (
        <Dialog open={showAd} onOpenChange={setShowAd}>
            <DialogContent className="p-0 bg-black">
                <iframe
                    key={String(showAd)} 
                    width="100%"
                    height="315"
                    src="https://www.youtube.com/embed/DKtBBvE6myk?autoplay=1&mute=0"
                    title="Ad"
                    allow="autoplay; encrypted-media"
                    allowFullScreen
                />
            </DialogContent>
        </Dialog>
    );
}
