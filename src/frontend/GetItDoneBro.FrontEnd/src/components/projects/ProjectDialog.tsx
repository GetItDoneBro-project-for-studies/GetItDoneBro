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
}

export function ProjectDialog({
	open,
	onOpenChange,
	project,
	onSubmit,
}: ProjectDialogProps) {
	const handleSubmit = (data: CreateProjectInput) => {
		onSubmit(data)
		onOpenChange(false)
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
				/>
			</DialogContent>
		</Dialog>
	)
}
