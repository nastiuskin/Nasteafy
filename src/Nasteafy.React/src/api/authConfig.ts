import type { IConfig } from "./AuthorizedApiBase";

let currentToken: string | null = null;

export const updateAuthToken = (token: string | null) => {
  currentToken = token;
};

export const authConfig: IConfig = {
  getAuthorization: () => (currentToken ? `Bearer ${currentToken}` : ""),
};
