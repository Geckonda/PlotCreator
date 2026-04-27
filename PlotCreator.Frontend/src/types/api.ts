import type { Entity, EntityStatus, EntityType, Relation } from './entity'
import type { World } from './world'

// ───── Wire DTOs (match BE Contracts/ shape) ─────

export interface WorldDto {
  id: number
  name: string
  genre: string | null
  description: string | null
  color: string | null
  entitiesCount: number
  createdAt: string
  updatedAt: string
}

export interface WorldCreateRequest {
  name: string
  genre?: string | null
  description?: string | null
  color?: string | null
}

export interface WorldUpdateRequest extends WorldCreateRequest {}

export interface EntitySummaryDto {
  id: number
  type: EntityType
  name: string
  tags: string[]
  status: EntityStatus
  desc?: string | null
}

export interface RelationDto {
  id: number
  worldId: number
  fromId: number
  fromType: EntityType
  toId: number
  toType: EntityType
  label: string
  createdAt: string
}

export interface RelationCreateRequest {
  fromId: number
  fromType: EntityType
  toId: number
  toType: EntityType
  label: string
}

export interface RelationUpdateRequest {
  label: string
}

export interface EntityCreatePayload {
  type: EntityType
  name: string
  tags: string[]
  status: EntityStatus
  desc?: string
}

// ───── Per-type DTOs (returned by GET /api/{type}/{id}) ─────

export interface CharacterDto extends EntitySummaryDto {
  worldId: number
  birthday: string | null
  deathday: string | null
  gender: string | null
  height: number | null
  weight: number | null
  personality: string | null
  appearance: string | null
  conflict: string | null
  goals: string | null
  motivation: string | null
  history: string | null
  pictureUrl: string | null
}

export interface LocationDto extends EntitySummaryDto {
  worldId: number
  region: string | null
  climate: string | null
  pictureUrl: string | null
}

export interface EventDto extends EntitySummaryDto {
  worldId: number
  beginning: string | null
  ending: string | null
  chekhovsGun: boolean
}

export interface FactionDto extends EntitySummaryDto {
  worldId: number
  ideology: string | null
  headquarters: string | null
}

export interface EpisodeDto extends EntitySummaryDto {
  worldId: number
  position: number
  content: string | null
}

export interface ArtifactDto extends EntitySummaryDto {
  worldId: number
  material: string | null
  origin: string | null
  pictureUrl: string | null
}

export interface LoreDto extends EntitySummaryDto {
  worldId: number
  era: string | null
  content: string | null
}

export type FullEntityDto =
  | CharacterDto
  | LocationDto
  | EventDto
  | FactionDto
  | EpisodeDto
  | ArtifactDto
  | LoreDto

// ───── Mappers ─────

export const toWorld = (dto: WorldDto): World => ({
  id: dto.id,
  name: dto.name,
  genre: dto.genre ?? '',
  description: dto.description ?? '',
  color: dto.color ?? '#7a4824',
  entitiesCount: dto.entitiesCount,
  active: true,
})

export const toEntity = (dto: EntitySummaryDto): Entity => ({
  id: dto.id,
  type: dto.type,
  name: dto.name,
  tags: dto.tags,
  status: dto.status,
  desc: dto.desc ?? undefined,
})

export const toRelation = (dto: RelationDto): Relation => ({
  id: dto.id,
  from: dto.fromId,
  to: dto.toId,
  label: dto.label,
})
