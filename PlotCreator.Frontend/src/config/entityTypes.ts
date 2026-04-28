// Fallback config used while the user's type catalog is loading from the API.
// The source of truth is `useEntityTypesStore`.

export interface EntityTypeConfig {
  label: string
  color: string
  icon: string
  radius: number
}

export const FALLBACK_ENTITY_TYPE: EntityTypeConfig = {
  label: '?',
  color: '#7a4824',
  icon: '◯',
  radius: 16,
}

export const SEEDED_DEFAULTS: Record<string, EntityTypeConfig> = {
  character: { label: 'Персонажи', color: '#4a2c1a', icon: '⚔', radius: 21 },
  location:  { label: 'Локации',   color: '#3d6b4a', icon: '◎', radius: 19 },
  event:     { label: 'События',   color: '#b8860b', icon: '◆', radius: 17 },
  faction:   { label: 'Фракции',   color: '#a02929', icon: '⚜', radius: 19 },
  episode:   { label: 'Эпизоды',   color: '#2c4a7a', icon: '▸', radius: 17 },
  artifact:  { label: 'Артефакты', color: '#c45a14', icon: '✦', radius: 15 },
  lore:      { label: 'Лор',       color: '#6b3a7a', icon: '◈', radius: 15 },
}
