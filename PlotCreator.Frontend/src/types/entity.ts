export type EntityType =
  | 'character'
  | 'location'
  | 'event'
  | 'faction'
  | 'episode'
  | 'artifact'
  | 'lore'

export type EntityStatus = 'draft' | 'in-progress' | 'complete'

export interface Entity {
  id: number
  type: EntityType
  name: string
  tags: string[]
  status: EntityStatus
  desc?: string
}

// Per-type tables on the BE each have their own identity column, so
// `id` alone is NOT unique across types. Always key entities by this
// composite when storing them in maps or referencing them by URL/state.
export type EntityKey = string
export const entityKey = (type: EntityType, id: number): EntityKey =>
  `${type}:${id}`

export interface Relation {
  id: number
  from: number
  fromType: EntityType
  to: number
  toType: EntityType
  label: string
}

export const relationFromKey = (r: Relation): EntityKey =>
  entityKey(r.fromType, r.from)
export const relationToKey = (r: Relation): EntityKey =>
  entityKey(r.toType, r.to)
