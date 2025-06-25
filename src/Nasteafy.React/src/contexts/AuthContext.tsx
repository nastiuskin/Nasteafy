import {
  useState,
  useEffect,
  createContext,
  type ReactNode,
} from "react";
import { decodeToken, type UserType } from "../helpers/decodeToken";
import { handleApiError } from "../helpers/handleApiError";
import { authService } from "../services/AuthService";
import { client, setAccessTokenHeader } from "../api/ApiClientProvider";
import { useNavigate } from "react-router-dom";

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

      setAccessToken(token);
      setIsAuthenticated(true);
      setAccessTokenHeader(token);

      const decoded = decodeToken(token);
      if (!decoded) return false;

      const profile = await client.profileGET();
      setUser({
        ...decoded,
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
      setAccessTokenHeader(null);
      setAccessToken(null);
      setIsAuthenticated(false);
      setUser(null);
      navigate("/");
    }
  };

  useEffect(() => {
    const tryRefresh = async () => {
      try {
        const token = await authService.refresh();
        if (token) {
          setAccessTokenHeader(token);
          setAccessToken(token);
          setIsAuthenticated(true);

          const decoded = decodeToken(token);
          if (decoded) {
            setUser(decoded);
          }
        } else {
          setAccessTokenHeader(null);
          setAccessToken(null);
          setIsAuthenticated(false);
          setUser(null);
          navigate("/login");
        }
      } catch {
        setAccessTokenHeader(null);
        setAccessToken(null);
        setIsAuthenticated(false);
        setUser(null);
        navigate("/login");
      } finally {
        setIsAuthReady(true);
      }
    };

    tryRefresh();
  }, [accessToken]);

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