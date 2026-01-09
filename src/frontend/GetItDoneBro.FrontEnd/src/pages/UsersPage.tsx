import { AddUserPayload, UserId } from '@/api/users/types'
import LoaderSkeleton from '@/components/LoaderSkeleton'
import { FadeIn } from '@/components/motion-primitives/FadeIn'
import { Button } from '@/components/ui/button'
import { UserDialog } from '@/components/users/UserDialog'
import { UserList } from '@/components/users/UserList'
import { useUsers } from '@/contexts/UsersContext'
import { UserPlus } from 'lucide-react'
import { useEffect, useState } from 'react'

export function UsersPage() {
	const {
		users,
		fetchUsers,
		addUser,
		disableUser,
		resetPassword,
		isOperating,
		isLoading,
	} = useUsers()
	const [isDialogOpen, setIsDialogOpen] = useState(false)

	useEffect(() => {
		fetchUsers()
	}, [fetchUsers])

	const handleAddUser = () => {
		setIsDialogOpen(true)
	}

	const handleSubmit = async (data: AddUserPayload) => {
		try {
			await addUser(data)
			setIsDialogOpen(false)
		} catch (error) {
			console.error('Operation failed:', error)
		}
	}

	const handleDisable = async (id: UserId) => {
		try {
			await disableUser(id)
		} catch (error) {
			console.error('Disable failed:', error)
		}
	}

	const handleResetPassword = async (id: UserId) => {
		try {
			await resetPassword(id)
		} catch (error) {
			console.error('Reset password failed:', error)
		}
	}

	if (isLoading) {
		return <LoaderSkeleton />
	}

	return (
		<div className="space-y-6">
			<FadeIn>
				<div className="flex items-center justify-between">
					<div>
						<h1 className="font-heading mb-2 text-3xl font-bold">
							Users
						</h1>
						<p className="text-muted-foreground font-body">
							Manage user accounts and permissions
						</p>
					</div>
					<Button
						onClick={handleAddUser}
						size="lg"
						className="bg-cta hover:bg-cta/90 gap-2"
						disabled={isOperating}
					>
						<UserPlus className="h-5 w-5" />
						Add User
					</Button>
				</div>
			</FadeIn>

			<UserList
				users={users}
				onDisable={handleDisable}
				onResetPassword={handleResetPassword}
			/>

			<UserDialog
				open={isDialogOpen}
				onOpenChange={setIsDialogOpen}
				onSubmit={handleSubmit}
				isSubmitting={isOperating}
			/>
		</div>
	)
}
