import { useState, useCallback, useEffect, type ReactNode } from 'react'
import { keycloakService } from '../services/keycloakService'
import {
	KeycloakContext,
	type KeycloakContextType,
} from './KeycloakContextType'

interface KeycloakProviderProps {
	children: ReactNode
}

export const KeycloakProvider = ({ children }: KeycloakProviderProps) => {
	const [isInitialized, setIsInitialized] = useState(false)
	const [isLoading, setIsLoading] = useState(true)
	const [error, setError] = useState<Error | null>(null)

	useEffect(() => {
		const initializeKeycloak = async () => {
			try {
				setIsLoading(true)
				const { defaultInitOptions } = await import(
					'../services/keycloakService'
				)
				if (!(await keycloakService.initialize(defaultInitOptions))) {
					throw new Error('Keycloak initialization failed')
				}
				keycloakService.setupTokenRefresh()
				setIsInitialized(true)
				setError(null)
			} catch (err) {
				const error =
					err instanceof Error ? err : new Error(String(err))
				console.error('Failed to initialize Keycloak:', error)
				setError(error)
			} finally {
				setIsLoading(false)
			}
		}
		initializeKeycloak()
	}, [])

	const logout = useCallback(async () => {
		try {
			await keycloakService.logoutAsync()
		} catch (err) {
			const error = err instanceof Error ? err : new Error(String(err))
			setError(error)
			throw error
		}
	}, [])

	const contextValue: KeycloakContextType = {
		isAuthenticated: keycloakService.isAuthenticated(),
		isInitialized,
		isLoading,
		error,
		logout,
		updatePassword: () => keycloakService.updatePassword(),
		updateProfile: () => keycloakService.updateProfile(),
		getUserProfile: () => keycloakService.getUserProfile(),
		getUserRoles: () => keycloakService.getUserRoles(),
		hasRole: (role: string) => keycloakService.hasRole(role),
	}
	return (
		<KeycloakContext.Provider value={contextValue}>
			{children}
		</KeycloakContext.Provider>
	)
}
