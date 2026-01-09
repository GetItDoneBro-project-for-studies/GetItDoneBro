import { axiosRequest } from '@/hooks/useAxios'
import {
	AssignUserToProjectPayload,
	GetProjectsParams,
	ProjectId,
	ProjectListItem,
	ProjectPayload,
	ProjectUserDto,
	UpdateUserRolePayload,
	UserId,
} from './types'

export async function getAllProjectsAsync(queryParameters?: GetProjectsParams) {
	const params = new URLSearchParams()
	if (queryParameters?.userId) {
		params.append('user_id', queryParameters.userId)
	}

	return axiosRequest<ProjectListItem[], void>({
		url: '/api/v1/projects',
		method: 'GET',
		params,
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

export async function getProjectUsersAsync(projectId: ProjectId) {
	return axiosRequest<ProjectUserDto[], void>({
		url: `/api/v1/projects/${projectId}/users`,
		method: 'GET',
		defaultErrorMessage: 'Failed to fetch project users',
	})
}

export async function assignUserToProjectAsync(
	projectId: ProjectId,
	data: AssignUserToProjectPayload
) {
	return axiosRequest<void, AssignUserToProjectPayload>({
		url: `/api/v1/projects/${projectId}/users`,
		method: 'POST',
		data,
		defaultErrorMessage: 'Failed to assign user to project',
	})
}

export async function updateUserRoleAsync(
	projectId: ProjectId,
	userId: UserId,
	data: UpdateUserRolePayload
) {
	return axiosRequest<void, UpdateUserRolePayload>({
		url: `/api/v1/projects/${projectId}/users/${userId}`,
		method: 'PUT',
		data,
		defaultErrorMessage: 'Failed to update user role',
	})
}

export async function removeUserFromProjectAsync(
	projectId: ProjectId,
	userId: UserId
) {
	return axiosRequest<void>({
		url: `/api/v1/projects/${projectId}/users/${userId}`,
		method: 'DELETE',
		defaultErrorMessage: 'Failed to remove user from project',
	})
}
