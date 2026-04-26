import type { EntityType } from '@/types/entity'

export interface EntityTypeConfig {
  label: string
  color: string
  icon: string
  radius: number
}

export const ENTITY_TYPES: Record<EntityType, EntityTypeConfig> = {
  character: { label: 'Персонажи', color: '#4a2c1a', icon: '⚔', radius: 21 },
  location:  { label: 'Локации',   color: '#3d6b4a', icon: '◎', radius: 19 },
  event:     { label: 'События',   color: '#b8860b', icon: '◆', radius: 17 },
  faction:   { label: 'Фракции',   color: '#a02929', icon: '⚜', radius: 19 },
  episode:   { label: 'Эпизоды',   color: '#2c4a7a', icon: '▸', radius: 17 },
  artifact:  { label: 'Артефакты', color: '#c45a14', icon: '✦', radius: 15 },
  lore:      { label: 'Лор',       color: '#6b3a7a', icon: '◈', radius: 15 },
}

export const ENTITY_TYPE_LIST = Object.entries(ENTITY_TYPES) as Array<
  [EntityType, EntityTypeConfig]
>
