import api from './api'
import type { AuthUser, LoginRequest, RegisterRequest } from '@/types/auth'

export const login = (body: LoginRequest) =>
  api.post<AuthUser>('/auth/login', body).then((r) => r.data)

export const register = (body: RegisterRequest) =>
  api.post<AuthUser>('/auth/register', body).then((r) => r.data)

export const logout = () =>
  api.post<{ ok: boolean }>('/auth/logout').then((r) => r.data)

export const me = () =>
  api.get<AuthUser>('/auth/me').then((r) => r.data)
