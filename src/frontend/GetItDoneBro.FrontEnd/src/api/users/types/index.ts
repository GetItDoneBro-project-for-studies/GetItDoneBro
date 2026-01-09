import { Branded } from '@/types/models'

export type UserId = Branded<string, 'UserId'>

export interface RealmRole {
	name: string
}

export interface User {
	id: UserId
	username: string
	firstName: string | null
	lastName: string | null
	email: string | null
	emailVerified: boolean
	enabled: boolean
	realmRoles: RealmRole[]
}

export interface AddUserPayload {
	email: string
	firstName?: string
	lastName?: string
	enabled?: boolean
}
