import { useKeycloak } from "../contexts/useKeycloakContext";

/**
 * Hook do dostępu do informacji autoryzacji i metod Keycloaka
 * @example
 * const { isAuthenticated, getUserProfile, hasRole } = useAuth()
 */
export const useAuth = () => {
  return useKeycloak();
};
