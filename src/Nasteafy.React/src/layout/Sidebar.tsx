import { Link, useLocation } from 'react-router-dom';
import { Home, Music, Mic, Heart } from 'lucide-react';
import { useAuth } from '../hooks/useAuth';
import logo from "../assets/logo.png";

export default function Sidebar() {
  const { isAuthenticated } = useAuth();
  const location = useLocation();

  const isActive = (path: string) => location.pathname === path;

  const linkClasses = (active: boolean) =>
    `flex items-center gap-2 px-2 py-1 rounded hover:text-foreground transition ${active ? 'text-foreground bg-muted font-semibold' : ''
    }`;

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

      <nav className="space-y-2">
        <Link to="/" className={linkClasses(isActive("/"))}>
          <Home className="w-4 h-4" /> Home
        </Link>
        <Link to="/artists" className={linkClasses(isActive("/artists"))}>
          <Mic className="w-4 h-4" /> Artists
        </Link>

        {isAuthenticated && (
          <>
            <Link to="/playlists" className={linkClasses(isActive("/playlists"))}>
              <Music className="w-4 h-4" /> My Playlists
            </Link>
          </>
        )}
      </nav>
    </div>
  );
}
