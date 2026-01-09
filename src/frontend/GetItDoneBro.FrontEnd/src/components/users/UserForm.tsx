import { AddUserPayload } from '@/api/users/types'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Loader2 } from 'lucide-react'
import { useState } from 'react'

interface UserFormProps {
	onSubmit: (data: AddUserPayload) => void | Promise<void>
	onCancel: () => void
	isSubmitting?: boolean
}

export function UserForm({
	onSubmit,
	onCancel,
	isSubmitting = false,
}: UserFormProps) {
	const [formData, setFormData] = useState<AddUserPayload>({
		email: '',
		firstName: '',
		lastName: '',
		enabled: true,
	})

	const [errors, setErrors] = useState<
		Partial<Record<keyof AddUserPayload, string>>
	>({})

	const validate = (): boolean => {
		const newErrors: Partial<Record<keyof AddUserPayload, string>> = {}

		if (!formData.email.trim()) {
			newErrors.email = 'Email is required'
		} else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
			newErrors.email = 'Invalid email format'
		}

		setErrors(newErrors)
		return Object.keys(newErrors).length === 0
	}

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		if (validate()) {
			await onSubmit(formData)
		}
	}

	return (
		<form onSubmit={handleSubmit} className="space-y-6">
			<div className="space-y-2">
				<Label htmlFor="email">
					Email <span className="text-destructive">*</span>
				</Label>
				<Input
					id="email"
					type="email"
					value={formData.email}
					onChange={(e) => {
						setFormData({ ...formData, email: e.target.value })
						if (errors.email)
							setErrors({ ...errors, email: undefined })
					}}
					placeholder="user@example.com"
					className={errors.email ? 'border-destructive' : ''}
					disabled={isSubmitting}
				/>
				{errors.email && (
					<p className="text-destructive text-sm">{errors.email}</p>
				)}
			</div>

			<div className="grid gap-4 sm:grid-cols-2">
				<div className="space-y-2">
					<Label htmlFor="firstName">First Name</Label>
					<Input
						id="firstName"
						value={formData.firstName}
						onChange={(e) =>
							setFormData({
								...formData,
								firstName: e.target.value,
							})
						}
						placeholder="John"
						disabled={isSubmitting}
					/>
				</div>

				<div className="space-y-2">
					<Label htmlFor="lastName">Last Name</Label>
					<Input
						id="lastName"
						value={formData.lastName}
						onChange={(e) =>
							setFormData({
								...formData,
								lastName: e.target.value,
							})
						}
						placeholder="Doe"
						disabled={isSubmitting}
					/>
				</div>
			</div>

			<div className="flex justify-end gap-3 pt-4">
				<Button
					type="button"
					variant="outline"
					onClick={onCancel}
					disabled={isSubmitting}
				>
					Cancel
				</Button>
				<Button type="submit" disabled={isSubmitting}>
					{isSubmitting ? (
						<>
							<Loader2 className="mr-2 h-4 w-4 animate-spin" />
							Adding...
						</>
					) : (
						'Add User'
					)}
				</Button>
			</div>
		</form>
	)
}
