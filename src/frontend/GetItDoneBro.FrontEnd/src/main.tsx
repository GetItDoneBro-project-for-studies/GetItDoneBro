import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App";
import { KeycloakProvider } from "./contexts/KeycloakContext";
import "./index.css";

// test
createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <KeycloakProvider>
      <App />
    </KeycloakProvider>
  </StrictMode>
);
