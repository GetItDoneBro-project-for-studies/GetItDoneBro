import { AssignUserToProjectPayload, ProjectRole } from '@/api/projects/types'
import { User } from '@/api/users/types'
import { Button } from '@/components/ui/button'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import { Label } from '@/components/ui/label'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Loader2 } from 'lucide-react'
import { useEffect, useState } from 'react'

interface AssignUserDialogProps {
	open: boolean
	onOpenChange: (open: boolean) => void
	onSubmit: (data: AssignUserToProjectPayload) => void
	isSubmitting?: boolean
	availableUsers: User[]
	isLoadingUsers?: boolean
}

export function AssignUserDialog({
	open,
	onOpenChange,
	onSubmit,
	isSubmitting = false,
	availableUsers,
	isLoadingUsers = false,
}: AssignUserDialogProps) {
	const [selectedUserId, setSelectedUserId] = useState<string>('')
	const [selectedRole, setSelectedRole] = useState<ProjectRole>(
		ProjectRole.Member
	)
	const [error, setError] = useState<string>('')

	useEffect(() => {
		if (open) {
			setSelectedUserId('')
			setSelectedRole(ProjectRole.Member)
			setError('')
		}
	}, [open])

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()

		if (!selectedUserId) {
			setError('Please select a user')
			return
		}

		const selectedUser = availableUsers.find((u) => u.id === selectedUserId)
		if (!selectedUser) {
			setError('Invalid user selected')
			return
		}

		await onSubmit({
			keycloakId: selectedUser.id,
			role: selectedRole,
		})
	}

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent className="max-w-md">
				<DialogHeader>
					<DialogTitle className="font-heading text-2xl">
						Add Team Member
					</DialogTitle>
					<DialogDescription>
						Select a user to add to this project and assign their
						role.
					</DialogDescription>
				</DialogHeader>
				<form onSubmit={handleSubmit} className="space-y-6">
					<div className="space-y-2">
						<Label htmlFor="user">
							User <span className="text-destructive">*</span>
						</Label>
						{isLoadingUsers ? (
							<div className="flex items-center gap-2 py-2">
								<Loader2 className="h-4 w-4 animate-spin" />
								<span className="text-muted-foreground text-sm">
									Loading users...
								</span>
							</div>
						) : (
							<Select
								value={selectedUserId}
								onValueChange={(value) => {
									setSelectedUserId(value)
									setError('')
								}}
								disabled={isSubmitting}
							>
								<SelectTrigger
									id="user"
									className={
										error ? 'border-destructive' : ''
									}
								>
									<SelectValue placeholder="Select a user" />
								</SelectTrigger>
								<SelectContent>
									{availableUsers.length === 0 ? (
										<div className="text-muted-foreground p-2 text-sm">
											No available users
										</div>
									) : (
										availableUsers.map((user) => (
											<SelectItem
												key={user.id}
												value={user.id}
											>
												<div className="flex items-center gap-2">
													<span>
														{user.firstName &&
														user.lastName
															? `${user.firstName} ${user.lastName}`
															: user.username}
													</span>
													<span className="text-muted-foreground text-xs">
														({user.email})
													</span>
												</div>
											</SelectItem>
										))
									)}
								</SelectContent>
							</Select>
						)}
						{error && (
							<p className="text-destructive text-sm">{error}</p>
						)}
					</div>

					<div className="space-y-2">
						<Label htmlFor="role">Role</Label>
						<Select
							value={selectedRole.toString()}
							onValueChange={(value) =>
								setSelectedRole(Number(value) as ProjectRole)
							}
							disabled={isSubmitting}
						>
							<SelectTrigger id="role">
								<SelectValue />
							</SelectTrigger>
							<SelectContent>
								<SelectItem
									value={ProjectRole.Member.toString()}
								>
									Member
								</SelectItem>
								<SelectItem
									value={ProjectRole.Admin.toString()}
								>
									Admin
								</SelectItem>
							</SelectContent>
						</Select>
						<p className="text-muted-foreground text-xs">
							Admins can manage project settings and team members.
						</p>
					</div>

					<div className="flex justify-end gap-3 pt-4">
						<Button
							type="button"
							variant="outline"
							onClick={() => onOpenChange(false)}
							disabled={isSubmitting}
						>
							Cancel
						</Button>
						<Button
							type="submit"
							disabled={
								isSubmitting ||
								isLoadingUsers ||
								availableUsers.length === 0
							}
						>
							{isSubmitting ? (
								<>
									<Loader2 className="mr-2 h-4 w-4 animate-spin" />
									Adding...
								</>
							) : (
								'Add Member'
							)}
						</Button>
					</div>
				</form>
			</DialogContent>
		</Dialog>
	)
}
