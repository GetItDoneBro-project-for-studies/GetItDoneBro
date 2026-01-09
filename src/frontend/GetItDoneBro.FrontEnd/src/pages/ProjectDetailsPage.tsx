import {
	assignUserToProjectAsync,
	getProjectByIdAsync,
	getProjectUsersAsync,
	removeUserFromProjectAsync,
	updateUserRoleAsync,
} from '@/api/projects'
import {
	AssignUserToProjectPayload,
	ProjectId,
	ProjectRole,
	ProjectUserDto,
	UserId,
} from '@/api/projects/types'
import { getAllUsersAsync } from '@/api/users'
import { User } from '@/api/users/types'
import LoaderSkeleton from '@/components/LoaderSkeleton'
import { FadeIn } from '@/components/motion-primitives/FadeIn'
import { AssignUserDialog } from '@/components/projects/AssignUserDialog'
import { ProjectDialog } from '@/components/projects/ProjectDialog'
import { ProjectUsersCard } from '@/components/projects/ProjectUsersCard'
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
import { CreateProjectInput, Project } from '@/types/project'
import { ArrowLeft, Calendar, Clock, Pencil, Trash2 } from 'lucide-react'
import { useCallback, useEffect, useState } from 'react'
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
	const { updateProject, deleteProject, isOperating } = useProjects()
	const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)
	const [isAssignUserDialogOpen, setIsAssignUserDialogOpen] = useState(false)
	const [project, setProject] = useState<Project | null>(null)
	const [projectUsers, setProjectUsers] = useState<ProjectUserDto[]>([])
	const [availableUsers, setAvailableUsers] = useState<User[]>([])
	const [isLoading, setIsLoading] = useState(true)
	const [isLoadingUsers, setIsLoadingUsers] = useState(false)
	const [isUserOperating, setIsUserOperating] = useState(false)

	const fetchProjectUsers = useCallback(async () => {
		if (!id) return
		setIsLoadingUsers(true)
		try {
			const response = await getProjectUsersAsync(id as ProjectId)
			if (response && response.data) {
				setProjectUsers(response.data)
			} else {
				setProjectUsers([])
			}
		} catch (error) {
			console.error('Failed to load project users:', error)
		} finally {
			setIsLoadingUsers(false)
		}
	}, [id])

	const fetchAvailableUsers = useCallback(async () => {
		try {
			const response = await getAllUsersAsync()
			if (response && response.data) {
				// Filter out users already assigned to project
				const assignedUserIds = projectUsers.map((u) => u.id)
				const available = response.data.filter(
					(u) => u.enabled && !assignedUserIds.includes(u.id)
				)
				setAvailableUsers(available)
			}
		} catch (error) {
			console.error('Failed to load available users:', error)
		}
	}, [projectUsers])

	useEffect(() => {
		const loadProject = async () => {
			if (!id) {
				setIsLoading(false)
				return
			}

			setIsLoading(true)
			try {
				const response = await getProjectByIdAsync(id as ProjectId)
				if (response && response.data) {
					// Convert API response to Project type
					const fullProject: Project = {
						id: response.data.id,
						name: response.data.name,
						description: '', // API doesn't return description in detail
						status: 'active',
						createdAt: new Date(),
					}
					setProject(fullProject)
				} else {
					setProject(null)
				}
			} catch (error) {
				console.error('Failed to load project:', error)
				toast.error('Failed to load project')
				setProject(null)
			} finally {
				setIsLoading(false)
			}
		}

		loadProject()
	}, [id])

	useEffect(() => {
		if (project) {
			fetchProjectUsers()
		}
	}, [project, fetchProjectUsers])

	useEffect(() => {
		if (isAssignUserDialogOpen) {
			fetchAvailableUsers()
		}
	}, [isAssignUserDialogOpen, fetchAvailableUsers])

	if (isLoading) {
		return <LoaderSkeleton />
	}

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
			!project ||
			!window.confirm(
				`Are you sure you want to delete "${project.name}"?`
			)
		) {
			return
		}

		try {
			await deleteProject(project.id)
			navigate('/projects')
		} catch (error) {
			// Error handled in context
		}
	}

	const handleUpdate = async (data: CreateProjectInput) => {
		if (!project) return

		try {
			await updateProject(project.id, data)
			// Reload project after update
			const response = await getProjectByIdAsync(project.id as ProjectId)
			if (response && response.data) {
				const updatedProject: Project = {
					...project,
					name: response.data.name,
					description: data.description,
					status: data.status,
				}
				setProject(updatedProject)
			}
			setIsEditDialogOpen(false)
		} catch (error) {
			// Error handled in context
		}
	}

	const handleAssignUser = async (data: AssignUserToProjectPayload) => {
		if (!project) return
		setIsUserOperating(true)
		try {
			await assignUserToProjectAsync(project.id as ProjectId, data)
			toast.success('User assigned successfully')
			await fetchProjectUsers()
			setIsAssignUserDialogOpen(false)
		} catch (error) {
			console.error('Failed to assign user:', error)
		} finally {
			setIsUserOperating(false)
		}
	}

	const handleUpdateUserRole = async (userId: UserId, role: ProjectRole) => {
		if (!project) return
		setIsUserOperating(true)
		try {
			await updateUserRoleAsync(project.id as ProjectId, userId, { role })
			toast.success('User role updated')
			await fetchProjectUsers()
		} catch (error) {
			console.error('Failed to update user role:', error)
		} finally {
			setIsUserOperating(false)
		}
	}

	const handleRemoveUser = async (userId: UserId) => {
		if (!project) return
		setIsUserOperating(true)
		try {
			await removeUserFromProjectAsync(project.id as ProjectId, userId)
			toast.success('User removed from project')
			await fetchProjectUsers()
		} catch (error) {
			console.error('Failed to remove user:', error)
		} finally {
			setIsUserOperating(false)
		}
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
						<Button
							variant="outline"
							onClick={handleEdit}
							disabled={isOperating}
						>
							<Pencil className="mr-2 h-4 w-4" />
							Edit
						</Button>
						<Button
							variant="outline"
							onClick={handleDeleteAsync}
							className="text-destructive hover:bg-destructive/10"
							disabled={isOperating}
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

			<FadeIn delay={0.45}>
				<ProjectUsersCard
					users={projectUsers}
					isLoading={isLoadingUsers}
					onAssignUser={() => setIsAssignUserDialogOpen(true)}
					onUpdateRole={handleUpdateUserRole}
					onRemoveUser={handleRemoveUser}
					disabled={isUserOperating}
				/>
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
				isSubmitting={isOperating}
			/>

			<AssignUserDialog
				open={isAssignUserDialogOpen}
				onOpenChange={setIsAssignUserDialogOpen}
				onSubmit={handleAssignUser}
				isSubmitting={isUserOperating}
				availableUsers={availableUsers}
				isLoadingUsers={isLoadingUsers}
			/>
		</div>
	)
}
