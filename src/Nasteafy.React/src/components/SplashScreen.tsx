import { useEffect, useState } from "react";
import logo from "../assets/logo.png"; 
import { Loader, Loader2 } from "lucide-react";

export default function SplashScreen() {
  const [isVisible, setIsVisible] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => setIsVisible(false), 2000); 
    return () => clearTimeout(timer);
  }, []);

  if (!isVisible) return null;

  return (
    <div className="fixed inset-0 z-50 flex flex-col items-center justify-center bg-background text-foreground">
      <img
        src={logo}
        alt="Nasteafy logo"
        className="w-50 h-40 animate-fade-in"
      />
     <Loader2 className="mt-6 w-6 h-6 animate-spin text-muted-foreground" />
    </div>
  );
}
