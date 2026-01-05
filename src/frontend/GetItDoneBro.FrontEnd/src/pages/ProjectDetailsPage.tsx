import { FadeIn } from '@/components/motion-primitives/FadeIn'
import { ProjectDialog } from '@/components/projects/ProjectDialog'
import { Badge } from '@/components/ui/badge'
import {
	Breadcrumb,
	BreadcrumbItem,
	BreadcrumbLink,
	BreadcrumbList,
	BreadcrumbPage,
	BreadcrumbSeparator,
} from '@/components/ui/breadcrumb'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { useProjects } from '@/contexts/ProjectsContext'
import { CreateProjectInput } from '@/types/project'
import { ArrowLeft, Calendar, Clock, Pencil, Trash2 } from 'lucide-react'
import { useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { toast } from 'sonner'

const statusColors = {
	active: 'bg-blue-500/10 text-blue-600 dark:text-blue-400 border-blue-500/20',
	completed:
		'bg-green-500/10 text-green-600 dark:text-green-400 border-green-500/20',
	archived:
		'bg-gray-500/10 text-gray-600 dark:text-gray-400 border-gray-500/20',
}

export function ProjectDetailsPage() {
	const { id } = useParams<{ id: string }>()
	const navigate = useNavigate()
	const { getProjectById, updateProject, deleteProject } = useProjects()
	const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)

	const project = id ? getProjectById(id) : undefined

	if (!project) {
		return (
			<div className="flex flex-col items-center justify-center py-20">
				<h2 className="font-heading mb-4 text-2xl font-bold">
					Project not found
				</h2>
				<Button onClick={() => navigate('/projects')}>
					<ArrowLeft className="mr-2 h-4 w-4" />
					Back to Projects
				</Button>
			</div>
		)
	}

	const handleEdit = () => {
		setIsEditDialogOpen(true)
	}

	const handleDeleteAsync = async () => {
		if (
			window.confirm(`Are you sure you want to delete "${project.name}"?`)
		) {
			deleteProject(project.id)
			toast.success('Project deleted successfully!')
			await navigate('/projects')
		}
	}

	const handleUpdate = (data: CreateProjectInput) => {
		updateProject(project.id, data)
		toast.success('Project updated successfully!')
	}

	const daysSinceCreation = Math.floor(
		(new Date().getTime() - new Date(project.createdAt).getTime()) /
			(1000 * 60 * 60 * 24)
	)

	return (
		<div className="max-w-4xl space-y-6">
			<FadeIn>
				<Breadcrumb>
					<BreadcrumbList>
						<BreadcrumbItem>
							<BreadcrumbLink
								onClick={() => navigate('/')}
								className="cursor-pointer"
							>
								Dashboard
							</BreadcrumbLink>
						</BreadcrumbItem>
						<BreadcrumbSeparator />
						<BreadcrumbItem>
							<BreadcrumbLink
								onClick={() => navigate('/projects')}
								className="cursor-pointer"
							>
								Projects
							</BreadcrumbLink>
						</BreadcrumbItem>
						<BreadcrumbSeparator />
						<BreadcrumbItem>
							<BreadcrumbPage>{project.name}</BreadcrumbPage>
						</BreadcrumbItem>
					</BreadcrumbList>
				</Breadcrumb>
			</FadeIn>

			<FadeIn delay={0.1}>
				<div className="flex items-start justify-between">
					<div className="flex-1">
						<div className="mb-2 flex items-center gap-3">
							<h1 className="font-heading text-3xl font-bold">
								{project.name}
							</h1>
							<Badge
								variant="outline"
								className={statusColors[project.status]}
							>
								{project.status}
							</Badge>
						</div>
						<p className="text-muted-foreground font-body">
							{project.description}
						</p>
					</div>
					<div className="ml-4 flex gap-2">
						<Button variant="outline" onClick={handleEdit}>
							<Pencil className="mr-2 h-4 w-4" />
							Edit
						</Button>
						<Button
							variant="outline"
							onClick={handleDeleteAsync}
							className="text-destructive hover:bg-destructive/10"
						>
							<Trash2 className="mr-2 h-4 w-4" />
							Delete
						</Button>
					</div>
				</div>
			</FadeIn>

			<div className="grid gap-6 md:grid-cols-2">
				<FadeIn delay={0.2}>
					<Card className="border-2">
						<CardHeader>
							<CardTitle className="flex items-center gap-2 text-lg">
								<Calendar className="text-primary h-5 w-5" />
								Created Date
							</CardTitle>
						</CardHeader>
						<CardContent>
							<p className="font-heading text-2xl font-semibold">
								{new Date(project.createdAt).toLocaleDateString(
									'en-US',
									{
										month: 'long',
										day: 'numeric',
										year: 'numeric',
									}
								)}
							</p>
							<p className="text-muted-foreground mt-1 text-sm">
								{daysSinceCreation === 0
									? 'Created today'
									: `${daysSinceCreation} ${daysSinceCreation === 1 ? 'day' : 'days'} ago`}
							</p>
						</CardContent>
					</Card>
				</FadeIn>

				<FadeIn delay={0.3}>
					<Card className="border-2">
						<CardHeader>
							<CardTitle className="flex items-center gap-2 text-lg">
								<Clock className="text-primary h-5 w-5" />
								Status
							</CardTitle>
						</CardHeader>
						<CardContent>
							<p className="font-heading text-2xl font-semibold capitalize">
								{project.status}
							</p>
							<p className="text-muted-foreground mt-1 text-sm">
								{project.status === 'active'
									? 'Currently in progress'
									: project.status === 'completed'
										? 'Successfully completed'
										: 'Archived and inactive'}
							</p>
						</CardContent>
					</Card>
				</FadeIn>
			</div>

			<FadeIn delay={0.4}>
				<Card className="border-2">
					<CardHeader>
						<CardTitle>Project Details</CardTitle>
					</CardHeader>
					<CardContent className="space-y-4">
						<div>
							<h3 className="font-heading mb-2 font-semibold">
								Description
							</h3>
							<p className="text-muted-foreground font-body">
								{project.description}
							</p>
						</div>

						<div className="border-t pt-4">
							<h3 className="font-heading mb-2 font-semibold">
								Project Information
							</h3>
							<dl className="grid grid-cols-2 gap-4 text-sm">
								<div>
									<dt className="text-muted-foreground">
										Project ID
									</dt>
									<dd className="mt-1 font-mono text-xs">
										{project.id}
									</dd>
								</div>
								<div>
									<dt className="text-muted-foreground">
										Status
									</dt>
									<dd className="mt-1 capitalize">
										{project.status}
									</dd>
								</div>
							</dl>
						</div>
					</CardContent>
				</Card>
			</FadeIn>

			<FadeIn delay={0.5}>
				<Button
					variant="outline"
					onClick={() => navigate('/projects')}
					className="w-full sm:w-auto"
				>
					<ArrowLeft className="mr-2 h-4 w-4" />
					Back to Projects
				</Button>
			</FadeIn>

			<ProjectDialog
				open={isEditDialogOpen}
				onOpenChange={setIsEditDialogOpen}
				project={project}
				onSubmit={handleUpdate}
			/>
		</div>
	)
}
