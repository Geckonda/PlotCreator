import api from './api'
import type {
  WorldDto,
  WorldCreateRequest,
  WorldUpdateRequest,
} from '@/types/api'

export const listWorlds = () =>
  api.get<WorldDto[]>('/worlds').then((r) => r.data)

export const getWorld = (id: number) =>
  api.get<WorldDto>(`/worlds/${id}`).then((r) => r.data)

export const createWorld = (body: WorldCreateRequest) =>
  api.post<WorldDto>('/worlds', body).then((r) => r.data)

export const updateWorld = (id: number, body: WorldUpdateRequest) =>
  api.put<WorldDto>(`/worlds/${id}`, body).then((r) => r.data)

export const deleteWorld = (id: number) =>
  api.delete<void>(`/worlds/${id}`).then(() => undefined)
