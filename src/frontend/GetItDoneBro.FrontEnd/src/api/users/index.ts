import { axiosRequest } from '@/hooks/useAxios'
import { AddUserPayload, User, UserId } from './types'

export async function getAllUsersAsync() {
	return axiosRequest<User[], void>({
		url: '/api/v1/users',
		method: 'GET',
		defaultErrorMessage: 'Failed to fetch users',
	})
}

export async function addUserAsync(data: AddUserPayload) {
	return axiosRequest<void, AddUserPayload>({
		url: '/api/v1/users',
		method: 'POST',
		data,
		defaultErrorMessage: 'Failed to add user',
	})
}

export async function disableUserAsync(userId: UserId) {
	return axiosRequest<void>({
		url: `/api/v1/users/${userId}/disable`,
		method: 'PUT',
		defaultErrorMessage: 'Failed to disable user',
	})
}

export async function resetUserPasswordAsync(userId: UserId) {
	return axiosRequest<void>({
		url: `/api/v1/users/${userId}/reset-password`,
		method: 'PUT',
		defaultErrorMessage: 'Failed to reset user password',
	})
}
