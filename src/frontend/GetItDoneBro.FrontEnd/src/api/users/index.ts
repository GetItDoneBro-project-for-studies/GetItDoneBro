import { axiosRequest } from '@/hooks/useAxios'
import { AxiosResultObject } from '@/types/models'

export async function getProjects() {
	return axiosRequest<AxiosResultObject<[]>, void>({
		url: '/api/v1/projects',
		method: 'GET',
		defaultErrorMessage: 'Failed to fetch users',
	})
}
