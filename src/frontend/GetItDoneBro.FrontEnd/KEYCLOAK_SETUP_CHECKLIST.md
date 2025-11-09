# Checklist - Keycloak React Integration Setup

## ✅ Już Zrobione

- [x] `KeycloakService` (serwis z autoryzacją)
- [x] `KeycloakContext` (context API provider)
- [x] `KeycloakGuard` (komponent ochronny)
- [x] `useAuth` (custom hook)
- [x] `main.tsx` (zawinięty w KeycloakProvider)
- [x] Dokumentacja

## 📋 TODO - Konfiguracja

- [ ] **Zmienne Środowiskowe** - Utwórz `.env` w głównym katalogu projektu:

  ```env
  VITE_KEYCLOAK_URL=https://your-keycloak-instance.com
  VITE_KEYCLOAK_REALM=your-realm-name
  VITE_KEYCLOAK_CLIENT_ID=your-client-id
  ```

- [ ] **Silent Check SSO HTML** - Utwórz plik `public/silent-check-sso.html`:

  ```html
  <!DOCTYPE html>
  <html>
    <head>
      <title>Silent Check SSO</title>
    </head>
    <body>
      <script>
        parent.postMessage(location.hash, location.origin);
      </script>
    </body>
  </html>
  ```

- [ ] **App.tsx Update** - Zawiń twoją aplikację w `KeycloakGuard`:

  ```tsx
  import { KeycloakGuard } from "./components/KeycloakGuard";
  import { useAuth } from "./hooks/useAuth";

  function App() {
    return (
      <KeycloakGuard>
        <YourAppContent />
      </KeycloakGuard>
    );
  }
  ```

## 🧪 Testowanie

- [ ] Uruchom aplikację: `npm run dev`
- [ ] Sprawdź czy redirect do Keycloak logowania działa
- [ ] Zaloguj się na Keycloak
- [ ] Sprawdź czy aplikacja się renderuje po zalogowaniu
- [ ] Testuj `useAuth()` hook w konsoli:
  ```tsx
  // W componencie
  const auth = useAuth();
  console.log(auth.getUserProfile());
  console.log(auth.hasRole("admin"));
  ```

## 🔧 Opcjonalne - Ulepszenia

- [ ] Dodaj Loading Spinner w `KeycloakGuard` fallback
- [ ] Dodaj Error Boundary wokół aplikacji
- [ ] Integracja z axios dla automatycznego tokena w header'ach
- [ ] Protected Routes (withAuth HOC)
- [ ] Logout przy page refresh (jeśli token expired)

## 📝 Keycloak Server Setup

Upewnij się, że na Keycloak masz:

- [ ] Realm utworzony
- [ ] Client ID skonfigurowany z:
  - [ ] Access Type: `public`
  - [ ] Valid Redirect URIs:
    - `http://localhost:5173/*` (dev)
    - `https://your-domain.com/*` (production)
  - [ ] Web Origins:
    - `http://localhost:5173` (dev)
    - `https://your-domain.com` (production)
  - [ ] PKCE enabled (Project)

## 🚀 Production Deployment

- [ ] Zmień env variables na production values
- [ ] Włącz HTTPS
- [ ] Skonfiguruj CORS jeśli backend jest na innym originnie
- [ ] Ustaw odpowiednie timeout dla token refresh
- [ ] Ustaw appropriate `silentCheckSsoRedirectUri` dla Keycloak

## 🐛 Troubleshooting

### Aplikacja nie redirect'uje do logowania

- Sprawdź czy `VITE_KEYCLOAK_*` env vars są ustawione
- Sprawdź konsole przeglądarki na błędy
- Sprawdź czy Keycloak server jest dostępny

### Token nie refresh'uje się

- Sprawdź czy `setupTokenRefresh()` jest wywoływane
- Sprawdź czy `minValidity` w updateToken jest właściwy
- Sprawdź console.log w keycloakService

### Logout nie działa

- Sprawdź czy `clearInterval` jest wywoływane
- Sprawdź CORS ustawienia na Keycloak
- Sprawdź valid redirect URIs na Keycloak

### useAuth throws "must be used within KeycloakProvider"

- Sprawdź czy hook jest używany wewnątrz KeycloakGuard
- Sprawdź czy KeycloakProvider wraps cały App

## 📚 Przydatne Linki

- [Keycloak Docs](https://www.keycloak.org/documentation.html)
- [Keycloak JS Adapter](https://www.keycloak.org/docs/latest/securing_apps/#_javascript_adapter)
- [React Context API](https://react.dev/reference/react/useContext)
- [Custom Hooks](https://react.dev/learn/reusing-logic-with-custom-hooks)
