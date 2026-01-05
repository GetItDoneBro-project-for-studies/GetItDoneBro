import { CreateProjectInput, Project } from '@/types/project'
import React, { createContext, useContext, useEffect, useState } from 'react'

interface ProjectsContextType {
	projects: Project[]
	addProject: (project: CreateProjectInput) => void
	updateProject: (id: string, updates: Partial<Project>) => void
	deleteProject: (id: string) => void
	getProjectById: (id: string) => Project | undefined
	isLoading: boolean
}

const ProjectsContext = createContext<ProjectsContextType | undefined>(
	undefined
)

const STORAGE_KEY = 'getitdonebro_projects'

// Initial dummy data
const initialProjects: Project[] = [
	{
		id: '1',
		name: 'Website Redesign',
		description: 'Redesign company website with modern UI/UX',
		status: 'active',
		createdAt: new Date('2026-01-01'),
	},
	{
		id: '2',
		name: 'Mobile App Development',
		description: 'Build iOS and Android mobile application',
		status: 'active',
		createdAt: new Date('2026-01-02'),
	},
	{
		id: '3',
		name: 'Database Migration',
		description: 'Migrate from MySQL to PostgreSQL',
		status: 'completed',
		createdAt: new Date('2025-12-15'),
	},
]

export function ProjectsProvider({ children }: { children: React.ReactNode }) {
	const [projects, setProjects] = useState<Project[]>([])
	const [isLoading, setIsLoading] = useState(true)

	// Load from localStorage on mount
	useEffect(() => {
		try {
			const stored = localStorage.getItem(STORAGE_KEY)
			if (stored) {
				const parsed = JSON.parse(stored)
				// Convert date strings back to Date objects
				const withDates = parsed.map((p: unknown) => {
					const project = p as Project
					return {
						...project,
						createdAt: new Date(project.createdAt),
					}
				})
				setProjects(withDates)
			} else {
				// First time - use initial data
				setProjects(initialProjects)
			}
		} catch (error) {
			console.error('Failed to load projects from storage:', error)
			setProjects(initialProjects)
		} finally {
			setIsLoading(false)
		}
	}, [])

	// Save to localStorage whenever projects change
	useEffect(() => {
		if (!isLoading) {
			try {
				localStorage.setItem(STORAGE_KEY, JSON.stringify(projects))
			} catch (error) {
				console.error('Failed to save projects to storage:', error)
			}
		}
	}, [projects, isLoading])

	const addProject = (projectInput: CreateProjectInput) => {
		const newProject: Project = {
			...projectInput,
			id: crypto.randomUUID(),
			createdAt: new Date(),
		}
		setProjects((prev) => [newProject, ...prev])
	}

	const updateProject = (id: string, updates: Partial<Project>) => {
		setProjects((prev) =>
			prev.map((project) =>
				project.id === id ? { ...project, ...updates } : project
			)
		)
	}

	const deleteProject = (id: string) => {
		setProjects((prev) => prev.filter((project) => project.id !== id))
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
				isLoading,
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
