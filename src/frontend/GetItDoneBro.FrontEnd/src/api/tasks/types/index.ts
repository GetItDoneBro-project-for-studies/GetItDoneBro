import { ProjectId } from '@/api/projects/types'
import { TaskColumnId } from '@/api/taskColumns/types'
import { Branded } from '@/types/models'

export type TaskId = Branded<string, 'TaskId'>

export interface Task {
	id: TaskId
	projectId: ProjectId
	taskColumnId: TaskColumnId
	title: string
	description: string
	assignedToKeycloakId: string | null
	imageUrl: string | null
	createdAtUtc: string
	updatedAtUtc: string | null
}

export interface TaskPayload {
	projectId: string
	taskColumnId: string
	title: string
	description: string
	assignedToKeycloakId?: string | null
	imageUrl?: string | null
}

export interface UpdateTaskPayload {
	title: string
	description: string
	assignedToKeycloakId?: string | null
	imageUrl?: string | null
}

export interface MoveTaskToColumnPayload {
	taskColumnId: string
}
