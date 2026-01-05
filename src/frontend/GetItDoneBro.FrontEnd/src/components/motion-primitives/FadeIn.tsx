import { usePageTransition } from '@/hooks/usePageTransition'
import { motion } from 'framer-motion'

interface FadeInProps {
	children: React.ReactNode
	delay?: number
	direction?: 'up' | 'down' | 'left' | 'right' | 'none'
	className?: string
}

export function FadeIn({
	children,
	delay = 0,
	direction = 'up',
	className,
}: FadeInProps) {
	const { isReducedMotion } = usePageTransition()

	if (isReducedMotion) {
		return <div className={className}>{children}</div>
	}

	const directionOffset = {
		up: { y: 20 },
		down: { y: -20 },
		left: { x: 20 },
		right: { x: -20 },
		none: {},
	} as const

	const initialState = { 
	opacity: 0, 
	...directionOffset[direction] 
}

const animateState = { 
	opacity: 1, 
	x: 0, 
	y: 0 
}

	return (
		<motion.div
			initial={initialState}
			animate={animateState}
			transition={{
				duration: 0.5,
				delay,
				ease: [0.4, 0.0, 0.2, 1],
			}}
			className={className}
		>
			{children}
		</motion.div>
	)
}
