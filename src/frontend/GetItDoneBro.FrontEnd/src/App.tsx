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
import { getUsersAsync } from './api/users'
import { User } from './api/users/types'
import { KeycloakGuard } from './components/KeycloakGuard'
import LoaderSkeleton from './components/LoaderSkeleton'
import { ThemeToggle } from './components/ThemeToggle'
import { Button } from './components/ui/button'
import { ThemeProvider } from './contexts/ThemeContext'
import { useAuth } from './hooks/useAuth'
function App() {
	const { getUserProfile, logout } = useAuth()

	const [users, setUsers] = useState<User[]>([])
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

	useEffect(() => {
		const fetchUsers = async () => {
			try {
				setIsLoadingUsers(true)
				const users = await getUsersAsync()
				setUsers(users.data)
			} finally {
				setIsLoadingUsers(false)
			}
		}

		void fetchUsers()
	}, [])

	const userProfiles = users.map((user) => {
		return (
			<div key={user.id}>
				<p>
					<strong>{user.username}</strong> - {user.email}
				</p>
			</div>
		)
	})

	return (
		<ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
			<KeycloakGuard>
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
						Lorem ipsum dolor sit amet, consectetur adipisicing
						elit. Ut, architecto, itaque aut repudiandae
						perspiciatis sed tenetur aliquid officiis, voluptates
						maxime dolor ipsa sit esse laboriosam et obcaecati amet
						cupiditate vitae?
					</CardContent>
					<CardFooter className="flex gap-4">
						<Button type="button" onClick={logout}>
							logout
						</Button>
						<Button type="button" onClick={showToast}>
							toast
						</Button>
						<Button
							variant="destructive"
							onClick={toastPromise(true)}
						>
							Promise
						</Button>
						<Button onClick={toastPromise(false)}>Promise</Button>
						<ThemeToggle />
					</CardFooter>
				</Card>
			</KeycloakGuard>

			<Toaster closeButton richColors />
		</ThemeProvider>
	)
}

export default App
