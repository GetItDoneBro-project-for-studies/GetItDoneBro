import { Task } from '@/api/tasks/types'
import { Button } from '@/components/ui/button'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogFooter,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { useEffect, useState } from 'react'

interface TaskDialogProps {
	open: boolean
	onOpenChange: (open: boolean) => void
	onSubmit: (data: {
		title: string
		description: string
		assignedToKeycloakId?: string | null
		imageUrl?: string | null
	}) => Promise<void>
	task?: Task | null
	isSubmitting: boolean
}

export function TaskDialog({
	open,
	onOpenChange,
	onSubmit,
	task,
	isSubmitting,
}: TaskDialogProps) {
	const [title, setTitle] = useState('')
	const [description, setDescription] = useState('')
	const [imageUrl, setImageUrl] = useState('')

	useEffect(() => {
		if (task) {
			setTitle(task.title)
			setDescription(task.description)
			setImageUrl(task.imageUrl || '')
		} else {
			setTitle('')
			setDescription('')
			setImageUrl('')
		}
	}, [task, open])

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		await onSubmit({
			title,
			description,
			imageUrl: imageUrl || null,
		})
	}

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent>
				<form onSubmit={handleSubmit}>
					<DialogHeader>
						<DialogTitle>
							{task ? 'Edit Task' : 'Create New Task'}
						</DialogTitle>
						<DialogDescription>
							{task
								? 'Update the task details'
								: 'Add a new task to the column'}
						</DialogDescription>
					</DialogHeader>

					<div className="space-y-4 py-4">
						<div className="space-y-2">
							<Label htmlFor="title">Title</Label>
							<Input
								id="title"
								value={title}
								onChange={(e) => setTitle(e.target.value)}
								placeholder="Task title"
								required
							/>
						</div>

						<div className="space-y-2">
							<Label htmlFor="description">Description</Label>
							<Textarea
								id="description"
								value={description}
								onChange={(e) => setDescription(e.target.value)}
								placeholder="Task description"
								rows={4}
								required
							/>
						</div>

						<div className="space-y-2">
							<Label htmlFor="imageUrl">
								Image URL (optional)
							</Label>
							<Input
								id="imageUrl"
								value={imageUrl}
								onChange={(e) => setImageUrl(e.target.value)}
								placeholder="https://example.com/image.jpg"
								type="url"
							/>
						</div>
					</div>

					<DialogFooter>
						<Button
							type="button"
							variant="outline"
							onClick={() => onOpenChange(false)}
							disabled={isSubmitting}
						>
							Cancel
						</Button>
						<Button type="submit" disabled={isSubmitting}>
							{isSubmitting
								? 'Saving...'
								: task
									? 'Update'
									: 'Create'}
						</Button>
					</DialogFooter>
				</form>
			</DialogContent>
		</Dialog>
	)
}
