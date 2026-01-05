import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import { CreateProjectInput, Project } from '@/types/project'
import { ProjectForm } from './ProjectForm'

interface ProjectDialogProps {
	open: boolean
	onOpenChange: (open: boolean) => void
	project?: Project
	onSubmit: (data: CreateProjectInput) => void
	isSubmitting?: boolean
}

export function ProjectDialog({
	open,
	onOpenChange,
	project,
	onSubmit,
	isSubmitting = false,
}: ProjectDialogProps) {
	const handleSubmit = async (data: CreateProjectInput) => {
		await onSubmit(data)
	}

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent className="max-w-2xl">
				<DialogHeader>
					<DialogTitle className="font-heading text-2xl">
						{project ? 'Edit Project' : 'Create New Project'}
					</DialogTitle>
					<DialogDescription>
						{project
							? 'Update the project details below.'
							: 'Fill in the details to create a new project.'}
					</DialogDescription>
				</DialogHeader>
				<ProjectForm
					project={project}
					onSubmit={handleSubmit}
					onCancel={() => onOpenChange(false)}
					isSubmitting={isSubmitting}
				/>
			</DialogContent>
		</Dialog>
	)
}
