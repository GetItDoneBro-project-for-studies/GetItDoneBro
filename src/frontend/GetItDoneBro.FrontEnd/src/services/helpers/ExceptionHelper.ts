import { ExceptionMessage, ExternalException } from '@/types/models'
import { AxiosError } from 'axios'

export function exceptionHelper() {
	function showExceptionMessage(
		error: AxiosError,
		message: string,
		additionalInternalMessage?: string
	) {
		if (!error.response) {
			return
		}
		if (isExceptionMessage(error.response?.data)) {
			console.log('Exception message:', error.response.data)
			return
		}

		if (isExternalException(error.response?.data)) {
			const internalMessage = additionalInternalMessage
				? additionalInternalMessage
				: ''
			console.log(
				'External exception:',
				error.response.data.error.description,
				internalMessage
			)
			return
		}
		console.log(
			'Unknown error format:',
			error.response.data,
			message,
			additionalInternalMessage
		)
	}

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	function isExceptionMessage(obj: any): obj is ExceptionMessage {
		return (
			typeof obj.title === 'string' && typeof obj.description === 'string'
		)
	}

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	function isExternalException(obj: any): obj is ExternalException {
		return (
			typeof obj.error?.description === 'string' &&
			typeof obj.error?.message === 'string'
		)
	}

	return {
		showExceptionMessage,
	}
}
