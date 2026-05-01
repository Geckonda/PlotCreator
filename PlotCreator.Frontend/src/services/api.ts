import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true,
})

let on401: (() => void) | null = null

export function setUnauthorizedHandler(handler: (() => void) | null) {
  on401 = handler
}

api.interceptors.response.use(
  (resp) => resp,
  (error) => {
    const status = error?.response?.status
    if (status === 401) {
      const url: string = error?.config?.url ?? ''
      // Don't fire the handler for the /auth/me probe — that's how we check
      // session state on app boot and a 401 there is expected when logged out.
      if (!url.includes('/auth/me') && on401) {
        on401()
      }
    }
    return Promise.reject(error)
  },
)

export default api
