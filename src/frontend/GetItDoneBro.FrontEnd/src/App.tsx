import { KeycloakGuard } from "./components/KeycloakGuard";
import { useAuth } from "./hooks/useAuth";
function App() {
  const { getUserProfile } = useAuth();
  return (
    <KeycloakGuard>
      <p>
        Działa, zalogowano jako:
        <span style={{ fontWeight: 600 }}> {getUserProfile().name}</span>
      </p>
    </KeycloakGuard>
  );
}

export default App;
