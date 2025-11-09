import { type ReactNode } from "react";
import { useKeycloak } from "../contexts/useKeycloakContext";

interface KeycloakGuardProps {
  children: ReactNode;
  fallback?: ReactNode;
}

export const KeycloakGuard = ({
  children,
  fallback = (
    <div className="flex items-center justify-center min-h-screen">
      Loading...
    </div>
  ),
}: KeycloakGuardProps) => {
  const { isInitialized, isLoading, error, isAuthenticated } = useKeycloak();

  if (isLoading) {
    return <>{fallback}</>;
  }

  if (error) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-red-600">
          <h1 className="text-2xl font-bold mb-4">Authentication Error</h1>
          <p>{error.message}</p>
        </div>
      </div>
    );
  }

  if (!isInitialized || !isAuthenticated) {
    return <>{fallback}</>;
  }

  return <>{children}</>;
};
