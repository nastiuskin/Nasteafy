import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import ConfirmDialog from '../components/ConfirmDialog';

import {
  DropdownMenu,
  DropdownMenuTrigger,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator
} from "../components/ui/dropdown-menu";

import {
  Avatar,
  AvatarFallback,
  AvatarImage
} from "../components/ui/avatar";

import { Button } from "../components/ui/button";
import { useAuth } from '../hooks/useAuth';
import { Moon, Sun } from "lucide-react";
import { Badge } from '../components/ui/badge';

export default function Topbar() {
  const { isAuthenticated, user, logout } = useAuth();
  const [showConfirm, setShowConfirm] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const [isDark, setIsDark] = useState(() =>
    document.documentElement.classList.contains('dark')
  );

  const navigate = useNavigate();
  const location = useLocation();

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();

    const params = new URLSearchParams(location.search);
    params.set("q", searchQuery.trim());
    navigate({ pathname: location.pathname, search: params.toString() });
  };

  const toggleTheme = () => {
    const html = document.documentElement;
    const isNowDark = !html.classList.contains("dark");

    if (isNowDark) {
      html.classList.add("dark");
      localStorage.setItem("theme", "dark");
    } else {
      html.classList.remove("dark");
      localStorage.setItem("theme", "light");
    }

    setIsDark(isNowDark);
  };

  return (
    <>
      <div className="h-16 px-6 flex items-center justify-between bg-background border-b border-border">
        <form onSubmit={handleSearch} className="w-1/2">
          <input
            type="text"
            placeholder="Search..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full bg-muted text-foreground px-4 py-2 rounded placeholder:text-muted-foreground"
          />
        </form>

        <div className="flex gap-4 items-center">
          <Button
            variant="ghost"
            size="icon"
            onClick={toggleTheme}
            title="Toggle theme"
            className="text-foreground hover:text-yellow-400"
          >
            {isDark ? <Sun size={20} /> : <Moon size={20} />}
          </Button>

          <Button
            variant="ghost"
            onClick={() => navigate("/subscriptions")}
            className="text-sm text-foreground hover:underline flex items-center gap-1"
          >
            {user?.subscriptionType ? (
              <>
                {user.subscriptionType}
                <Badge variant="success">Active</Badge>
              </>
            ) : (
              "Premium"
            )}
          </Button>

          {isAuthenticated ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Avatar className="w-8 h-8 cursor-pointer border border-border">
                  <AvatarImage
                    src={user?.avatarUrl || ""}
                    alt="Avatar"
                    className="w-full h-full object-cover rounded-full"
                  />
                  <AvatarFallback>
                    {user?.email?.[0]?.toUpperCase() || "U"}
                  </AvatarFallback>
                </Avatar>
              </DropdownMenuTrigger>

              <DropdownMenuContent className="w-56 bg-muted text-foreground border-border mt-2">
                <div className="px-3 py-2 text-sm text-muted-foreground">
                  {user?.email || ""}
                </div>
                <DropdownMenuSeparator className="bg-border" />
                <DropdownMenuItem onClick={() => navigate("/profile")}>
                  Profile
                </DropdownMenuItem>
                <DropdownMenuSeparator className="bg-border" />
                <DropdownMenuItem
                  onClick={() => setShowConfirm(true)}
                  className="text-red-400 hover:text-red-300"
                >
                  Logout
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          ) : (
            <>
              <Button variant="ghost" asChild>
                <Link to="/register">SignUp</Link>
              </Button>

              <Button variant="default" asChild className="rounded-full">
                <Link to="/login">Login</Link>
              </Button>
            </>
          )}
        </div>
      </div>

      {showConfirm && (
        <ConfirmDialog
          message="Are you sure you want to log out?"
          onCancel={() => setShowConfirm(false)}
          onConfirm={() => {
            setShowConfirm(false);
            logout();
          }}
          confirmText="Logout"
          cancelText="Stay"
        />
      )}
    </>
  );
}
