import { Link } from 'react-router-dom';
import {Home, Music, Settings2, Mic, Heart } from 'lucide-react';
import { useAuth } from '../hooks/useAuth';
import logo from "../assets/logo.png";

export default function Sidebar() {
  const { isAuthenticated, isAdmin } = useAuth();

  return (
    <div className="p-4 text-sm font-medium flex flex-col h-full text-muted-foreground">
   <Link to="/" className="mb-8 flex items-center gap-3">
 <img
  src={logo}
  alt="Nasteafy"
  className="w-12 h-12 rounded-xl bg-white p-1 shadow"
  style={{ objectFit: "cover" }}
/>
  <span className="text-xl font-bold text-foreground">Nasteafy</span>
</Link>

      {/* NAVIGATION */}
      <nav className="space-y-2">
        <Link to="/" className="flex items-center gap-2 hover:text-foreground">
          <Home className="w-4 h-4" /> Home
        </Link>
        <Link to="/artists" className="flex items-center gap-2 hover:text-foreground">
          <Mic className="w-4 h-4" /> Artists
        </Link>

        {isAuthenticated && (
          <>
            <Link to="/playlists" className="flex items-center gap-2 hover:text-foreground">
              <Music className="w-4 h-4" /> My Playlists
            </Link>
            <Link to="/liked" className="flex items-center gap-2 hover:text-foreground">
              <Heart className="w-4 h-4" /> Liked Songs
            </Link>
          </>
        )}

        {isAdmin && (
          <Link to="/admin" className="flex items-center gap-2 hover:text-foreground">
            <Settings2 className="w-4 h-4" /> Admin Panel
          </Link>
        )}
      </nav>
    </div>
  );
}