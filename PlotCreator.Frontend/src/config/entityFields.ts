import type { EntityType } from '@/types/entity'

export type FieldKind = 'text' | 'textarea' | 'number' | 'date' | 'checkbox'

export interface FieldDef {
  key: string
  label: string
  kind: FieldKind
}

export const ENTITY_FIELD_SCHEMAS: Record<EntityType, FieldDef[]> = {
  character: [
    { key: 'birthday', label: 'Рождение', kind: 'date' },
    { key: 'deathday', label: 'Смерть', kind: 'date' },
    { key: 'gender', label: 'Пол', kind: 'text' },
    { key: 'height', label: 'Рост (см)', kind: 'number' },
    { key: 'weight', label: 'Вес (кг)', kind: 'number' },
    { key: 'personality', label: 'Характер', kind: 'textarea' },
    { key: 'appearance', label: 'Внешность', kind: 'textarea' },
    { key: 'goals', label: 'Цели', kind: 'textarea' },
    { key: 'motivation', label: 'Мотивация', kind: 'textarea' },
    { key: 'conflict', label: 'Конфликт', kind: 'textarea' },
    { key: 'history', label: 'История', kind: 'textarea' },
    { key: 'pictureUrl', label: 'Изображение (URL)', kind: 'text' },
  ],
  location: [
    { key: 'region', label: 'Регион', kind: 'text' },
    { key: 'climate', label: 'Климат', kind: 'text' },
    { key: 'pictureUrl', label: 'Изображение (URL)', kind: 'text' },
  ],
  event: [
    { key: 'beginning', label: 'Начало', kind: 'date' },
    { key: 'ending', label: 'Конец', kind: 'date' },
    { key: 'chekhovsGun', label: 'Ружьё Чехова', kind: 'checkbox' },
  ],
  faction: [
    { key: 'ideology', label: 'Идеология', kind: 'textarea' },
    { key: 'headquarters', label: 'Штаб', kind: 'text' },
  ],
  episode: [
    { key: 'position', label: 'Позиция', kind: 'number' },
    { key: 'content', label: 'Содержание', kind: 'textarea' },
  ],
  artifact: [
    { key: 'material', label: 'Материал', kind: 'text' },
    { key: 'origin', label: 'Происхождение', kind: 'textarea' },
    { key: 'pictureUrl', label: 'Изображение (URL)', kind: 'text' },
  ],
  lore: [
    { key: 'era', label: 'Эпоха', kind: 'text' },
    { key: 'content', label: 'Текст', kind: 'textarea' },
  ],
}
