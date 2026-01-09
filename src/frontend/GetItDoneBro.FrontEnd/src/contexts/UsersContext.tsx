import {
	addUserAsync,
	disableUserAsync,
	getAllUsersAsync,
	resetUserPasswordAsync,
} from '@/api/users'
import { AddUserPayload, User, UserId } from '@/api/users/types'
import React, { createContext, useCallback, useContext, useState } from 'react'
import { toast } from 'sonner'

interface UsersContextType {
	users: User[]
	isLoading: boolean
	isOperating: boolean
	fetchUsers: () => Promise<void>
	addUser: (data: AddUserPayload) => Promise<void>
	disableUser: (userId: UserId) => Promise<void>
	resetPassword: (userId: UserId) => Promise<void>
}

const UsersContext = createContext<UsersContextType | undefined>(undefined)

export function UsersProvider({ children }: { children: React.ReactNode }) {
	const [users, setUsers] = useState<User[]>([])
	const [isLoading, setIsLoading] = useState(false)
	const [isOperating, setIsOperating] = useState(false)

	const fetchUsers = useCallback(async () => {
		setIsLoading(true)
		try {
			const response = await getAllUsersAsync()
			if (response && response.data) {
				const data = Array.isArray(response.data) ? response.data : []
				setUsers(data)
			} else {
				setUsers([])
			}
		} catch (error) {
			console.error('Failed to fetch users:', error)
			toast.error('Failed to fetch users')
			setUsers([])
		} finally {
			setIsLoading(false)
		}
	}, [])

	const addUser = useCallback(
		async (data: AddUserPayload) => {
			setIsOperating(true)
			try {
				await addUserAsync(data)
				toast.success('User added successfully')
				await fetchUsers()
			} catch (error) {
				console.error('Failed to add user:', error)
				throw error
			} finally {
				setIsOperating(false)
			}
		},
		[fetchUsers]
	)

	const disableUser = useCallback(
		async (userId: UserId) => {
			setIsOperating(true)
			try {
				await disableUserAsync(userId)
				toast.success('User disabled successfully')
				await fetchUsers()
			} catch (error) {
				console.error('Failed to disable user:', error)
				throw error
			} finally {
				setIsOperating(false)
			}
		},
		[fetchUsers]
	)

	const resetPassword = useCallback(async (userId: UserId) => {
		setIsOperating(true)
		try {
			await resetUserPasswordAsync(userId)
			toast.success('Password reset email sent')
		} catch (error) {
			console.error('Failed to reset password:', error)
			throw error
		} finally {
			setIsOperating(false)
		}
	}, [])

	return (
		<UsersContext.Provider
			value={{
				users,
				isLoading,
				isOperating,
				fetchUsers,
				addUser,
				disableUser,
				resetPassword,
			}}
		>
			{children}
		</UsersContext.Provider>
	)
}

export function useUsers() {
	const context = useContext(UsersContext)
	if (context === undefined) {
		throw new Error('useUsers must be used within a UsersProvider')
	}
	return context
}
