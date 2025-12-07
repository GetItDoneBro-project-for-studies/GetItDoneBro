import { exceptionHelper } from '@/services/helpers/ExceptionHelper'
import { AxiosResultObject } from '@/types/models'
import axios, { AxiosError, AxiosResponse } from 'axios'

type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE'

type ResponseType = 'arraybuffer' | 'blob' | 'document' | 'json' | 'text'

interface AxiosRequestOptions<TData> {
	url: string
	method: HttpMethod
	data?: TData
	signal?: AbortSignal
	defaultErrorMessage: string
	additionalInternalMessage?: string
	params?: URLSearchParams
	responseType?: ResponseType
	successMessage?: string
}

export async function axiosRequest<TResponse, TData = void>(
	options: AxiosRequestOptions<TData>
) {
	try {
		const headers: Record<string, string> = {}

		const result = await axios<TResponse>(options.url, {
			method: options.method,
			data: options.data,
			signal: options.signal,
			params: options.params,
			responseType: options.responseType,
			headers,
		})

		if (result.status === 200) {
			const data = result.data as AxiosResultObject<TResponse>
			if (data && data.isError && data.errors) {
				if (data.isError) {
					const [error] = data.errors
					const axiosResponseError: AxiosResponse = {
						status: result.status,
						statusText: result.statusText,
						headers: result.headers,
						config: result.config,
						data: {
							title: error.title,
							description: error.description,
						},
					}
					throw new AxiosError(
						undefined,
						undefined,
						undefined,
						undefined,
						axiosResponseError
					)
				}
			}
		}
		if (options.successMessage) {
			console.log('Success message:', options.successMessage)
		}
		return result
	} catch (err) {
		const error = err as AxiosError
		exceptionHelper().showExceptionMessage(
			error,
			options.defaultErrorMessage,
			options.additionalInternalMessage
		)
	}
	return null
}
