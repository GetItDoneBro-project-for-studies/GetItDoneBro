import Keycloak from 'keycloak-js'

interface KeycloakInitOptions {
	onLoad: 'check-sso' | 'login-required'
	silentCheckSsoRedirectUri?: string
	silentCheckSsoFallback?: boolean
	checkLoginIframe?: boolean
	enableLogging?: boolean
	pkceMethod?: 'S256'
	flow?: 'standard' | 'implicit' | 'hybrid'
}

export interface KeycloakError extends Error {
	error: string
	error_description: string
}

export const defaultInitOptions: KeycloakInitOptions = {
	onLoad: 'login-required',
	checkLoginIframe: false,
	pkceMethod: 'S256',
	enableLogging: false,
	flow: 'standard',
}

const keycloakConfig = {
	url: import.meta.env.VITE_KEYCLOAK_URL,
	realm: import.meta.env.VITE_KEYCLOAK_REALM,
	clientId: import.meta.env.VITE_KEYCLOAK_CLIENT_ID,
}

class KeycloakService {
	private keycloak: Keycloak
	private refreshInterval: number | null = null
	private initialized = false
	private initPromise: Promise<boolean> | null = null

	constructor() {
		this.keycloak = new Keycloak(keycloakConfig)
		this.setupKeycloakEvents()
	}

	private setupKeycloakEvents() {
		this.keycloak.onTokenExpired = async () => {
			await this.updateToken()
		}

		this.keycloak.onAuthRefreshError = async () => {
			console.error('Auth refresh error, logging out')
			await this.logoutAsync()
		}
	}
	async initialize(options: KeycloakInitOptions) {
		try {
			if (this.initialized) {
				return this.keycloak.authenticated ?? false
			}

			if (this.initPromise) {
				return this.initPromise
			}

			if (this.keycloak.authenticated) {
				this.initialized = true
				return true
			}

			this.initPromise = (async () => {
				const authenticated = await this.keycloak.init(options)
				this.initialized = true
				this.initPromise = null

				if (!authenticated) {
					console.warn('Not authenticated')
					await this.keycloak.login()
					return false
				}

				return authenticated
			})()
			return this.initPromise
		} catch (error) {
			console.error('Failed to initialize Keycloak:', error)
			this.initPromise = null
			throw error
		}
	}

	getToken() {
		return this.keycloak.token
	}

	getRefreshToken() {
		return this.keycloak.refreshToken
	}

	getUserProfile() {
		return {
			id: this.keycloak.subject!,
			name: this.keycloak.tokenParsed?.name,
			email: this.keycloak.tokenParsed?.email,
		}
	}

	isAuthenticated() {
		return !!this.keycloak.authenticated
	}

	isTokenExpired(minValidity = 0) {
		return this.keycloak.isTokenExpired(minValidity)
	}

	async updateToken(minValidity = 70) {
		if (!this.initialized) {
			console.warn('Cannot update token: Keycloak not initialized yet')
			return false
		}

		try {
			const refreshed = await this.keycloak.updateToken(minValidity)
			if (refreshed) {
				console.info('Token refreshed', minValidity, 'seconds')
			}
			return refreshed
		} catch (error) {
			console.error('Token update failed:', error)
			await this.logoutAsync()
			return false
		}
	}

	setupTokenRefresh() {
		if (this.refreshInterval) {
			clearInterval(this.refreshInterval)
		}

		this.refreshInterval = window.setInterval(() => {
			this.updateToken(90).catch((error) => {
				console.error('Failed to refresh token:', error)
			})
		}, 60000)
	}

	async logoutAsync(): Promise<void> {
		if (this.refreshInterval) {
			clearInterval(this.refreshInterval)
			this.refreshInterval = null
		}

		try {
			await this.keycloak.logout()
		} catch (error) {
			console.error('Logout failed:', error)
			throw error
		}
	}

	getUserRoles(): string[] {
		const roles = (this.keycloak.tokenParsed?.role as string[]) ?? []
		return roles.map((role) => role.toLowerCase())
	}

	hasRole(role: string): boolean {
		const userRoles = this.getUserRoles()
		return userRoles.includes(role.toLowerCase())
	}

	updatePassword() {
		return this.keycloak.login({
			action: 'UPDATE_PASSWORD',
		})
	}

	updateProfile() {
		return this.keycloak.login({
			action: 'UPDATE_PROFILE',
		})
	}
}

export const keycloakService = new KeycloakService()
