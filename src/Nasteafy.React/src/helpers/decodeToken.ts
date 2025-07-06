export type UserType = {
  id: string;
  email: string;
  userRole: string | null;
  subscriptionType: string | null;
  userName?: string | null;
  avatarUrl?: string | null; 
};

export function decodeToken(token: string): UserType | null {
  try {
    const payload = token.split(".")[1];
    const decoded = JSON.parse(atob(payload));

    const role = typeof decoded.role === "string"
      ? decoded.role
      : Array.isArray(decoded.role)
        ? decoded.role[0]
        : null;

    return {
      id: decoded.sub, 
      email: decoded.email,
      userRole: role,
      subscriptionType: decoded.subscription ?? null
    };
  } catch (error) {
    console.error("Token decode error:", error);
    return null;
  }
}
