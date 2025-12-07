import { User } from './types'

export async function getUsersAsync() {
	await new Promise((resolve) => setTimeout(resolve, 500))

	const users: User[] = [
		{
			id: '1',
			username: 'John Doe',
			email: 'john.doe@example.com',
		},
		{
			id: '2',
			username: 'Jane Smith',
			email: 'jane.smith@example.com',
		},
	]

	return {
		data: users,
	}
	// return axiosRequest<AxiosResultObject<User[]>, void>({
	// 	url: 'api/users',
	// 	method: 'GET',
	// 	defaultErrorMessage: 'Failed to fetch users',
	// })
}
