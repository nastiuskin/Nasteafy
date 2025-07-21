import { handleApiError } from "../../helpers/handleApiError";
import { client } from "../../api/ApiClientProvider"
import { LoginRequest, RegisterRequest } from "../../api/apiClient";
import { useNavigate } from "react-router-dom";

export const authService = {
  login: async (email: string, password: string): Promise<string | null> => {
    try {
      const command = new LoginRequest({ email, password });
      const token = await client.login(command);
      return token;
    } catch (error) {
      handleApiError(error);
      return null;
    }
  },

  register: async (email: string, password: string): Promise<void> => {
    try {
      const command = new RegisterRequest({ email, password });
      await client.register(command);
    } catch (error) {
      handleApiError(error);
    }
  },

  refresh: async (): Promise<string | null> => {
    try {
      const token = await client.refresh();
      return token;
    } catch (error) {
      return null;
    }
  },

  logout: async (): Promise<void> => {
    try {
      await client.logout();       
    } catch (error) {
      handleApiError(error);
    }
  },
};
