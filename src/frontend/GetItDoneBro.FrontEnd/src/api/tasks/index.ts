import { axiosRequest } from '@/hooks/useAxios'
import {
	MoveTaskToColumnPayload,
	Task,
	TaskId,
	TaskPayload,
	UpdateTaskPayload,
} from './types'
import { ProjectId } from '../projects/types'

export async function getTasksByProjectAsync(projectId: ProjectId) {
	return axiosRequest<Task[], void>({
		url: `/api/v1/projects/${projectId}/tasks`,
		method: 'GET',
		defaultErrorMessage: 'Failed to fetch tasks',
	})
}

export async function createTaskAsync(data: TaskPayload) {
	return axiosRequest<void, TaskPayload>({
		url: '/api/v1/tasks',
		method: 'POST',
		data,
		defaultErrorMessage: 'Failed to create task',
	})
}

export async function updateTaskAsync(id: TaskId, data: UpdateTaskPayload) {
	return axiosRequest<void, UpdateTaskPayload>({
		url: `/api/v1/tasks/${id}`,
		method: 'PUT',
		data,
		defaultErrorMessage: 'Failed to update task',
	})
}

export async function deleteTaskAsync(id: TaskId) {
	return axiosRequest<void>({
		url: `/api/v1/tasks/${id}`,
		method: 'DELETE',
		defaultErrorMessage: 'Failed to delete task',
	})
}

export async function moveTaskToColumnAsync(
	id: TaskId,
	data: MoveTaskToColumnPayload
) {
	return axiosRequest<void, MoveTaskToColumnPayload>({
		url: `/api/v1/tasks/${id}/move`,
		method: 'PATCH',
		data,
		defaultErrorMessage: 'Failed to move task',
	})
}
