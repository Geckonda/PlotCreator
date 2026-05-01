// Default type keys seeded into every user's catalog. Users may add custom
// keys; treat `typeKey` as an open string, not this union.
export type DefaultTypeKey =
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
  typeKey: string
  name: string
  tags: string[]
  aliases: string[]
  status: EntityStatus
  desc?: string
}

// Entity ids are globally unique on the unified `Entities` table — no
// composite key needed. Aliases and helpers kept so call sites that still
// pass through `entityKey(...)` continue to work.
export type EntityKey = number
export const entityKey = (id: number): EntityKey => id

export interface Relation {
  id: number
  from: number
  to: number
  label: string | null
}

export const relationFromKey = (r: Relation): EntityKey => r.from
export const relationToKey = (r: Relation): EntityKey => r.to
