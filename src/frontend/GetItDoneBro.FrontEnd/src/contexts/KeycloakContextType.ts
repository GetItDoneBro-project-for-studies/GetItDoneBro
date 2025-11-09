import { createContext } from "react";
import { keycloakService } from "../services/keycloakService";

export interface KeycloakContextType {
  isAuthenticated: boolean;
  isInitialized: boolean;
  isLoading: boolean;
  error: Error | null;
  logout: () => Promise<void>;
  updatePassword: () => void;
  updateProfile: () => void;
  getUserProfile: () => ReturnType<typeof keycloakService.getUserProfile>;
  getUserRoles: () => string[];
  hasRole: (role: string) => boolean;
}

export const KeycloakContext = createContext<KeycloakContextType | undefined>(
  undefined
);
