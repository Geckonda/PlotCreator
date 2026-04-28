import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import type { EntityTypeDto, PropertyDef } from '@/types/api'
import * as api from '@/services/entityTypes'
import {
  FALLBACK_ENTITY_TYPE,
  SEEDED_DEFAULTS,
  type EntityTypeConfig,
} from '@/config/entityTypes'

export interface DisplayConfig extends EntityTypeConfig {}

export const useEntityTypesStore = defineStore('entityTypes', () => {
  const types = ref<EntityTypeDto[]>([])
  const loading = ref(false)
  const loaded = ref(false)
  const error = ref<string | null>(null)

  let inflight: Promise<EntityTypeDto[]> | null = null

  async function ensureLoaded() {
    if (loaded.value) return types.value
    if (inflight) return inflight
    loading.value = true
    error.value = null
    inflight = api
      .getEntityTypes()
      .then((list) => {
        types.value = list
        loaded.value = true
        return list
      })
      .catch((e: unknown) => {
        error.value = e instanceof Error ? e.message : 'Failed to load entity types'
        throw e
      })
      .finally(() => {
        loading.value = false
        inflight = null
      })
    return inflight
  }

  async function refresh() {
    loaded.value = false
    return ensureLoaded()
  }

  const byKey = computed<Record<string, EntityTypeDto>>(() => {
    const m: Record<string, EntityTypeDto> = {}
    for (const t of types.value) m[t.key] = t
    return m
  })

  function display(typeKey: string): DisplayConfig {
    const t = byKey.value[typeKey]
    if (t) {
      return {
        label: t.label,
        color: t.color ?? FALLBACK_ENTITY_TYPE.color,
        icon: t.icon ?? FALLBACK_ENTITY_TYPE.icon,
        radius: t.radius,
      }
    }
    return SEEDED_DEFAULTS[typeKey] ?? FALLBACK_ENTITY_TYPE
  }

  function schemaFor(typeKey: string): PropertyDef[] {
    return byKey.value[typeKey]?.propertySchema ?? []
  }

  function clear() {
    types.value = []
    loaded.value = false
    error.value = null
  }

  async function create(req: import('@/types/api').EntityTypeCreateRequest) {
    const dto = await api.createEntityType(req)
    types.value.push(dto)
    return dto
  }

  async function update(
    id: number,
    req: import('@/types/api').EntityTypeUpdateRequest,
  ) {
    const dto = await api.updateEntityType(id, req)
    const i = types.value.findIndex((t) => t.id === id)
    if (i >= 0) types.value[i] = dto
    return dto
  }

  async function remove(id: number) {
    await api.deleteEntityType(id)
    types.value = types.value.filter((t) => t.id !== id)
  }

  return {
    types,
    loading,
    loaded,
    error,
    byKey,
    ensureLoaded,
    refresh,
    display,
    schemaFor,
    clear,
    create,
    update,
    remove,
  }
})
