import { User, UserId } from '@/api/users/types'
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
import { usePageTransition } from '@/hooks/usePageTransition'
import { motion } from 'framer-motion'
import { KeyRound, Mail, MoreVertical, Shield, UserX } from 'lucide-react'

interface UserCardProps {
	user: User
	onDisable: (id: UserId) => void
	onResetPassword: (id: UserId) => void
	index?: number
}

export function UserCard({
	user,
	onDisable,
	onResetPassword,
	index = 0,
}: UserCardProps) {
	const handleDisable = (e: React.MouseEvent) => {
		e.stopPropagation()
		if (
			window.confirm(
				`Are you sure you want to disable "${user.username}"?`
			)
		) {
			onDisable(user.id)
		}
	}

	const handleResetPassword = (e: React.MouseEvent) => {
		e.stopPropagation()
		if (
			window.confirm(
				`Are you sure you want to reset password for "${user.username}"?`
			)
		) {
			onResetPassword(user.id)
		}
	}

	const displayName =
		user.firstName && user.lastName
			? `${user.firstName} ${user.lastName}`
			: user.username

	const initials =
		user.firstName && user.lastName
			? `${user.firstName[0]}${user.lastName[0]}`
			: user.username.slice(0, 2).toUpperCase()

	const { isReducedMotion } = usePageTransition()
	const CardWrapper = isReducedMotion ? 'div' : motion.div

	return (
		<CardWrapper
			{...(isReducedMotion
				? {}
				: {
						initial: { opacity: 0, y: 20 },
						animate: { opacity: 1, y: 0 },
						transition: { delay: index * 0.05, duration: 0.2 },
					})}
		>
			<Card className="border-border hover:border-primary/50 cursor-pointer overflow-hidden border-2 transition-all duration-200 hover:shadow-lg">
				<CardHeader className="pb-3">
					<div className="flex items-start justify-between gap-2">
						<div className="flex items-center gap-3">
							<div className="bg-primary text-primary-foreground font-heading flex h-10 w-10 items-center justify-center rounded-full text-sm font-semibold">
								{initials}
							</div>
							<div>
								<CardTitle className="font-heading line-clamp-1 text-lg">
									{displayName}
								</CardTitle>
								<p className="text-muted-foreground text-xs">
									@{user.username}
								</p>
							</div>
						</div>
						<div className="flex items-center gap-2">
							<Badge
								variant="outline"
								className={
									user.enabled
										? 'border-green-500/20 bg-green-500/10 text-green-600 dark:text-green-400'
										: 'border-red-500/20 bg-red-500/10 text-red-600 dark:text-red-400'
								}
							>
								{user.enabled ? 'Active' : 'Disabled'}
							</Badge>
							<DropdownMenu>
								<DropdownMenuTrigger asChild>
									<Button
										variant="ghost"
										size="icon"
										className="h-8 w-8"
										onClick={(e) => e.stopPropagation()}
									>
										<MoreVertical className="h-4 w-4" />
									</Button>
								</DropdownMenuTrigger>
								<DropdownMenuContent align="end">
									<DropdownMenuItem
										onClick={handleResetPassword}
									>
										<KeyRound className="mr-2 h-4 w-4" />
										Reset Password
									</DropdownMenuItem>
									<DropdownMenuSeparator />
									{user.enabled && (
										<DropdownMenuItem
											onClick={handleDisable}
											className="text-destructive focus:text-destructive"
										>
											<UserX className="mr-2 h-4 w-4" />
											Disable User
										</DropdownMenuItem>
									)}
								</DropdownMenuContent>
							</DropdownMenu>
						</div>
					</div>
				</CardHeader>
				<CardContent className="space-y-3">
					{user.email && (
						<div className="flex items-center gap-2 text-sm">
							<Mail className="text-muted-foreground h-4 w-4" />
							<span className="text-muted-foreground">
								{user.email}
							</span>
							{user.emailVerified && (
								<Badge
									variant="outline"
									className="border-blue-500/20 bg-blue-500/10 text-xs text-blue-600 dark:text-blue-400"
								>
									Verified
								</Badge>
							)}
						</div>
					)}

					{user.realmRoles && user.realmRoles.length > 0 && (
						<div className="flex items-center gap-2">
							<Shield className="text-muted-foreground h-4 w-4" />
							<div className="flex flex-wrap gap-1">
								{user.realmRoles.map((role) => (
									<Badge
										key={role.name}
										variant="secondary"
										className="text-xs"
									>
										{role.name}
									</Badge>
								))}
							</div>
						</div>
					)}
				</CardContent>
			</Card>
		</CardWrapper>
	)
}
