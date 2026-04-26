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
  id: string
  type: EntityType
  name: string
  tags: string[]
  status: EntityStatus
  desc?: string
}

export interface Relation {
  id: string
  from: string
  to: string
  label: string
}
