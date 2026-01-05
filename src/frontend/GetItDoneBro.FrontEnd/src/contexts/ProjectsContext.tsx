import {
	createProjectAsync,
	deleteProjectAsync,
	getAllProjectsAsync,
	updateProjectAsync,
} from '@/api/projects'
import { ProjectId, ProjectPayload } from '@/api/projects/types'
import { CreateProjectInput, Project } from '@/types/project'
import React, { createContext, useCallback, useContext, useState } from 'react'
import { toast } from 'sonner'

interface ProjectsContextType {
	projects: Project[]
	addProject: (project: CreateProjectInput) => Promise<void>
	updateProject: (id: string, updates: Partial<Project>) => Promise<void>
	deleteProject: (id: string) => Promise<void>
	getProjectById: (id: string) => Project | undefined
	fetchProjects: () => Promise<void>
	isLoading: boolean
	isOperating: boolean
}

const ProjectsContext = createContext<ProjectsContextType | undefined>(
	undefined
)

export function ProjectsProvider({ children }: { children: React.ReactNode }) {
	const [projects, setProjects] = useState<Project[]>([])
	const [isLoading, setIsLoading] = useState(false)
	const [isOperating, setIsOperating] = useState(false)

	// Fetch projects from API
	const fetchProjects = useCallback(async () => {
		setIsLoading(true)
		try {
			const response = await getAllProjectsAsync()
			if (response && response.data) {
				const data = Array.isArray(response.data) ? response.data : []
				// Convert API response to Project type with status and createdAt
				const projectsWithFullData: Project[] = data.map((item) => ({
					id: item.id,
					name: item.name,
					description: '', // API doesn't return description in list
					status: 'active' as const,
					createdAt: new Date(),
				}))
				setProjects(projectsWithFullData)
			} else {
				setProjects([])
			}
		} catch (error) {
			console.error('Failed to fetch projects:', error)
			toast.error('Failed to load projects')
			setProjects([])
		} finally {
			setIsLoading(false)
		}
	}, [])

	// Initial fetch removed - each page will fetch when needed

	const addProject = async (projectInput: CreateProjectInput) => {
		setIsOperating(true)
		try {
			const payload: ProjectPayload = {
				name: projectInput.name,
				description: projectInput.description,
			}
			await createProjectAsync(payload)
			toast.success('Project created successfully')
			// Refetch all projects after creation
			await fetchProjects()
		} catch (error) {
			console.error('Failed to create project:', error)
			toast.error('Failed to create project')
			throw error
		} finally {
			setIsOperating(false)
		}
	}

	const updateProject = async (id: string, updates: Partial<Project>) => {
		setIsOperating(true)
		try {
			const payload: ProjectPayload = {
				name: updates.name || '',
				description: updates.description || '',
			}
			await updateProjectAsync(id as ProjectId, payload)
			toast.success('Project updated successfully')
			// Refetch all projects after update
			await fetchProjects()
		} catch (error) {
			console.error('Failed to update project:', error)
			toast.error('Failed to update project')
			throw error
		} finally {
			setIsOperating(false)
		}
	}

	const deleteProject = async (id: string) => {
		setIsOperating(true)
		try {
			await deleteProjectAsync(id as ProjectId)
			toast.success('Project deleted successfully')
			// Refetch all projects after deletion
			await fetchProjects()
		} catch (error) {
			console.error('Failed to delete project:', error)
			toast.error('Failed to delete project')
			throw error
		} finally {
			setIsOperating(false)
		}
	}

	const getProjectById = (id: string) => {
		return projects.find((project) => project.id === id)
	}

	return (
		<ProjectsContext.Provider
			value={{
				projects,
				addProject,
				updateProject,
				deleteProject,
				getProjectById,
				fetchProjects,
				isLoading,
				isOperating,
			}}
		>
			{children}
		</ProjectsContext.Provider>
	)
}

export function useProjects() {
	const context = useContext(ProjectsContext)
	if (context === undefined) {
		throw new Error('useProjects must be used within a ProjectsProvider')
	}
	return context
}
