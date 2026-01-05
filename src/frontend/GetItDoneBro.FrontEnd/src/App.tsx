import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { Toaster } from 'sonner'
import { ProjectsProvider } from './contexts/ProjectsContext'
import { ThemeProvider } from './contexts/ThemeContext'
import { Layout } from './layouts/Layout'
import { Dashboard } from './pages/Dashboard'
import { ProjectDetailsPage } from './pages/ProjectDetailsPage'
import { ProjectsPage } from './pages/ProjectsPage'

function App() {
	console.log(1)
	return (
		<ThemeProvider>
			<ProjectsProvider>
				<BrowserRouter>
					<Routes>
						<Route path="/" element={<Layout />}>
							<Route index element={<Dashboard />} />
							<Route path="projects" element={<ProjectsPage />} />
							<Route
								path="projects/:id"
								element={<ProjectDetailsPage />}
							/>
						</Route>
					</Routes>
				</BrowserRouter>
				<Toaster position="top-right" richColors />
			</ProjectsProvider>
		</ThemeProvider>
	)
}

export default App
