import { FadeIn } from '@/components/motion-primitives/FadeIn'
import { Project } from '@/types/project'
import { FolderOpen } from 'lucide-react'
import { ProjectCard } from './ProjectCard'

interface ProjectListProps {
	projects: Project[]
	onEdit: (project: Project) => void
	onDelete: (id: string) => void
}

export function ProjectList({ projects, onEdit, onDelete }: ProjectListProps) {
	if (projects.length === 0) {
		return (
			<FadeIn>
				<div className="flex flex-col items-center justify-center py-20 text-center">
					<div className="bg-muted mb-4 flex h-20 w-20 items-center justify-center rounded-full">
						<FolderOpen className="text-muted-foreground h-10 w-10" />
					</div>
					<h3 className="font-heading mb-2 text-xl font-semibold">
						No projects yet
					</h3>
					<p className="text-muted-foreground max-w-sm">
						Get started by creating your first project. Click the
						"New Project" button above.
					</p>
				</div>
			</FadeIn>
		)
	}

	return (
		<div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
			{projects.map((project, index) => (
				<FadeIn key={project.id} delay={index * 0.05}>
					<ProjectCard
						project={project}
						onEdit={onEdit}
						onDelete={onDelete}
					/>
				</FadeIn>
			))}
		</div>
	)
}
