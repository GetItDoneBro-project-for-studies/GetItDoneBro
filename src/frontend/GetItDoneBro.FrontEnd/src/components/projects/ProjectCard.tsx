import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { usePageTransition } from '@/hooks/usePageTransition'
import { Project } from '@/types/project'
import { motion } from 'framer-motion'
import { Calendar, Pencil, Trash2 } from 'lucide-react'
import { useNavigate } from 'react-router-dom'

interface ProjectCardProps {
	project: Project
	onEdit: (project: Project) => void
	onDelete: (id: string) => void
}

const statusColors = {
	active: 'bg-blue-500/10 text-blue-600 dark:text-blue-400 border-blue-500/20',
	completed:
		'bg-green-500/10 text-green-600 dark:text-green-400 border-green-500/20',
	archived:
		'bg-gray-500/10 text-gray-600 dark:text-gray-400 border-gray-500/20',
}

export function ProjectCard({ project, onEdit, onDelete }: ProjectCardProps) {
	const navigate = useNavigate()
	const { isReducedMotion } = usePageTransition()

	const handleCardClickAsync = async (e: React.MouseEvent) => {
		if ((e.target as HTMLElement).closest('button')) {
			return
		}
		await navigate(`/projects/${project.id}`)
	}

	const handleEdit = (e: React.MouseEvent) => {
		e.stopPropagation()
		onEdit(project)
	}

	const handleDelete = (e: React.MouseEvent) => {
		e.stopPropagation()
		if (
			window.confirm(`Are you sure you want to delete "${project.name}"?`)
		) {
			onDelete(project.id)
		}
	}

	const CardWrapper = isReducedMotion ? 'div' : motion.div

	return (
		<CardWrapper
			{...(isReducedMotion
				? {}
				: {
						whileHover: { scale: 1.02, y: -4 },
						transition: { duration: 0.1, ease: 'easeOut' },
					})}
		>
			<Card
				className="border-border hover:border-primary/50 cursor-pointer overflow-hidden border-2 transition-all duration-200 hover:shadow-lg"
				onClick={handleCardClickAsync}
			>
				<CardHeader className="pb-3">
					<div className="flex items-start justify-between gap-2">
						<CardTitle className="font-heading line-clamp-1 text-lg">
							{project.name}
						</CardTitle>
						<Badge
							variant="outline"
							className={statusColors[project.status]}
						>
							{project.status}
						</Badge>
					</div>
				</CardHeader>
				<CardContent className="space-y-4">
					<p className="text-muted-foreground font-body line-clamp-2 text-sm">
						{project.description}
					</p>

					<div className="flex items-center justify-between">
						<div className="text-muted-foreground flex items-center gap-2 text-xs">
							<Calendar className="h-3.5 w-3.5" />
							<span>
								{new Date(project.createdAt).toLocaleDateString(
									'en-US',
									{
										month: 'short',
										day: 'numeric',
										year: 'numeric',
									}
								)}
							</span>
						</div>

						<div className="flex items-center gap-1">
							<Button
								variant="ghost"
								size="icon"
								className="hover:bg-primary/10 hover:text-primary h-8 w-8 transition-colors duration-100"
								onClick={handleEdit}
							>
								<Pencil className="h-4 w-4" />
							</Button>
							<Button
								variant="ghost"
								size="icon"
								className="hover:bg-destructive/10 hover:text-destructive h-8 w-8 transition-colors duration-100"
								onClick={handleDelete}
							>
								<Trash2 className="h-4 w-4" />
							</Button>
						</div>
					</div>
				</CardContent>
			</Card>
		</CardWrapper>
	)
}
