import { useContext } from "react";
import { KeycloakContext } from "./KeycloakContextType";

/**
 * Hook to access Keycloak context
 * Must be used within KeycloakProvider
 */
export const useKeycloak = () => {
  const context = useContext(KeycloakContext);
  if (!context) {
    throw new Error("useKeycloak must be used within KeycloakProvider");
  }
  return context;
};
