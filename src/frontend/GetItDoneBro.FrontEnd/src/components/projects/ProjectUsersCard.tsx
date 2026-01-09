import { ProjectRole, ProjectUserDto, UserId } from '@/api/projects/types'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import {
	MoreVertical,
	Shield,
	ShieldCheck,
	UserMinus,
	Users,
} from 'lucide-react'

interface ProjectUsersCardProps {
	users: ProjectUserDto[]
	isLoading: boolean
	onAssignUser: () => void
	onUpdateRole: (userId: UserId, role: ProjectRole) => void
	onRemoveUser: (userId: UserId) => void
	disabled?: boolean
}

const roleLabels: Record<ProjectRole, string> = {
	[ProjectRole.Member]: 'Member',
	[ProjectRole.Admin]: 'Admin',
}

const roleColors: Record<ProjectRole, string> = {
	[ProjectRole.Member]:
		'border-blue-500/20 bg-blue-500/10 text-blue-600 dark:text-blue-400',
	[ProjectRole.Admin]:
		'border-purple-500/20 bg-purple-500/10 text-purple-600 dark:text-purple-400',
}

export function ProjectUsersCard({
	users,
	isLoading,
	onAssignUser,
	onUpdateRole,
	onRemoveUser,
	disabled = false,
}: ProjectUsersCardProps) {
	const handleRemoveUser = (userId: UserId, e: React.MouseEvent) => {
		e.stopPropagation()
		if (
			window.confirm(
				'Are you sure you want to remove this user from the project?'
			)
		) {
			onRemoveUser(userId)
		}
	}

	const handleRoleChange = (
		userId: UserId,
		newRole: ProjectRole,
		e: React.MouseEvent
	) => {
		e.stopPropagation()
		onUpdateRole(userId, newRole)
	}

	return (
		<Card className="border-2">
			<CardHeader>
				<div className="flex items-center justify-between">
					<CardTitle className="flex items-center gap-2 text-lg">
						<Users className="text-primary h-5 w-5" />
						Team Members
					</CardTitle>
					<Button
						size="sm"
						onClick={onAssignUser}
						disabled={disabled}
					>
						Add Member
					</Button>
				</div>
			</CardHeader>
			<CardContent>
				{isLoading ? (
					<div className="flex items-center justify-center py-8">
						<div className="border-primary h-6 w-6 animate-spin rounded-full border-2 border-t-transparent" />
					</div>
				) : users.length === 0 ? (
					<div className="py-8 text-center">
						<Users className="text-muted-foreground mx-auto mb-3 h-10 w-10" />
						<p className="text-muted-foreground text-sm">
							No team members assigned yet.
						</p>
						<Button
							variant="link"
							className="mt-2"
							onClick={onAssignUser}
							disabled={disabled}
						>
							Assign the first member
						</Button>
					</div>
				) : (
					<div className="space-y-3">
						{users.map((user) => (
							<div
								key={user.id}
								className="hover:bg-accent/50 flex items-center justify-between rounded-lg border p-3 transition-colors duration-100"
							>
								<div className="flex items-center gap-3">
									<div className="bg-primary text-primary-foreground font-heading flex h-9 w-9 items-center justify-center rounded-full text-xs font-semibold">
										{(user.firstName && user.lastName
											? `${user.firstName.charAt(0)}${user.lastName.charAt(0)}`
											: user.username.slice(0, 2)
										).toUpperCase()}
									</div>
									<div>
										<p className="font-heading text-sm font-medium">
											{user.firstName && user.lastName
												? `${user.firstName} ${user.lastName}`
												: user.username}
										</p>
										<p className="text-muted-foreground text-xs">
											{user.firstName &&
												user.lastName && (
													<>@{user.username} • </>
												)}
											Assigned{' '}
											{new Date(
												user.assignedAtUtc
											).toLocaleDateString('en-US', {
												month: 'short',
												day: 'numeric',
												year: 'numeric',
											})}
										</p>
									</div>
								</div>
								<div className="flex items-center gap-2">
									<Badge
										variant="outline"
										className={roleColors[user.role]}
									>
										{user.role === ProjectRole.Admin ? (
											<ShieldCheck className="mr-1 h-3 w-3" />
										) : (
											<Shield className="mr-1 h-3 w-3" />
										)}
										{roleLabels[user.role]}
									</Badge>
									<DropdownMenu>
										<DropdownMenuTrigger asChild>
											<Button
												variant="ghost"
												size="icon"
												className="h-8 w-8"
												disabled={disabled}
												onClick={(e) =>
													e.stopPropagation()
												}
											>
												<MoreVertical className="h-4 w-4" />
											</Button>
										</DropdownMenuTrigger>
										<DropdownMenuContent align="end">
											{user.role !==
												ProjectRole.Admin && (
												<DropdownMenuItem
													onClick={(e) =>
														handleRoleChange(
															user.id,
															ProjectRole.Admin,
															e as unknown as React.MouseEvent
														)
													}
												>
													<ShieldCheck className="mr-2 h-4 w-4" />
													Make Admin
												</DropdownMenuItem>
											)}
											{user.role !==
												ProjectRole.Member && (
												<DropdownMenuItem
													onClick={(e) =>
														handleRoleChange(
															user.id,
															ProjectRole.Member,
															e as unknown as React.MouseEvent
														)
													}
												>
													<Shield className="mr-2 h-4 w-4" />
													Make Member
												</DropdownMenuItem>
											)}
											<DropdownMenuSeparator />
											<DropdownMenuItem
												onClick={(e) =>
													handleRemoveUser(
														user.id,
														e as unknown as React.MouseEvent
													)
												}
												className="text-destructive focus:text-destructive"
											>
												<UserMinus className="mr-2 h-4 w-4" />
												Remove from Project
											</DropdownMenuItem>
										</DropdownMenuContent>
									</DropdownMenu>
								</div>
							</div>
						))}
					</div>
				)}
			</CardContent>
		</Card>
	)
}
