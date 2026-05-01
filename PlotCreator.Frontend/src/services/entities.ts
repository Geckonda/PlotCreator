import api from './api'
import type {
  EntitySummaryDto,
  EntityDto,
  EntityCreatePayload,
  EntityUpdatePayload,
} from '@/types/api'

export const getEntities = (worldId: number) =>
  api
    .get<EntitySummaryDto[]>(`/worlds/${worldId}/entities`)
    .then((r) => r.data)

export const getEntity = (worldId: number, id: number) =>
  api.get<EntityDto>(`/worlds/${worldId}/entities/${id}`).then((r) => r.data)

export const createEntity = (worldId: number, payload: EntityCreatePayload) =>
  api
    .post<EntityDto>(`/worlds/${worldId}/entities`, payload)
    .then((r) => r.data)

export const updateEntity = (worldId: number, id: number, body: EntityUpdatePayload) =>
  api.put<EntityDto>(`/worlds/${worldId}/entities/${id}`, body).then((r) => r.data)

export const deleteEntity = (worldId: number, id: number) =>
  api.delete<void>(`/worlds/${worldId}/entities/${id}`).then(() => undefined)
