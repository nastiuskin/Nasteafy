import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';
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

export default function Topbar() {
  const { isAuthenticated, user, logout } = useAuth();
  const [showConfirm, setShowConfirm] = useState(false);

  const navigate = useNavigate();

  //  useEffect(() => {
  //   if (!isAuthReady || !isAuthenticated) return;
  //   const loadProfile = async () => {
  //     const profile = await client.profileGET();
  //     if (profile) {
  //       setUser(profile);
  //     }
  //   };

  //   loadProfile();
  // }, [isAuthReady, isAuthenticated, user?.avatarUrl]);

  return (
    <>
      <div className="h-16 px-6 flex items-center justify-between bg-neutral-900 border-b border-neutral-800">
        <input
          type="text"
          placeholder="Search..."
          className="bg-neutral-800 text-white px-4 py-2 rounded w-1/2 placeholder:text-neutral-400"
        />

        <div className="flex gap-4 items-center">
          {isAuthenticated ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Avatar className="w-8 h-8 cursor-pointer border border-neutral-700">
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

            <DropdownMenuContent className="w-56 bg-neutral-800 text-white border-neutral-700 mt-2">
              <div className="px-3 py-2 text-sm text-neutral-400">
                {user?.email || "anonymous@example.com"}
              </div>
              <DropdownMenuSeparator className="bg-neutral-600" />
            <DropdownMenuItem onClick={() => navigate("/profile")}>
                Profile
            </DropdownMenuItem>
              <DropdownMenuSeparator className="bg-neutral-600" />
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
                className="text-white hover:text-blue-400 font-medium"
              >
                SignUp
              </Link>
              <Link
                to="/login"
                className="bg-white text-black px-4 py-2 rounded-full hover:bg-neutral-200 font-semibold"
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
};
