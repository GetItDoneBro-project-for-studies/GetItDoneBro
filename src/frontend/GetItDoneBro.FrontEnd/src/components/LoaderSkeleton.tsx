import { LoaderCircle } from 'lucide-react'

type LoadingSkeletonProps = {
	message?: string
}

export default function LoaderSkeleton({
	message = 'Loading...',
}: LoadingSkeletonProps) {
	return (
		<div className="text-muted-foreground flex animate-pulse items-center gap-2">
			<span>{message}</span>
			<LoaderCircle className="size-5 animate-spin" />
		</div>
	)
}
