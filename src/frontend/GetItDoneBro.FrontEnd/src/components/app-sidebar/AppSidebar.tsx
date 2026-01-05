import { ThemeToggle } from '@/components/ThemeToggle'
import {
	Sidebar,
	SidebarContent,
	SidebarFooter,
	SidebarGroup,
	SidebarGroupContent,
	SidebarGroupLabel,
	SidebarHeader,
	SidebarMenu,
	SidebarMenuButton,
	SidebarMenuItem,
} from '@/components/ui/sidebar'
import { useAuth } from '@/hooks/useAuth'
import { keycloakService } from '@/services/keycloakService'
import { FolderKanban, Home, LogOut } from 'lucide-react'
import { NavLink } from 'react-router-dom'

const navigation = [
	{
		name: 'Dashboard',
		href: '/',
		icon: Home,
	},
	{
		name: 'Projects',
		href: '/projects',
		icon: FolderKanban,
	},
]

export function AppSidebar() {
	const { getUserProfile } = useAuth()
	const userProfile = getUserProfile()

	const handleLogoutAsync = async () => {
		await keycloakService.logoutAsync()
	}

	return (
		<Sidebar>
			<SidebarHeader className="border-sidebar-border border-b p-4">
				<div className="flex items-center gap-3">
					<div className="bg-primary text-primary-foreground flex h-10 w-10 items-center justify-center rounded-lg">
						<FolderKanban className="h-5 w-5" />
					</div>
					<div>
						<h2 className="font-heading text-lg font-semibold">
							GetItDoneBro
						</h2>
						<p className="text-muted-foreground text-xs">
							Project Manager
						</p>
					</div>
				</div>
			</SidebarHeader>

			<SidebarContent>
				<SidebarGroup>
					<SidebarGroupLabel>Navigation</SidebarGroupLabel>
					<SidebarGroupContent>
						<SidebarMenu>
							{navigation.map((item) => (
								<SidebarMenuItem key={item.name}>
									<SidebarMenuButton asChild>
										<NavLink
											to={item.href}
											className={({ isActive }) =>
												`flex items-center gap-3 rounded-lg px-3 py-2 transition-all duration-100 ${
													isActive
														? 'bg-primary text-primary-foreground'
														: 'hover:bg-sidebar-accent'
												}`
											}
										>
											<item.icon className="h-5 w-5" />
											<span className="font-body">
												{item.name}
											</span>
										</NavLink>
									</SidebarMenuButton>
								</SidebarMenuItem>
							))}
						</SidebarMenu>
					</SidebarGroupContent>
				</SidebarGroup>
			</SidebarContent>

			<SidebarFooter className="border-sidebar-border space-y-3 border-t p-4">
				<div className="flex items-center justify-between px-2">
					<span className="text-muted-foreground text-xs font-medium">
						Theme
					</span>
					<ThemeToggle />
				</div>
				<div className="bg-sidebar-accent mb-2 flex items-center gap-3 rounded-lg p-3">
					<div className="bg-primary text-primary-foreground font-heading flex h-10 w-10 items-center justify-center rounded-full font-semibold">
						{userProfile?.name?.[0] || 'U'}
					</div>
					<div className="flex-1 overflow-hidden">
						<p className="font-heading truncate text-sm font-medium">
							{userProfile?.name || 'User'}
						</p>
						<p className="text-muted-foreground truncate text-xs">
							{userProfile?.email || 'user@example.com'}
						</p>
					</div>
				</div>
				<button
					type="button"
					onClick={async () => await handleLogoutAsync()}
					className="hover:bg-destructive/10 hover:text-destructive flex w-full cursor-pointer items-center gap-2 rounded-lg px-3 py-2 text-sm transition-colors duration-100"
				>
					<LogOut className="h-4 w-4" />
					<span>Logout</span>
				</button>
			</SidebarFooter>
		</Sidebar>
	)
}
