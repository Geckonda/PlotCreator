import type { EntityType } from '@/types/entity'

export interface EntityTypeConfig {
  label: string
  color: string
  icon: string
  radius: number
}

export const ENTITY_TYPES: Record<EntityType, EntityTypeConfig> = {
  character: { label: 'Персонажи', color: '#a78bfa', icon: '⚔', radius: 21 },
  location:  { label: 'Локации',   color: '#34d399', icon: '◎', radius: 19 },
  event:     { label: 'События',   color: '#fbbf24', icon: '◆', radius: 17 },
  faction:   { label: 'Фракции',   color: '#f87171', icon: '⚜', radius: 19 },
  episode:   { label: 'Эпизоды',   color: '#60a5fa', icon: '▸', radius: 17 },
  artifact:  { label: 'Артефакты', color: '#fb923c', icon: '✦', radius: 15 },
  lore:      { label: 'Лор',       color: '#e879f9', icon: '◈', radius: 15 },
}

export const ENTITY_TYPE_LIST = Object.entries(ENTITY_TYPES) as Array<
  [EntityType, EntityTypeConfig]
>
