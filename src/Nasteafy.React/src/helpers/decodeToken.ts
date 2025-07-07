export function decodeToken(token: string): string | null {
  try {
    const payload = token.split(".")[1];
    const decoded = JSON.parse(atob(payload));

    return decoded.sub ?? null; 
  } catch (error) {
    console.error("Token decode error:", error);
    return null;
  }
}