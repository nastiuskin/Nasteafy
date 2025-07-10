import {
  useState,
  createContext,
  type ReactNode,
  useEffect,
} from "react";
import { handleApiError } from "../helpers/handleApiError";
import { useNavigate } from "react-router-dom";
import { client } from "../api/ApiClientProvider";
import { authService } from "../services/auth/AuthService";
import { decodeToken } from "../helpers/decodeToken"; 

export type UserType = {
  id: string,
  email: string;
  userRole: string | null;
  subscriptionType: string | null;
  userName?: string | null;
  avatarUrl?: string | null;
};

export type AuthContextType = {
  accessToken: string | null;
  isAuthenticated: boolean;
  isAuthReady: boolean;
  user: UserType | null;
  setUser: (user: UserType | null) => void;
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => void;
};

export const AuthContext = createContext<AuthContextType>({} as AuthContextType);

type AuthProviderProps = { children: ReactNode; };

export function AuthProvider({ children }: AuthProviderProps) {
  const [isAuthReady, setIsAuthReady] = useState(false);
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [user, setUser] = useState<UserType | null>(null);
  const navigate = useNavigate();

  const login = async (email: string, password: string): Promise<boolean> => {
    try {
      const token = await authService.login(email, password);
      if (!token) return false;

      localStorage.setItem("accessToken", token);
      setAccessToken(token);
      setIsAuthenticated(true);
      const userId = decodeToken(token);
      const profile = await client.profileGET();
      setUser({
        id: userId ?? "",
        email: profile.email ?? "",
        userRole: profile.userRole ?? "User",
        subscriptionType: profile.subscriptionType ?? "Free",
        userName: profile.userName,
        avatarUrl: profile.avatarUrl,
      });

      return true;
    } catch (error) {
      handleApiError(error);
      return false;
    }
  };

  const logout = async () => {
    try {
      await authService.logout();
    } catch (error) {
      handleApiError(error);
    } finally {
      localStorage.removeItem("accessToken");
      setAccessToken(null);
      setIsAuthenticated(false);
      setUser(null);
      navigate("/");
    }
  };

  useEffect(() => {
    const tryInit = async () => {
      const token = localStorage.getItem("accessToken");

      if (token) {
        setAccessToken(token);
        setIsAuthenticated(true);
         const userId = decodeToken(token);

        try {
          const profile = await client.profileGET();
          setUser({
            id: userId ?? "",
            email: profile.email ?? "",
            userRole: profile.userRole ?? "User",
            subscriptionType: profile.subscriptionType ?? null,
            userName: profile.userName,
            avatarUrl: profile.avatarUrl,
          });
        } catch (error) {
          handleApiError(error);
          setAccessToken(null);
          setIsAuthenticated(false);
          setUser(null);
        }
      }

      setIsAuthReady(true);
    };

    tryInit();
  }, []);

  return (
    <AuthContext.Provider
      value={{
        accessToken,
        isAuthenticated,
        isAuthReady,
        user,
        setUser,
        login,
        logout,
      }}
    >
      {isAuthReady ? children : null}
    </AuthContext.Provider>
  );
}