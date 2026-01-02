import { Spinner } from './ui/spinner'

type LoadingSkeletonProps = {
	message?: string
}

export default function LoaderSkeleton({
	message = 'Loading...',
}: LoadingSkeletonProps) {
	return (
		<div className="text-muted-foreground flex animate-pulse items-center gap-2">
			<span>{message}</span>
			<Spinner />
		</div>
	)
}
