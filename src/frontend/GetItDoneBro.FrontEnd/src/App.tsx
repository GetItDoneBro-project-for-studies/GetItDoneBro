import { KeycloakGuard } from "./components/KeycloakGuard";
import { useAuth } from "./hooks/useAuth";
function App() {
  const { getUserProfile, logout } = useAuth();
  return (
    <KeycloakGuard>
      <p>
        Działa, zalogowano jako:
        <span style={{ fontWeight: 600 }}> {getUserProfile().name}</span>
      </p>
      <button type="button" onClick={logout}>
        logout
      </button>
    </KeycloakGuard>
  );
}

export default App;
