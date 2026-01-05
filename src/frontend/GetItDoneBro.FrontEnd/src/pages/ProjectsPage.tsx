import LoaderSkeleton from '@/components/LoaderSkeleton'
import { FadeIn } from '@/components/motion-primitives/FadeIn'
import { ProjectDialog } from '@/components/projects/ProjectDialog'
import { ProjectList } from '@/components/projects/ProjectList'
import { Button } from '@/components/ui/button'
import { useProjects } from '@/contexts/ProjectsContext'
import { CreateProjectInput, Project } from '@/types/project'
import { Plus } from 'lucide-react'
import { useEffect, useState } from 'react'

export function ProjectsPage() {
	const {
		projects,
		addProject,
		updateProject,
		deleteProject,
		fetchProjects,
		isOperating,
		isLoading,
	} = useProjects()
	const [isDialogOpen, setIsDialogOpen] = useState(false)
	const [editingProject, setEditingProject] = useState<Project | undefined>()

	// Fetch projects when component mounts
	useEffect(() => {
		fetchProjects()
	}, [fetchProjects])

	const handleCreateProject = () => {
		setEditingProject(undefined)
		setIsDialogOpen(true)
	}

	const handleEditProject = (project: Project) => {
		setEditingProject(project)
		setIsDialogOpen(true)
	}

	const handleSubmit = async (data: CreateProjectInput) => {
		try {
			if (editingProject) {
				await updateProject(editingProject.id, data)
			} else {
				await addProject(data)
			}
			setIsDialogOpen(false)
		} catch (error) {
			// Error is already handled in context with toast
			console.error('Operation failed:', error)
		}
	}

	const handleDelete = async (id: string) => {
		try {
			await deleteProject(id)
		} catch (error) {
			// Error is already handled in context with toast
			console.error('Delete failed:', error)
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
							Projects
						</h1>
						<p className="text-muted-foreground font-body">
							Manage and track all your projects in one place
						</p>
					</div>
					<Button
						onClick={handleCreateProject}
						size="lg"
						className="bg-cta hover:bg-cta/90 gap-2"
						disabled={isOperating}
					>
						<Plus className="h-5 w-5" />
						New Project
					</Button>
				</div>
			</FadeIn>

			<ProjectList
				projects={projects}
				onEdit={handleEditProject}
				onDelete={handleDelete}
			/>

			<ProjectDialog
				open={isDialogOpen}
				onOpenChange={setIsDialogOpen}
				project={editingProject}
				onSubmit={handleSubmit}
				isSubmitting={isOperating}
			/>
		</div>
	)
}
