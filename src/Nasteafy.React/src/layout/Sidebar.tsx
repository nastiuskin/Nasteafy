import { Link, useLocation } from "react-router-dom";
import { Home, Music, Mic, Heart } from "lucide-react";
import { useAuth } from "../hooks/useAuth";
import logo from "../assets/logo.png";

export default function Sidebar() {
  const { isAuthenticated, isAdmin, isArtist } = useAuth();
  const location = useLocation();

  const isActive = (path: string) => location.pathname === path;

  const linkClasses = (active: boolean) =>
    `group flex items-center gap-3 px-3 py-2 rounded-lg transition-all duration-200
     ${active ? "bg-[#1f2937] text-white shadow-inner" : "text-muted-foreground hover:bg-[#2a2a2a] hover:text-white"}`;

 return (
  <aside className="h-full w-full sm:w-60 bg-background text-foreground p-4">
    <div>
      <Link to="/" className="mb-8 flex items-center gap-3 px-2">
        <img
          src={logo}
          alt="Nasteafy"
          className="w-10 h-10 rounded-md bg-white p-1 shadow-md dark:shadow-none"
        />
        <span className="text-lg font-semibold tracking-tight">Nasteafy</span>
      </Link>

      <nav className="flex flex-col gap-1">
        <SidebarLink to="/" label="Home" icon={<Home className="w-5 h-5" />} active={isActive("/")} />
        <SidebarLink to="/artists" label="Artists" icon={<Mic className="w-5 h-5" />} active={isActive("/artists")} />

        {isAuthenticated && !isAdmin && (
          <SidebarLink to="/playlists" label="My Playlists" icon={<Music className="w-5 h-5" />} active={isActive("/playlists")} />
        )}
        {isAuthenticated && isArtist && (
          <SidebarLink to="/dashboard" label="Artist Dashboard" icon={<Music className="w-5 h-5" />} active={isActive("/dashboard")} />
        )}
        {isAuthenticated && (
          <SidebarLink to="/liked-songs" label="Liked Songs" icon={<Heart className="w-5 h-5" />} active={isActive("/liked-songs")} />
        )}
      </nav>
    </div>
  </aside>
);
}

function SidebarLink({
  to,
  label,
  icon,
  active,
}: {
  to: string;
  label: string;
  icon: React.ReactNode;
  active: boolean;
}) {
   return (
    <Link
      to={to}
      className={`
        ${active
          ? "bg-muted text-foreground font-semibold"
          : "text-muted-foreground hover:bg-muted hover:text-foreground"
        }
        flex items-center gap-3 px-3 py-2 rounded-lg transition-all duration-200 group relative
      `}
    >
      <div className="relative flex items-center">
        {icon}
        {active && (
          <span className="absolute -left-2 top-1/2 -translate-y-1/2 w-1 h-6 bg-primary rounded-full animate-pulse"></span>
        )}
      </div>
      <span className="text-sm">{label}</span>
    </Link>
  );
}