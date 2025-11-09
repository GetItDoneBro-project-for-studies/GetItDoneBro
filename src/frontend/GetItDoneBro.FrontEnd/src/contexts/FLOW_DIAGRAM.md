# Keycloak React Integration - Flow Diagramu

## Inicjalizacja Aplikacji

```
main.tsx
    ↓
createRoot() renders KeycloakProvider
    ↓
KeycloakProvider (useEffect)
    ↓
keycloakService.initialize(options)
    ↓
    ├─→ Jeśli NOT authenticated → Keycloak redirect to login
    │
    └─→ Jeśli authenticated
         ↓
    keycloakService.setupTokenRefresh()
         ↓
    setIsInitialized(true)
         ↓
    Context updated
         ↓
    App + KeycloakGuard renderuje się
```

## Runtime - Token Refresh

```
setupTokenRefresh() ustawia interval (60 sekund)
    ↓
Każde 60 sekund:
    ↓
updateToken(90 seconds minValidity)
    ↓
    ├─→ Token valid → silent refresh
    │
    └─→ Token invalid → logoutAsync() + redirect to login
```

## Component Rendering

```
main.tsx
    ↓
KeycloakProvider (inicjalizacja)
    ↓
App
    ↓
KeycloakGuard (sprawdza isInitialized && isAuthenticated)
    ↓
    ├─→ isLoading → render fallback
    ├─→ error → render error message
    └─→ authenticated → render children
         ↓
    AppContent (twoja aplikacja)
         ↓
    useAuth() hook accessible w komponencie
```

## Komponent + Keycloak Interaction

```
MyComponent
    ↓
useAuth() → pobiera z KeycloakContext
    ↓
    ├─→ isAuthenticated
    ├─→ getUserProfile()
    ├─→ hasRole('admin')
    ├─→ logout()
    └─→ updateProfile()
```

## Stan w Context

```
KeycloakContext State:
├─ isAuthenticated: boolean (z keycloakService.isAuthenticated())
├─ isInitialized: boolean (po init)
├─ isLoading: boolean (podczas init)
├─ error: Error | null (jeśli init failed)
├─ logout: () => Promise<void>
├─ updatePassword: () => void
├─ updateProfile: () => void
├─ getUserProfile: () => UserProfile
├─ getUserRoles: () => string[]
└─ hasRole: (role: string) => boolean
```

## Error Scenarios

```
1. Keycloak Server Down
   └─ error state w Context
   └─ KeycloakGuard wyświetla error message

2. Invalid Config
   └─ console.error z komunikatem
   └─ error state w Context

3. Token Refresh Failed
   └─ keycloakService.logoutAsync()
   └─ User redirected to login

4. Logout Failed
   └─ console.error z komunikatem
   └─ throw error (catch w komponencie)
```

## Ścieżka z Użytkownika - Normalna

```
User
    ↓
Otwiera aplikację
    ↓
main.tsx renderuje KeycloakProvider
    ↓
KeycloakProvider.useEffect() fires
    ↓
initialize() checks Keycloak state
    ↓
    └─→ NOT authenticated
         ↓
        Keycloak redirects to login page
         ↓
        User logs in on Keycloak
         ↓
        Keycloak redirects back to app
         ↓
        Token stored
         ↓
        App renders successfully
         ↓
    User sees: AppContent
```

## Ścieżka z Użytkownika - Admin Page

```
User (logged in)
    ↓
Navigates to /admin
    ↓
AdminPanel component
    ↓
useAuth() → hasRole('admin')
    ↓
    ├─→ User has 'admin' role
    │   └─ Renders admin content
    │
    └─→ User doesn't have 'admin' role
        └─ Renders "No permission" message
```

## Cleanup & Logout

```
User clicks Logout
    ↓
logout() called from useAuth
    ↓
keycloakService.logoutAsync()
    ↓
clearInterval(refreshInterval)
    ↓
keycloak.logout() (clears tokens)
    ↓
Redirects to Keycloak logout
    ↓
User redirected back to app
    ↓
isAuthenticated = false
    ↓
KeycloakGuard shows fallback
    ↓
User sees login redirect again
```
