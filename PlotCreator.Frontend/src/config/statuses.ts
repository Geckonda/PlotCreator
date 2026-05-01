import type { EntityStatus } from '@/types/entity'

export interface StatusConfig {
  label: string
  color: string
}

export const STATUSES: Record<EntityStatus, StatusConfig> = {
  draft:         { label: 'Черновик', color: '#8b7e5a' },
  'in-progress': { label: 'В работе', color: '#b8860b' },
  complete:      { label: 'Готово',   color: '#3d6b4a' },
}

export const STATUS_LIST = Object.entries(STATUSES) as Array<
  [EntityStatus, StatusConfig]
>
