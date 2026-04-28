import type { Entity, EntityStatus, Relation } from './entity'
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
  typeKey: string
  name: string
  tags: string[]
  status: EntityStatus
  desc?: string | null
}

export type FieldKind =
  | 'text'
  | 'textarea'
  | 'number'
  | 'date'
  | 'checkbox'
  | 'select'
  | 'image'

export interface PropertyDef {
  key: string
  label: string
  kind: FieldKind
  options?: string[]
  required?: boolean
}

export interface EntityTypeDto {
  id: number
  key: string
  label: string
  color: string | null
  icon: string | null
  radius: number
  propertySchema: PropertyDef[]
  isSystemDefault: boolean
  createdAt: string
  updatedAt: string
}

export interface EntityTypeCreateRequest {
  key: string
  label: string
  color?: string | null
  icon?: string | null
  radius?: number
  propertySchema?: PropertyDef[]
}

export interface EntityTypeUpdateRequest {
  label: string
  color?: string | null
  icon?: string | null
  radius: number
  propertySchema?: PropertyDef[]
}

export type TipTapDoc = {
  type: 'doc'
  content?: unknown[]
}

export const emptyTipTapDoc = (): TipTapDoc => ({ type: 'doc', content: [] })

export interface EntityDto {
  id: number
  worldId: number
  typeKey: string
  name: string
  tags: string[]
  status: EntityStatus
  desc?: string | null
  properties: Record<string, unknown>
  content: TipTapDoc
}

export interface EntityCreatePayload {
  typeKey: string
  name: string
  tags: string[]
  status: EntityStatus
  desc?: string
  properties?: Record<string, unknown>
  content?: TipTapDoc
}

export interface EntityUpdatePayload {
  name: string
  tags: string[]
  status: EntityStatus
  desc?: string | null
  properties?: Record<string, unknown>
  content?: TipTapDoc
}

export interface RelationDto {
  id: number
  worldId: number
  fromId: number
  toId: number
  label: string
  createdAt: string
}

export interface RelationCreateRequest {
  fromId: number
  toId: number
  label: string
}

export interface RelationUpdateRequest {
  label: string
}

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

export const toEntity = (dto: EntitySummaryDto | EntityDto): Entity => ({
  id: dto.id,
  typeKey: dto.typeKey,
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
