# Keycloak React Integration - Quick Start

## ✅ Status: Wszystkie błędy naprawione!

Integracja Keycloaka w React została w pełni zaimplementowana i gotowa do użycia.

## 📁 Struktura Plików

```
src/contexts/
├── KeycloakContext.tsx          # Provider komponent
├── KeycloakContextType.ts       # Context definition
└── useKeycloakContext.ts        # Hook do dostępu do contextu
├── KEYCLOAK_INTEGRATION.md      # Dokumentacja
└── FLOW_DIAGRAM.md              # Diagramy przepływu

src/components/
├── KeycloakGuard.tsx            # Komponent ochronny
└── AuthExamples.tsx             # Przykładowe komponenty

src/hooks/
└── useAuth.ts                   # Custom hook (alias dla useKeycloak)

src/main.tsx                      # Główny punkt wejścia (już zaktualizowany)
```

## 🚀 Kroki do Uruchomienia

### 1. Zmienne Środowiskowe

Utwórz plik `.env` w głównym katalogu projektu:

```env
VITE_KEYCLOAK_URL=https://your-keycloak-instance.com
VITE_KEYCLOAK_REALM=your-realm-name
VITE_KEYCLOAK_CLIENT_ID=your-client-id
```

### 2. Silent Check SSO (opcjonalnie)

Utwórz plik `public/silent-check-sso.html`:

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

### 3. Aktualizuj App.tsx (WAŻNE!)

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

export default App;
```

## 💻 Użycie w Komponentach

### Sprawdzenie Autoryzacji

```tsx
import { useAuth } from "./hooks/useAuth";

function MyComponent() {
  const { isAuthenticated, getUserProfile, hasRole } = useAuth();

  if (!isAuthenticated) {
    return <div>Please log in</div>;
  }

  const profile = getUserProfile();
  const isAdmin = hasRole("admin");

  return (
    <div>
      <h1>Welcome, {profile.name}</h1>
      {isAdmin && <AdminPanel />}
    </div>
  );
}
```

### Wylogowanie

```tsx
import { useAuth } from "./hooks/useAuth";

function LogoutButton() {
  const { logout } = useAuth();

  return <button onClick={() => logout()}>Logout</button>;
}
```

### Aktualizacja Profilu

```tsx
import { useAuth } from "./hooks/useAuth";

function ProfileSettings() {
  const { updateProfile, updatePassword } = useAuth();

  return (
    <div>
      <button onClick={updateProfile}>Update Profile</button>
      <button onClick={updatePassword}>Change Password</button>
    </div>
  );
}
```

## 🧪 Testowanie

1. Uruchom dev serwer:

   ```bash
   npm run dev
   ```

2. Otwórz aplikację na `http://localhost:5173`

3. Powinieneś być przekierowany do logowania Keycloaka

4. Po zalogowaniu aplikacja powinna się renderować

5. W konsoli możesz przetestować:
   ```typescript
   // Otwórz DevTools Console i wklej:
   // (jeśli masz dostęp do contextu)
   ```

## 📚 API Reference

### useAuth Hook

```typescript
const {
  // State
  isAuthenticated: boolean           // Czy user jest zalogowany
  isInitialized: boolean             // Czy Keycloak initialized
  isLoading: boolean                 // Czy trwa initialization
  error: Error | null                // Błąd jeśli jest

  // Methods
  logout: () => Promise<void>        // Wyloguj użytkownika
  updatePassword: () => void         // Przejdź do zmiany hasła
  updateProfile: () => void          // Przejdź do edycji profilu
  getUserProfile: () => UserProfile  // Pobierz profil (id, name, email)
  getUserRoles: () => string[]       // Pobierz role użytkownika
  hasRole: (role: string) => boolean // Sprawdź czy ma rolę
} = useAuth()
```

### KeycloakGuard Props

```typescript
<KeycloakGuard
  fallback={<LoadingSpinner />} // Optional: custom loading UI
>
  <YourContent />
</KeycloakGuard>
```

## 🔐 Security Features

- ✅ PKCE flow (S256) - secure token exchange
- ✅ Automatic token refresh (co 60 sekund)
- ✅ Silent SSO support
- ✅ Automatic logout on token refresh failure
- ✅ HTTP-only cookies recommended
- ✅ Type-safe React Context API

## 🐛 Troubleshooting

### Aplikacja nie redirect'uje do logowania

```
❌ Sprawdź:
1. Czy env vars są ustawione: VITE_KEYCLOAK_*
2. Czy Keycloak server jest dostępny
3. DevTools Console na błędy
```

### "useKeycloak must be used within KeycloakProvider"

```
❌ Sprawdź:
1. Czy komponent jest wewnątrz KeycloakGuard
2. Czy KeycloakProvider wraps App w main.tsx
```

### Token nie refresh'uje się

```
❌ Sprawdź:
1. Czy setupTokenRefresh() jest wywoływane
2. Console na errory
3. CORS ustawienia na Keycloak
```

## 📖 Dodatkowa Dokumentacja

- `src/contexts/KEYCLOAK_INTEGRATION.md` - Szczegółowa dokumentacja
- `src/contexts/FLOW_DIAGRAM.md` - Diagramy przepływu
- `KEYCLOAK_SETUP_CHECKLIST.md` - Pełny checklist setupu
- `IMPLEMENTATION_SUMMARY.md` - Podsumowanie implementacji

## 🎯 Następne Kroki

1. ✅ Setup .env variables
2. ✅ Setup Keycloak server
3. ✅ Zawiń App w KeycloakGuard
4. ✅ Test na localhost
5. ⏭️ Integruj useAuth() w swoich komponentach
6. ⏭️ Deploy na production

## 💡 Best Practices

- Zawsze sprawdzaj `isLoading` przed renderowaniem UI
- Używaj `hasRole()` dla role-based UI
- Obsługuj error state w KeycloakGuard
- Nie przechowuj tokena w localStorage (keycloak-js zarządza)
- Testuj logout flow w production

## 🔗 Przydatne Linki

- [Keycloak Documentation](https://www.keycloak.org/documentation.html)
- [React Hooks API](https://react.dev/reference/react/hooks)
- [Context API](https://react.dev/reference/react/useContext)

---

**Gotowe do użytku! 🎉**
