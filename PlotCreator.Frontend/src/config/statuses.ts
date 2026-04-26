import type { EntityStatus } from '@/types/entity'

export interface StatusConfig {
  label: string
  color: string
}

export const STATUSES: Record<EntityStatus, StatusConfig> = {
  draft:         { label: 'Черновик', color: '#5a4e78' },
  'in-progress': { label: 'В работе', color: '#fbbf24' },
  complete:      { label: 'Готово',   color: '#34d399' },
}

export const STATUS_LIST = Object.entries(STATUSES) as Array<
  [EntityStatus, StatusConfig]
>
