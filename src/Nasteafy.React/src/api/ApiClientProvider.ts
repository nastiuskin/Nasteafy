import { Client } from "./apiClient";

let token: string | null = null;

export const setAccessTokenHeader = (newToken: string | null) => {
  token = newToken;
};

const createAuthenticatedFetch = (): typeof fetch => async (input, init) => {
  const headers = new Headers(init?.headers);
  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }

  return fetch(input, {
    ...init,
    headers,
    credentials: "include",
  });
};

export const client = new Client("https://localhost:7041", {
  fetch: createAuthenticatedFetch(),
});
