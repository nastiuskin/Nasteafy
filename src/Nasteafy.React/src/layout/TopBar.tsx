import { Link, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ConfirmDialog from '../components/ConfirmDialog';

import {
  DropdownMenu,
  DropdownMenuTrigger,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
} from "../components/ui/dropdown-menu";

import {
  Avatar,
  AvatarFallback,
  AvatarImage,
} from "../components/ui/avatar";

import { useAuth } from '../hooks/useAuth';

import { Moon, Sun } from "lucide-react";

export default function Topbar() {
  const { isAuthenticated, user, logout } = useAuth();
  const [showConfirm, setShowConfirm] = useState(false);
  const [isDark, setIsDark] = useState(() => {
    return document.documentElement.classList.contains('dark');
  });

  const navigate = useNavigate();

  const toggleTheme = () => {
    const html = document.documentElement;
    if (html.classList.contains('dark')) {
      html.classList.remove('dark');
      setIsDark(false);
    } else {
      html.classList.add('dark');
      setIsDark(true);
    }
  };

  return (
    <>
      <div className="h-16 px-6 flex items-center justify-between bg-background border-b border-border">
        <input
          type="text"
          placeholder="Search..."
          className="bg-muted text-foreground px-4 py-2 rounded w-1/2 placeholder:text-muted-foreground"
        />

        <div className="flex gap-4 items-center">
          {/* Переключатель темы через иконки */}
          <button
            onClick={toggleTheme}
            className="text-foreground hover:text-yellow-400 p-1 rounded"
            title="Toggle theme"
          >
            {isDark ? <Sun size={20} /> : <Moon size={20} />}
          </button>

          {isAuthenticated ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Avatar className="w-8 h-8 cursor-pointer border border-border">
                  <AvatarImage
                    src={user?.avatarUrl || ""}
                    alt="Avatar"
                    className="object-cover"
                  />
                  <AvatarFallback>
                    {user?.email?.[0]?.toUpperCase() || "U"}
                  </AvatarFallback>
                </Avatar>
              </DropdownMenuTrigger>

              <DropdownMenuContent className="w-56 bg-muted text-foreground border-border mt-2">
                <div className="px-3 py-2 text-sm text-muted-foreground">
                  {user?.email || "anonymous@example.com"}
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
              <Link
                to="/register"
                className="text-foreground hover:text-primary font-medium"
              >
                SignUp
              </Link>
              <Link
                to="/login"
                className="bg-primary text-primary-foreground px-4 py-2 rounded-full hover:opacity-90 font-semibold"
              >
                Login
              </Link>
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
