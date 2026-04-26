import api from './api'
import type { EntityType } from '@/types/entity'
import type { EntitySummaryDto, EntityCreatePayload } from '@/types/api'

const TYPE_PATH: Record<EntityType, string> = {
  character: 'characters',
  location: 'locations',
  event: 'events',
  faction: 'factions',
  episode: 'episodes',
  artifact: 'artifacts',
  lore: 'lores',
}

export const getEntities = (worldId: number) =>
  api
    .get<EntitySummaryDto[]>(`/worlds/${worldId}/entities`)
    .then((r) => r.data)

export const createEntity = (worldId: number, payload: EntityCreatePayload) => {
  const path = TYPE_PATH[payload.type]
  const { type: _t, ...body } = payload
  return api
    .post<EntitySummaryDto>(`/worlds/${worldId}/${path}`, body)
    .then((r) => r.data)
}

export const deleteEntity = (type: EntityType, id: number) => {
  const path = TYPE_PATH[type]
  return api.delete<void>(`/${path}/${id}`).then(() => undefined)
}
