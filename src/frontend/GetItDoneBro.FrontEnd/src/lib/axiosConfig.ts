import { handleDates } from '@/hooks/utils/handleDates'
import { keycloakService } from '@/services/keycloakService'
import axios from 'axios'

axios.interceptors.response.use((rep) => {
	handleDates(rep.data)
	return rep
})
axios.interceptors.request.use(
	async (config) => {
		try {
			if (keycloakService.isTokenExpired(60)) {
				await keycloakService.updateToken(60)
			}

			const token = keycloakService.getToken()
			if (token) {
				config.headers['Authorization'] = `Bearer ${token}`
			}

			return config
		} catch (error) {
			console.error('Failed to refresh token', error)
			await keycloakService.logoutAsync()
			return Promise.reject(error)
		}
	},
	(error) => Promise.reject(error)
)
