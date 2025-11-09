import { useAuth } from "../hooks/useAuth";

/**
 * Example: User Profile Component
 * Shows how to access user information using useAuth hook
 */
export function UserProfile() {
  const { getUserProfile, isAuthenticated } = useAuth();

  if (!isAuthenticated) {
    return <div>Not authenticated</div>;
  }

  const profile = getUserProfile();

  return (
    <div className="p-4 border rounded">
      <h2 className="text-xl font-bold mb-2">User Profile</h2>
      <p>
        <strong>ID:</strong> {profile.id}
      </p>
      <p>
        <strong>Name:</strong> {profile.name}
      </p>
      <p>
        <strong>Email:</strong> {profile.email}
      </p>
    </div>
  );
}

/**
 * Example: Role-Based Component
 * Shows how to check user roles
 */
export function AdminPanel() {
  const { hasRole } = useAuth();

  if (!hasRole("admin")) {
    return <div>You don't have permission to access this panel</div>;
  }

  return (
    <div className="p-4 border rounded bg-blue-50">
      <h2 className="text-xl font-bold mb-2">Admin Panel</h2>
      <p>Welcome, Admin!</p>
      {/* Admin specific content */}
    </div>
  );
}

/**
 * Example: Logout Button
 * Shows how to logout user
 */
export function LogoutButton() {
  const { logout } = useAuth();

  const handleLogout = async () => {
    try {
      await logout();
    } catch (error) {
      console.error("Logout failed:", error);
    }
  };

  return (
    <button
      onClick={handleLogout}
      className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
    >
      Logout
    </button>
  );
}

/**
 * Example: Update Profile Button
 * Shows how to navigate to Keycloak profile update
 */
export function UpdateProfileButton() {
  const { updateProfile } = useAuth();

  return (
    <button
      onClick={updateProfile}
      className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
    >
      Update Profile
    </button>
  );
}
