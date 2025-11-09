# Keycloak Integration for React

## Architektura

Integracja Keycloaka w React jest zbudowana na trzech głównych komponentach:

### 1. **KeycloakProvider** (`contexts/KeycloakContext.tsx`)

- Context provider, który zarządza stanem Keycloaka
- Inicjalizuje usługę Keycloaka przy montażu aplikacji
- Udostępnia stan autoryzacji dla wszystkich komponentów

### 2. **KeycloakGuard** (`components/KeycloakGuard.tsx`)

- Komponent ochronny, który renderuje fallback do czasu zalogowania
- Blokuje dostęp do chronionej zawartości
- Wyświetla błędy autoryzacji

### 3. **useAuth Hook** (`hooks/useAuth.ts`)

- Custom hook do dostępu do stanu autoryzacji
- Alias dla `useKeycloak()`
- Ułatwia korzystanie w komponentach

## Użycie

### Podstawowa konfiguracja (już zrobiona)

```tsx
// main.tsx
<KeycloakProvider>
  <App />
</KeycloakProvider>
```

### Ochrona zawartości

```tsx
import { KeycloakGuard } from "./components/KeycloakGuard";

export function App() {
  return (
    <KeycloakGuard>
      <Dashboard />
    </KeycloakGuard>
  );
}
```

### Dostęp do informacji o użytkowniku

```tsx
import { useAuth } from "./hooks/useAuth";

export function Profile() {
  const { getUserProfile, getUserRoles, hasRole } = useAuth();

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

export function LogoutButton() {
  const { logout } = useAuth();

  return <button onClick={() => logout()}>Logout</button>;
}
```

## API

### useAuth Hook

- `isAuthenticated: boolean` - czy użytkownik jest zalogowany
- `isInitialized: boolean` - czy Keycloak jest zainicjalizowany
- `isLoading: boolean` - czy trwa ładowanie
- `error: Error | null` - błąd inicjalizacji
- `logout(): Promise<void>` - wylogowanie użytkownika
- `updatePassword(): void` - przejście do zmiany hasła
- `updateProfile(): void` - przejście do edycji profilu
- `getUserProfile()` - profil użytkownika (id, name, email)
- `getUserRoles(): string[]` - role użytkownika
- `hasRole(role: string): boolean` - sprawdzenie czy użytkownik ma rolę

## Zmienne środowiskowe

Upewnij się, że w `.env` masz skonfigurowane:

```
VITE_KEYCLOAK_URL=https://your-keycloak-server.com
VITE_KEYCLOAK_REALM=your-realm
VITE_KEYCLOAK_CLIENT_ID=your-client-id
```

## Flow autoryzacji

1. **main.tsx** → renderuje `KeycloakProvider`
2. **KeycloakProvider** → inicjalizuje Keycloak w `useEffect`
3. **Keycloak inicjalizacja** → sprawdza czy użytkownik jest zalogowany
4. Jeśli nie zalogowany → przekierowuje do logowania
5. Po zalogowaniu → ustawia token refresh interval
6. **Aplikacja renderuje się** z dostępem do autoryzacji

## Best Practices

- Zawsze używaj `KeycloakGuard` do ochrony wrażliwych stron
- Używaj `useAuth` do sprawdzenia roli przed renderowaniem uprzywilejowanej zawartości
- Obsługuj `isLoading` staat w UI (loading spinner)
- Token refresh jest automatycznie ustawiany na 60 sekund

## Różnice od Vue implementacji

- W React używamy Context API zamiast global properties
- Inicjalizacja jest asynchroniczna w `useEffect` zamiast przed `mount()`
- Komponent `KeycloakGuard` pełni rolę strażnika zamiast route guards
- Użycie custom hook `useAuth` zamiast inject/provide
