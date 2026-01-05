import { FadeIn } from '@/components/motion-primitives/FadeIn'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { useProjects } from '@/contexts/ProjectsContext'
import { Activity, Archive, CheckCircle2, FolderKanban } from 'lucide-react'
import { useNavigate } from 'react-router-dom'

export function Dashboard() {
	const { projects } = useProjects()
	const navigate = useNavigate()

	const stats = {
		total: projects.length,
		active: projects.filter((p) => p.status === 'active').length,
		completed: projects.filter((p) => p.status === 'completed').length,
		archived: projects.filter((p) => p.status === 'archived').length,
	}

	const statCards = [
		{
			title: 'Total Projects',
			value: stats.total,
			icon: FolderKanban,
			color: 'text-blue-600 dark:text-blue-400',
			bgColor: 'bg-blue-500/10',
		},
		{
			title: 'Active',
			value: stats.active,
			icon: Activity,
			color: 'text-primary',
			bgColor: 'bg-primary/10',
		},
		{
			title: 'Completed',
			value: stats.completed,
			icon: CheckCircle2,
			color: 'text-green-600 dark:text-green-400',
			bgColor: 'bg-green-500/10',
		},
		{
			title: 'Archived',
			value: stats.archived,
			icon: Archive,
			color: 'text-gray-600 dark:text-gray-400',
			bgColor: 'bg-gray-500/10',
		},
	]

	const recentProjects = projects.slice(0, 5)

	return (
		<div className="space-y-8">
			<FadeIn>
				<div>
					<h1 className="font-heading mb-2 text-3xl font-bold">
						Welcome back! 👋
					</h1>
					<p className="text-muted-foreground font-body">
						Here's an overview of your projects
					</p>
				</div>
			</FadeIn>

			<div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
				{statCards.map((stat, index) => (
					<FadeIn key={stat.title} delay={index * 0.1}>
						<Card className="border-2">
							<CardHeader className="flex flex-row items-center justify-between pb-2">
								<CardTitle className="text-muted-foreground text-sm font-medium">
									{stat.title}
								</CardTitle>
								<div
									className={`rounded-lg p-2 ${stat.bgColor}`}
								>
									<stat.icon
										className={`h-5 w-5 ${stat.color}`}
									/>
								</div>
							</CardHeader>
							<CardContent>
								<div className="font-heading text-3xl font-bold">
									{stat.value}
								</div>
							</CardContent>
						</Card>
					</FadeIn>
				))}
			</div>

			<FadeIn delay={0.4}>
				<Card className="border-2">
					<CardHeader>
						<div className="flex items-center justify-between">
							<div>
								<CardTitle className="font-heading text-xl">
									Recent Projects
								</CardTitle>
								<p className="text-muted-foreground mt-1 text-sm">
									Your latest project activity
								</p>
							</div>
							<Button onClick={() => navigate('/projects')}>
								View All
							</Button>
						</div>
					</CardHeader>
					<CardContent>
						{recentProjects.length === 0 ? (
							<div className="py-12 text-center">
								<p className="text-muted-foreground">
									No projects yet
								</p>
								<Button
									className="mt-4"
									onClick={() => navigate('/projects')}
								>
									Create Your First Project
								</Button>
							</div>
						) : (
							<div className="space-y-4">
								{recentProjects.map((project) => (
									<div
										key={project.id}
										className="hover:bg-accent/50 flex cursor-pointer items-center justify-between rounded-lg border p-4 transition-colors duration-100"
										onClick={() =>
											navigate(`/projects/${project.id}`)
										}
									>
										<div className="flex-1">
											<h3 className="font-heading font-semibold">
												{project.name}
											</h3>
											<p className="text-muted-foreground line-clamp-1 text-sm">
												{project.description}
											</p>
										</div>
										<div className="ml-4">
											<span
												className={`inline-flex items-center rounded-full px-3 py-1 text-xs font-medium ${
													project.status === 'active'
														? 'bg-blue-500/10 text-blue-600 dark:text-blue-400'
														: project.status ===
															  'completed'
															? 'bg-green-500/10 text-green-600 dark:text-green-400'
															: 'bg-gray-500/10 text-gray-600 dark:text-gray-400'
												}`}
											>
												{project.status}
											</span>
										</div>
									</div>
								))}
							</div>
						)}
					</CardContent>
				</Card>
			</FadeIn>
		</div>
	)
}
