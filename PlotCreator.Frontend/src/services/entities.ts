import api from './api'
import type { EntityType } from '@/types/entity'
import type {
  EntitySummaryDto,
  EntityCreatePayload,
  FullEntityDto,
} from '@/types/api'

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

// Per-type DTOs from the BE don't include the `type` discriminator (it's
// implicit in the route). Re-attach it on the FE so consumers always receive
// a complete FullEntityDto.
type WireDto = Omit<FullEntityDto, 'type'>
const withType = (type: EntityType, dto: WireDto): FullEntityDto =>
  ({ ...dto, type } as FullEntityDto)

export const getEntity = (type: EntityType, id: number) =>
  api
    .get<WireDto>(`/${TYPE_PATH[type]}/${id}`)
    .then((r) => withType(type, r.data))

export const createEntity = (worldId: number, payload: EntityCreatePayload) => {
  const { type, ...body } = payload
  return api
    .post<WireDto>(`/worlds/${worldId}/${TYPE_PATH[type]}`, body)
    .then((r) => withType(type, r.data))
}

export const updateEntity = (type: EntityType, id: number, body: object) =>
  api
    .put<WireDto>(`/${TYPE_PATH[type]}/${id}`, body)
    .then((r) => withType(type, r.data))

export const deleteEntity = (type: EntityType, id: number) => {
  const path = TYPE_PATH[type]
  return api.delete<void>(`/${path}/${id}`).then(() => undefined)
}
