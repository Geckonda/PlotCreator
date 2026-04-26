import api from './api'
import type {
  RelationDto,
  RelationCreateRequest,
  RelationUpdateRequest,
} from '@/types/api'

export const getRelations = (worldId: number) =>
  api.get<RelationDto[]>(`/worlds/${worldId}/relations`).then((r) => r.data)

export const createRelation = (
  worldId: number,
  body: RelationCreateRequest,
) =>
  api
    .post<RelationDto>(`/worlds/${worldId}/relations`, body)
    .then((r) => r.data)

export const updateRelation = (id: number, body: RelationUpdateRequest) =>
  api.put<RelationDto>(`/relations/${id}`, body).then((r) => r.data)

export const deleteRelation = (id: number) =>
  api.delete<void>(`/relations/${id}`).then(() => undefined)
