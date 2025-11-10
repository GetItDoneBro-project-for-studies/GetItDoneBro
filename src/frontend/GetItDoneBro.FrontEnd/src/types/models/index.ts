export interface ExceptionMessage {
	title: string
	description: string
}
export interface ExternalException {
	error: {
		message: string
		description: string
	}
}

export interface AxiosResponseError<T> {
	status: number
	statusText: string
	headers: Record<string, string>
	config: Record<string, T>
	data: T
}
export interface AxiosResultObject<T> {
	isError: boolean
	errors?: Array<{
		title: string
		description: string
	}>
	data?: T
}
