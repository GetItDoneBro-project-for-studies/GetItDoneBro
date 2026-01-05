export interface Project {
	id: string
	name: string
	description: string
	status: 'active' | 'completed' | 'archived'
	createdAt: Date
}

export type ProjectStatus = Project['status']

export type CreateProjectInput = Omit<Project, 'id' | 'createdAt'>
