import { axiosRequest } from '@/hooks/useAxios'
import { ProjectId, ProjectListItem, ProjectPayload } from './types'

export async function getAllProjectsAsync() {
	return axiosRequest<ProjectListItem[], void>({
		url: '/api/v1/projects',
		method: 'GET',
		defaultErrorMessage: 'Failed to fetch projects',
	})
}

export async function getProjectByIdAsync(id: ProjectId) {
	return axiosRequest<ProjectListItem, void>({
		url: `/api/v1/projects/${id}`,
		method: 'GET',
		defaultErrorMessage: 'Failed to fetch project',
	})
}

export async function createProjectAsync(data: ProjectPayload) {
	return axiosRequest<void, ProjectPayload>({
		url: '/api/v1/projects',
		method: 'POST',
		data,
		defaultErrorMessage: 'Failed to create project',
	})
}

export async function updateProjectAsync(id: ProjectId, data: ProjectPayload) {
	return axiosRequest<void, ProjectPayload>({
		url: `/api/v1/projects/${id}`,
		method: 'PUT',
		data,
		defaultErrorMessage: 'Failed to update project',
	})
}

export async function deleteProjectAsync(id: ProjectId) {
	return axiosRequest<void>({
		url: `/api/v1/projects/${id}`,
		method: 'DELETE',
		defaultErrorMessage: 'Failed to delete project',
	})
}
