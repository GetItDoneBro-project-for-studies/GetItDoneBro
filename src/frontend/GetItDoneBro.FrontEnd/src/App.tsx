import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { useEffect, useState } from 'react'
import { toast, Toaster } from 'sonner'
import { getProjects } from './api/users'
import { Project } from './api/users/types'
import LoaderSkeleton from './components/LoaderSkeleton'
import { ThemeToggle } from './components/ThemeToggle'
import { Button } from './components/ui/button'
import { ThemeProvider } from './contexts/ThemeContext'
import { useAuth } from './hooks/useAuth'
import { keycloakService } from './services/keycloakService'
function App() {
	const { getUserProfile } = useAuth()
	const [users, setUsers] = useState<Project[]>([])
	const [isLoadingUsers, setIsLoadingUsers] = useState(false)
	function showToast() {
		toast('Event has been created.')
	}
	function toastPromise(shouldReject: boolean) {
		return () => {
			toast.promise<{ name: string }>(
				() =>
					new Promise((resolve, reject) =>
						setTimeout(
							() =>
								shouldReject
									? reject(new Error('Error'))
									: resolve({ name: 'Event' }),
							2000
						)
					),
				{
					loading: 'Loading...',
					success: (data) => `${data.name} has been created`,
					error: 'Error',
				}
			)
		}
	}

	async function fetchUsersAsync() {
		setIsLoadingUsers(true)
		try {
			const users = await getProjects()
			// setUsers(users.data)
		} finally {
			setIsLoadingUsers(false)
		}
	}

	useEffect(() => {
		fetchUsersAsync()
	}, [])

	const userProfiles =
		users.length > 0 ? (
			users.map((user) => {
				return (
					<div key={user.id}>
						<p>
							<strong>{user.name}</strong> - {user.description}
						</p>
					</div>
				)
			})
		) : (
			<p>No users found.</p>
		)

	return (
		<ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
			<Card className="m-8">
				<CardHeader>
					<CardTitle>Działa, zalogowano jako:</CardTitle>
					<CardDescription>
						<span style={{ fontWeight: 600 }}>
							{getUserProfile().name}
						</span>
					</CardDescription>
				</CardHeader>
				<CardContent>
					{isLoadingUsers ? <LoaderSkeleton /> : userProfiles}
					<br />
					Lorem ipsum dolor sit amet, consectetur adipisicing elit.
					Ut, architecto, itaque aut repudiandae perspiciatis sed
					tenetur aliquid officiis, voluptates maxime dolor ipsa sit
					esse laboriosam et obcaecati amet cupiditate vitae? ---{' '}
					{keycloakService.getToken()}
				</CardContent>
				<CardFooter className="flex gap-4">
					<Button
						type="button"
						onClick={() => keycloakService.logoutAsync()}
					>
						logout
					</Button>
					<Button type="button" onClick={showToast}>
						toast
					</Button>
					<Button variant="destructive" onClick={toastPromise(true)}>
						Promise
					</Button>
					<Button onClick={() => fetchUsersAsync()}>
						fetchUsers
					</Button>
					<Button onClick={toastPromise(false)}>Promise</Button>
					<ThemeToggle />
				</CardFooter>
			</Card>

			<Toaster closeButton richColors />
		</ThemeProvider>
	)
}

export default App
