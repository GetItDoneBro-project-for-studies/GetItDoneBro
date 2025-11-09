import "./App.css";
import { KeycloakGuard } from "./components/KeycloakGuard";
import { useAuth } from "./hooks/useAuth";
function App() {
  const { getUserProfile } = useAuth();
  return (
    <KeycloakGuard>
      dziala zalogowano jako: {getUserProfile().name}
    </KeycloakGuard>
  );
}

export default App;
