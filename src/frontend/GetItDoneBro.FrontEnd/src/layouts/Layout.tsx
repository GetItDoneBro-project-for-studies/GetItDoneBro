import { AppSidebar } from '@/components/app-sidebar/AppSidebar'
import { PageTransition } from '@/components/motion-primitives/PageTransition'
import { SidebarInset, SidebarProvider } from '@/components/ui/sidebar'
import { AnimatePresence } from 'framer-motion'
import { Outlet, useLocation } from 'react-router-dom'

export function Layout() {
	const location = useLocation()

	return (
		<SidebarProvider>
			<AppSidebar />
			<SidebarInset>
				<main className="flex-1 overflow-auto p-6">
					<AnimatePresence mode="wait" initial={false}>
						<PageTransition key={location.pathname}>
							<Outlet />
						</PageTransition>
					</AnimatePresence>
				</main>
			</SidebarInset>
		</SidebarProvider>
	)
}
