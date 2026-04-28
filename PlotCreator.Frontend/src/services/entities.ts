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

export const getEntity = (id: number) =>
  api.get<EntityDto>(`/entities/${id}`).then((r) => r.data)

export const createEntity = (worldId: number, payload: EntityCreatePayload) =>
  api
    .post<EntityDto>(`/worlds/${worldId}/entities`, payload)
    .then((r) => r.data)

export const updateEntity = (id: number, body: EntityUpdatePayload) =>
  api.put<EntityDto>(`/entities/${id}`, body).then((r) => r.data)

export const deleteEntity = (id: number) =>
  api.delete<void>(`/entities/${id}`).then(() => undefined)
