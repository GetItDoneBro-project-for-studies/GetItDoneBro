import { keycloakService } from '../services/keycloakService'

export const useAuth = () => {
	return {
		isAuthenticated: keycloakService.isAuthenticated(),
		error: null,
		logout: () => keycloakService.logoutAsync(),
		updatePassword: () => keycloakService.updatePassword(),
		updateProfile: () => keycloakService.updateProfile(),
		getUserProfile: () => keycloakService.getUserProfile(),
		getUserRoles: () => keycloakService.getUserRoles(),
		hasRole: (role: string) => keycloakService.hasRole(role),
	}
}
