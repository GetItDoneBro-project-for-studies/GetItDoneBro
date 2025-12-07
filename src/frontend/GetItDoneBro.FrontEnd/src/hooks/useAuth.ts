import { useKeycloak } from '../contexts/useKeycloakContext'

export const useAuth = () => {
	return useKeycloak()
}
