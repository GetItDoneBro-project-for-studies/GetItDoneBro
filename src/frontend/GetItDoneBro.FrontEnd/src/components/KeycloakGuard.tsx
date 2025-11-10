import { type ReactNode } from 'react'
import { useKeycloak } from '../contexts/useKeycloakContext'
import LoaderSkeleton from './LoaderSkeleton'

interface KeycloakGuardProps {
	children: ReactNode
	fallback?: ReactNode
}

export const KeycloakGuard = ({
	children,
	fallback = (
		<div className="flex min-h-screen items-center justify-center">
			<LoaderSkeleton />
		</div>
	),
}: KeycloakGuardProps) => {
	const { isInitialized, isLoading, error, isAuthenticated } = useKeycloak()

	if (isLoading) {
		return <>{fallback}</>
	}

	if (error) {
		return (
			<div className="flex min-h-screen items-center justify-center">
				<div className="text-red-600">
					<h1 className="mb-4 text-2xl font-bold">
						Authentication Error
					</h1>
					<p>{error.message}</p>
				</div>
			</div>
		)
	}

	if (!isInitialized || !isAuthenticated) {
		return <>{fallback}</>
	}

	return <>{children}</>
}
