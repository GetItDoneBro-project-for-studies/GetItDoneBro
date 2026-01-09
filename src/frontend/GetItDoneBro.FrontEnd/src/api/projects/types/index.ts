import { Branded } from '@/types/models'

export type ProjectId = Branded<string, 'ProjectId'>
export type UserId = Branded<string, 'UserId'>

export enum ProjectRole {
	Member = 1,
	Admin = 2,
}

export interface Project {
	id: ProjectId
	name: string
	description: string
}

export interface ProjectListItem extends Omit<Project, 'description'> {}

export interface ProjectPayload {
	name: string
	description: string
}

export interface ProjectUserDto {
	id: UserId
	keycloakId: string
	role: ProjectRole
	assignedAtUtc: string
}

export interface AssignUserToProjectPayload {
	keycloakId: string
	role: ProjectRole
}

export interface UpdateUserRolePayload {
	role: ProjectRole
}

export interface GetProjectsParams {
	userId?: string
}
