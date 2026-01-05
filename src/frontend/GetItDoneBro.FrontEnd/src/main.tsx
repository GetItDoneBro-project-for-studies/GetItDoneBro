import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App'
import './index.css'
import './lib/axiosConfig'
import { defaultInitOptions, keycloakService } from './services/keycloakService'

const initApp = async () => {
	if (!(await keycloakService.initialize(defaultInitOptions))) {
		console.error('Keycloak initialization failed')
	}
	keycloakService.setupTokenRefresh()
	createRoot(document.getElementById('root')!).render(
		<StrictMode>
		<App />
		 </StrictMode>
	)
}

await initApp()
