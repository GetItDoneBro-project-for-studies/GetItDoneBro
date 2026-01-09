import { TaskColumn } from '@/api/taskColumns/types'
import { Task } from '@/api/tasks/types'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { GripVertical, Pencil, Plus, Trash2 } from 'lucide-react'
import { useState } from 'react'

interface KanbanBoardProps {
	columns: TaskColumn[]
	tasks: Task[]
	isLoading: boolean
	onCreateColumn: () => void
	onUpdateColumn: (columnId: string) => void
	onDeleteColumn: (columnId: string) => void
	onCreateTask: (columnId: string) => void
	onUpdateTask: (taskId: string) => void
	onDeleteTask: (taskId: string) => void
	onMoveTask: (taskId: string, targetColumnId: string) => void
}

export function KanbanBoard({
	columns,
	tasks,
	isLoading,
	onCreateColumn,
	onUpdateColumn,
	onDeleteColumn,
	onCreateTask,
	onUpdateTask,
	onDeleteTask,
	onMoveTask,
}: KanbanBoardProps) {
	const [draggedTask, setDraggedTask] = useState<string | null>(null)

	const getTasksByColumn = (columnId: string) => {
		return tasks.filter((task) => task.taskColumnId === columnId)
	}

	const handleDragStart = (taskId: string) => {
		setDraggedTask(taskId)
	}

	const handleDragOver = (e: React.DragEvent) => {
		e.preventDefault()
	}

	const handleDrop = (columnId: string) => {
		if (draggedTask) {
			onMoveTask(draggedTask, columnId)
			setDraggedTask(null)
		}
	}

	if (isLoading) {
		return (
			<div className="flex h-96 items-center justify-center">
				<div className="text-muted-foreground">Loading board...</div>
			</div>
		)
	}

	const sortedColumns = [...columns].sort(
		(a, b) => a.orderIndex - b.orderIndex
	)

	return (
		<div className="space-y-4">
			<div className="flex items-center justify-between">
				<h2 className="font-heading text-2xl font-bold">
					Kanban Board
				</h2>
				<Button onClick={onCreateColumn}>
					<Plus className="mr-2 h-4 w-4" />
					Add Column
				</Button>
			</div>

			<div className="flex gap-4 overflow-x-auto pb-4">
				{sortedColumns.length === 0 ? (
					<div className="flex h-96 w-full items-center justify-center">
						<div className="text-center">
							<p className="text-muted-foreground mb-4">
								No columns yet. Create your first column to get
								started.
							</p>
							<Button onClick={onCreateColumn}>
								<Plus className="mr-2 h-4 w-4" />
								Create First Column
							</Button>
						</div>
					</div>
				) : (
					sortedColumns.map((column) => {
						const columnTasks = getTasksByColumn(column.id)
						return (
							<Card
								key={column.id}
								className="min-w-80 shrink-0 border-2"
								onDragOver={handleDragOver}
								onDrop={() => handleDrop(column.id)}
							>
								<CardHeader className="border-b pb-3">
									<div className="flex items-center justify-between">
										<CardTitle className="flex items-center gap-2 text-lg">
											<GripVertical className="text-muted-foreground h-4 w-4 cursor-move" />
											{column.name}
											<span className="text-muted-foreground ml-2 text-sm font-normal">
												({columnTasks.length})
											</span>
										</CardTitle>
										<div className="flex gap-1">
											<Button
												variant="ghost"
												size="sm"
												onClick={() =>
													onUpdateColumn(column.id)
												}
											>
												<Pencil className="h-4 w-4" />
											</Button>
											<Button
												variant="ghost"
												size="sm"
												onClick={() =>
													onDeleteColumn(column.id)
												}
												className="text-destructive hover:bg-destructive/10"
											>
												<Trash2 className="h-4 w-4" />
											</Button>
										</div>
									</div>
								</CardHeader>
								<CardContent className="space-y-2 p-3">
									{columnTasks.map((task) => (
										<Card
											key={task.id}
											draggable
											onDragStart={() =>
												handleDragStart(task.id)
											}
											className="bg-card cursor-move border p-3 transition-shadow hover:shadow-md"
										>
											<div className="mb-2 flex items-start justify-between">
												<h4 className="font-semibold">
													{task.title}
												</h4>
												<div className="flex gap-1">
													<Button
														variant="ghost"
														size="sm"
														className="h-6 w-6 p-0"
														onClick={() =>
															onUpdateTask(
																task.id
															)
														}
													>
														<Pencil className="h-3 w-3" />
													</Button>
													<Button
														variant="ghost"
														size="sm"
														className="text-destructive hover:bg-destructive/10 h-6 w-6 p-0"
														onClick={() =>
															onDeleteTask(
																task.id
															)
														}
													>
														<Trash2 className="h-3 w-3" />
													</Button>
												</div>
											</div>
											{task.description && (
												<p className="text-muted-foreground mb-2 text-sm">
													{task.description}
												</p>
											)}
											{task.imageUrl && (
												<img
													src={task.imageUrl}
													alt={task.title}
													className="mb-2 h-32 w-full rounded object-cover"
												/>
											)}
											<div className="text-muted-foreground flex items-center justify-between text-xs">
												<span>
													{new Date(
														task.createdAtUtc
													).toLocaleDateString()}
												</span>
												{task.assignedToKeycloakId && (
													<span className="bg-primary/10 text-primary rounded px-2 py-1">
														Assigned
													</span>
												)}
											</div>
										</Card>
									))}
									<Button
										variant="outline"
										className="w-full"
										onClick={() => onCreateTask(column.id)}
									>
										<Plus className="mr-2 h-4 w-4" />
										Add Task
									</Button>
								</CardContent>
							</Card>
						)
					})
				)}
			</div>
		</div>
	)
}
