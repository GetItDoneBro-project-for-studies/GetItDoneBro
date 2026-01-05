const ISODateFormat =
	/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d*)?(?:[-+]\d{2}:?\d{2}|Z)?$/
const ISODateOnlyFormat = /\d{4}-\d{2}-\d{2}/

const isIsoDateString = (value: unknown): value is string => {
	return typeof value === 'string' && ISODateFormat.test(value)
}

const isIsoDateOnlyString = (value: unknown): value is string => {
	return typeof value === 'string' && ISODateOnlyFormat.test(value)
}

export function handleDates(
	data: unknown,
	seen: WeakSet<object> = new WeakSet()
) {
	if (isIsoDateString(data) || isIsoDateOnlyString(data))
		return new Date(data)
	if (data === null || data === undefined || typeof data !== 'object')
		return data

	if (seen.has(data)) return data
	seen.add(data)

	if (Array.isArray(data)) {
		for (let i = 0; i < data.length; i++) {
			const val = data[i]
			if (isIsoDateString(val) || isIsoDateOnlyString(val)) {
				data[i] = new Date(val)
			} else if (val !== null && typeof val === 'object') {
				data[i] = handleDates(val, seen)
			}
		}
	} else {
		for (const key in data) {
			if (!Object.prototype.hasOwnProperty.call(data, key)) continue
			const val = (data as Record<string, unknown>)[key]
			if (isIsoDateString(val) || isIsoDateOnlyString(val)) {
				;(data as Record<string, unknown>)[key] = new Date(val)
			} else if (val !== null && typeof val === 'object') {
				;(data as Record<string, unknown>)[key] = handleDates(val, seen)
			}
		}
	}
	return data
}
