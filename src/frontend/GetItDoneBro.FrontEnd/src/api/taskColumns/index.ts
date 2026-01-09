import { axiosRequest } from '@/hooks/useAxios'
import {
	TaskColumn,
	TaskColumnId,
	TaskColumnPayload,
	UpdateTaskColumnPayload,
} from './types'
import { ProjectId } from '../projects/types'

export async function getTaskColumnsByProjectAsync(projectId: ProjectId) {
	return axiosRequest<TaskColumn[], void>({
		url: `/api/v1/projects/${projectId}/task-columns`,
		method: 'GET',
		defaultErrorMessage: 'Failed to fetch task columns',
	})
}

export async function createTaskColumnAsync(data: TaskColumnPayload) {
	return axiosRequest<void, TaskColumnPayload>({
		url: '/api/v1/task-columns',
		method: 'POST',
		data,
		defaultErrorMessage: 'Failed to create task column',
	})
}

export async function updateTaskColumnAsync(
	id: TaskColumnId,
	data: UpdateTaskColumnPayload
) {
	return axiosRequest<void, UpdateTaskColumnPayload>({
		url: `/api/v1/task-columns/${id}`,
		method: 'PUT',
		data,
		defaultErrorMessage: 'Failed to update task column',
	})
}

export async function deleteTaskColumnAsync(id: TaskColumnId) {
	return axiosRequest<void>({
		url: `/api/v1/task-columns/${id}`,
		method: 'DELETE',
		defaultErrorMessage: 'Failed to delete task column',
	})
}
