import api from './api'
import type {
  EntityTypeDto,
  EntityTypeCreateRequest,
  EntityTypeUpdateRequest,
} from '@/types/api'

export const getEntityTypes = () =>
  api.get<EntityTypeDto[]>('/users/me/entity-types').then((r) => r.data)

export const createEntityType = (body: EntityTypeCreateRequest) =>
  api
    .post<EntityTypeDto>('/users/me/entity-types', body)
    .then((r) => r.data)

export const updateEntityType = (id: number, body: EntityTypeUpdateRequest) =>
  api
    .put<EntityTypeDto>(`/users/me/entity-types/${id}`, body)
    .then((r) => r.data)

export const deleteEntityType = (id: number) =>
  api.delete<void>(`/users/me/entity-types/${id}`).then(() => undefined)
