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

export interface Relation {
  id: number
  from: number
  to: number
  label: string
}
