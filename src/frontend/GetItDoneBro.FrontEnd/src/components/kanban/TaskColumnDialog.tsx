import { TaskColumn } from '@/api/taskColumns/types'
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
import { useEffect, useState } from 'react'

interface TaskColumnDialogProps {
	open: boolean
	onOpenChange: (open: boolean) => void
	onSubmit: (data: { name: string; orderIndex: number }) => Promise<void>
	column?: TaskColumn | null
	isSubmitting: boolean
	nextOrderIndex: number
}

export function TaskColumnDialog({
	open,
	onOpenChange,
	onSubmit,
	column,
	isSubmitting,
	nextOrderIndex,
}: TaskColumnDialogProps) {
	const [name, setName] = useState('')
	const [orderIndex, setOrderIndex] = useState(nextOrderIndex)

	useEffect(() => {
		if (column) {
			setName(column.name)
			setOrderIndex(column.orderIndex)
		} else {
			setName('')
			setOrderIndex(nextOrderIndex)
		}
	}, [column, nextOrderIndex, open])

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		await onSubmit({ name, orderIndex })
	}

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent>
				<form onSubmit={handleSubmit}>
					<DialogHeader>
						<DialogTitle>
							{column ? 'Edit Column' : 'Create New Column'}
						</DialogTitle>
						<DialogDescription>
							{column
								? 'Update the column details'
								: 'Add a new column to your board'}
						</DialogDescription>
					</DialogHeader>

					<div className="space-y-4 py-4">
						<div className="space-y-2">
							<Label htmlFor="name">Column Name</Label>
							<Input
								id="name"
								value={name}
								onChange={(e) => setName(e.target.value)}
								placeholder="e.g., To Do, In Progress, Done"
								required
							/>
						</div>

						<div className="space-y-2">
							<Label htmlFor="orderIndex">Order Index</Label>
							<Input
								id="orderIndex"
								type="number"
								value={orderIndex}
								onChange={(e) =>
									setOrderIndex(parseInt(e.target.value))
								}
								min={0}
								required
							/>
							<p className="text-muted-foreground text-xs">
								Lower numbers appear first
							</p>
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
								: column
									? 'Update'
									: 'Create'}
						</Button>
					</DialogFooter>
				</form>
			</DialogContent>
		</Dialog>
	)
}
