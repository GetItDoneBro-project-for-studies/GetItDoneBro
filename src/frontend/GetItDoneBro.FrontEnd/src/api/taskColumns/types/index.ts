import { ProjectId } from '@/api/projects/types'
import { Branded } from '@/types/models'

export type TaskColumnId = Branded<string, 'TaskColumnId'>

export interface TaskColumn {
	id: TaskColumnId
	projectId: ProjectId
	name: string
	orderIndex: number
	createdAtUtc: string
	updatedAtUtc: string | null
}

export interface TaskColumnPayload {
	projectId: string
	name: string
	orderIndex: number
}

export interface UpdateTaskColumnPayload {
	name: string
	orderIndex: number
}
