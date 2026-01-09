import { User, UserId } from '@/api/users/types'
import { FadeIn } from '@/components/motion-primitives/FadeIn'
import { Users } from 'lucide-react'
import { UserCard } from './UserCard'

interface UserListProps {
	users: User[]
	onDisable: (id: UserId) => void
	onResetPassword: (id: UserId) => void
}

export function UserList({ users, onDisable, onResetPassword }: UserListProps) {
	if (users.length === 0) {
		return (
			<FadeIn>
				<div className="flex flex-col items-center justify-center py-20 text-center">
					<div className="bg-muted mb-4 flex h-20 w-20 items-center justify-center rounded-full">
						<Users className="text-muted-foreground h-10 w-10" />
					</div>
					<h3 className="font-heading mb-2 text-xl font-semibold">
						No users yet
					</h3>
					<p className="text-muted-foreground max-w-sm">
						Get started by adding your first user. Click the "Add
						User" button above.
					</p>
				</div>
			</FadeIn>
		)
	}

	return (
		<div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
			{users.map((user, index) => (
				<UserCard
					key={user.id}
					user={user}
					onDisable={onDisable}
					onResetPassword={onResetPassword}
					index={index}
				/>
			))}
		</div>
	)
}
