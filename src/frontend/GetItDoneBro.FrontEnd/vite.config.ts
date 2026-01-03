import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite'
import { fileURLToPath, URL } from 'node:url'
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');
    const keycloakUrl = process.env.services__keycloak__https__0 ||
        process.env.services__keycloak__http__0 ||
        env.VITE_KEYCLOAK_URL;

    if (!keycloakUrl) {
        throw new Error(
            'Keycloak URL is not configured. Please set one of the following environment variables: ' +
            '"services__keycloak__https__0", "services__keycloak__http__0", or "VITE_KEYCLOAK_URL".'
        );
    }
    return {
        plugins: [react(),tailwindcss()],
        define: {
            'import.meta.env.VITE_KEYCLOAK_URL': JSON.stringify(keycloakUrl),
        },
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url)),
            },
        },
        server: {
            port: env.VITE_PORT ? parseInt(env.VITE_PORT) : 5173,
            proxy: {
                '/api': {
                    target: process.env.services__api__https__0 ||
                        process.env.services__api__http__0,
                    changeOrigin: true,
                    rewrite: (path) => path.replace(/^\/api/, ''),
                    secure: true,
                },
                '/auth': {
                    target: process.env.services__keycloak__https__0 ||
                        process.env.services__keycloak__http__0,
                    changeOrigin: true,
                    secure: false,
                }
            }
        },
        build: {
            outDir: 'dist',
            rollupOptions: {
                input: './index.html'
            }
        }
    }
})