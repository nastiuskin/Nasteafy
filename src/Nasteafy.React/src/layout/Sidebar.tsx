import { Link } from 'react-router-dom';
import {Home, Music, Settings2, Mic, Heart } from 'lucide-react';
import { useAuth } from '../hooks/useAuth';

export default function Sidebar() {
  const { isAuthenticated, isAdmin } = useAuth();

  return (
    <div className="p-4 text-sm font-medium flex flex-col h-full text-neutral-300">
      <nav className="space-y-2">

        <Link to="/" className="flex items-center gap-2 hover:text-white">
          <Home className="w-4 h-4" /> Home
        </Link>

        <Link to="/artists" className="flex items-center gap-2 hover:text-white">
          <Mic className="w-4 h-4" /> Artists
        </Link>
        
        {isAuthenticated && (
          <>
            <Link to="/playlists" className="flex items-center gap-2 hover:text-white">
              <Music className="w-4 h-4" /> My Playlists
            </Link>

            <Link to="/liked" className="flex items-center gap-2 hover:text-white">
              <Heart className="w-4 h-4" /> Liked Songs
            </Link>
          </>
        )}

        {isAdmin && (
          <Link to="/admin" className="flex items-center gap-2 hover:text-white">
            <Settings2 className="w-4 h-4" /> Admin Panel
          </Link>
        )}
      </nav>
    </div>
  );
}
