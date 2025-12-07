import tailwindcss from '@tailwindcss/vite'
import react from '@vitejs/plugin-react'
import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'

export default defineConfig({
	plugins: [react(), tailwindcss()],
	resolve: {
		alias: {
			'@': fileURLToPath(new URL('./src', import.meta.url)),
		},
	},
	server: {
		allowedHosts: ['host.docker.internal'],
		host: true,
		proxy: {
			'/api': {
				target: process.env.API_HTTPS || process.env.API_HTTP,
				changeOrigin: true,
			},
		},
	},
})
