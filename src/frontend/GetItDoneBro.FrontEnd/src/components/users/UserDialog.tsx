import { AddUserPayload } from '@/api/users/types'
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogHeader,
	DialogTitle,
} from '@/components/ui/dialog'
import { UserForm } from './UserForm'

interface UserDialogProps {
	open: boolean
	onOpenChange: (open: boolean) => void
	onSubmit: (data: AddUserPayload) => void
	isSubmitting?: boolean
}

export function UserDialog({
	open,
	onOpenChange,
	onSubmit,
	isSubmitting = false,
}: UserDialogProps) {
	const handleSubmit = async (data: AddUserPayload) => {
		await onSubmit(data)
	}

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent className="max-w-lg">
				<DialogHeader>
					<DialogTitle className="font-heading text-2xl">
						Add New User
					</DialogTitle>
					<DialogDescription>
						Enter the user details below. An invitation email will
						be sent to the provided email address.
					</DialogDescription>
				</DialogHeader>
				<UserForm
					onSubmit={handleSubmit}
					onCancel={() => onOpenChange(false)}
					isSubmitting={isSubmitting}
				/>
			</DialogContent>
		</Dialog>
	)
}
