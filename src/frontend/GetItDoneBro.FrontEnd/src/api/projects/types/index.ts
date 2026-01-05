import { Branded } from '@/types/models'

export type ProjectId = Branded<string, 'ProjectId'>

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
