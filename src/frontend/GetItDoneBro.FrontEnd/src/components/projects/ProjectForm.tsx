import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from '@/components/ui/select'
import { Textarea } from '@/components/ui/textarea'
import { CreateProjectInput, Project, ProjectStatus } from '@/types/project'
import { Loader2 } from 'lucide-react'
import { useEffect, useState } from 'react'

interface ProjectFormProps {
	project?: Project
	onSubmit: (data: CreateProjectInput) => void | Promise<void>
	onCancel: () => void
	isSubmitting?: boolean
}

export function ProjectForm({
	project,
	onSubmit,
	onCancel,
	isSubmitting = false,
}: ProjectFormProps) {
	const [formData, setFormData] = useState<CreateProjectInput>({
		name: project?.name || '',
		description: project?.description || '',
		status: project?.status || 'active',
	})

	const [errors, setErrors] = useState<
		Partial<Record<keyof CreateProjectInput, string>>
	>({})

	useEffect(() => {
		if (project) {
			setFormData({
				name: project.name,
				description: project.description,
				status: project.status,
			})
		}
	}, [project])

	const validate = (): boolean => {
		const newErrors: Partial<Record<keyof CreateProjectInput, string>> = {}

		if (!formData.name.trim()) {
			newErrors.name = 'Project name is required'
		} else if (formData.name.length < 3) {
			newErrors.name = 'Project name must be at least 3 characters'
		}

		if (!formData.description.trim()) {
			newErrors.description = 'Description is required'
		}

		setErrors(newErrors)
		return Object.keys(newErrors).length === 0
	}

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		if (validate()) {
			await onSubmit(formData)
		}
	}

	return (
		<form onSubmit={handleSubmit} className="space-y-6">
			<div className="space-y-2">
				<Label htmlFor="name">
					Project Name <span className="text-destructive">*</span>
				</Label>
				<Input
					id="name"
					value={formData.name}
					onChange={(e) => {
						setFormData({ ...formData, name: e.target.value })
						if (errors.name)
							setErrors({ ...errors, name: undefined })
					}}
					placeholder="Enter project name"
					className={errors.name ? 'border-destructive' : ''}
					disabled={isSubmitting}
				/>
				{errors.name && (
					<p className="text-destructive text-sm">{errors.name}</p>
				)}
			</div>

			<div className="space-y-2">
				<Label htmlFor="description">
					Description <span className="text-destructive">*</span>
				</Label>
				<Textarea
					id="description"
					value={formData.description}
					onChange={(e) => {
						setFormData({
							...formData,
							description: e.target.value,
						})
						if (errors.description)
							setErrors({ ...errors, description: undefined })
					}}
					placeholder="Describe your project"
					rows={4}
					className={errors.description ? 'border-destructive' : ''}
					disabled={isSubmitting}
				/>
				{errors.description && (
					<p className="text-destructive text-sm">
						{errors.description}
					</p>
				)}
			</div>

			<div className="space-y-2">
				<Label htmlFor="status">Status</Label>
				<Select
					value={formData.status}
					onValueChange={(value: ProjectStatus) =>
						setFormData({ ...formData, status: value })
					}
					disabled={isSubmitting}
				>
					<SelectTrigger id="status">
						<SelectValue />
					</SelectTrigger>
					<SelectContent>
						<SelectItem value="active">Active</SelectItem>
						<SelectItem value="completed">Completed</SelectItem>
						<SelectItem value="archived">Archived</SelectItem>
					</SelectContent>
				</Select>
			</div>

			<div className="flex justify-end gap-3 pt-4">
				<Button
					type="button"
					variant="outline"
					onClick={onCancel}
					disabled={isSubmitting}
				>
					Cancel
				</Button>
				<Button type="submit" disabled={isSubmitting}>
					{isSubmitting ? (
						<>
							<Loader2 className="mr-2 h-4 w-4 animate-spin" />
							Saving...
						</>
					) : project ? (
						'Update Project'
					) : (
						'Create Project'
					)}
				</Button>
			</div>
		</form>
	)
}
