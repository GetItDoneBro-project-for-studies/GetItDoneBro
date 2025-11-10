import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
	CardHeader,
	CardTitle,
} from '@/components/ui/card'
import { useEffect, useState } from 'react'
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

		fetchUsers()
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
						<ThemeToggle />
					</CardFooter>
				</Card>
			</KeycloakGuard>
		</ThemeProvider>
	)
}

export default App
