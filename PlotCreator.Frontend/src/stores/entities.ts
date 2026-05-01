import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Entity, EntityKey, Relation } from '@/types/entity'
import { entityKey, relationFromKey, relationToKey } from '@/types/entity'
import type {
  EntityCreatePayload,
  EntityDto,
  EntityUpdatePayload,
  RelationCreateRequest,
} from '@/types/api'
import { toEntity, toRelation } from '@/types/api'
import * as entitiesApi from '@/services/entities'
import * as relationsApi from '@/services/relations'

export const useEntitiesStore = defineStore('entities', () => {
  const entities = ref<Entity[]>([])
  const relations = ref<Relation[]>([])
  const details = ref<Map<number, EntityDto>>(new Map())
  const loading = ref(false)
  const error = ref<string | null>(null)

  const byKey = computed(() => {
    const m = new Map<EntityKey, Entity>()
    for (const e of entities.value) m.set(entityKey(e.id), e)
    return m
  })

  async function fetchForWorld(worldId: number) {
    loading.value = true
    error.value = null
    try {
      const [eDtos, rDtos] = await Promise.all([
        entitiesApi.getEntities(worldId),
        relationsApi.getRelations(worldId),
      ])
      entities.value = eDtos.map(toEntity)
      relations.value = rDtos.map(toRelation)
      details.value.clear()
    } catch (e: unknown) {
      const err = e as any
      error.value = err?.response?.data?.errorForUser || (e instanceof Error ? e.message : 'Failed to load world data')
      // Re-throw authorization errors so the view can handle them
      if (err?.response?.status === 403) {
        throw e
      }
    } finally {
      loading.value = false
    }
  }

  function clear() {
    entities.value = []
    relations.value = []
    details.value.clear()
  }

  async function create(worldId: number, payload: EntityCreatePayload) {
    const dto = await entitiesApi.createEntity(worldId, payload)
    const entity = toEntity(dto)
    entities.value.push(entity)
    details.value.set(dto.id, dto)
    return entity
  }

  async function fetchDetail(worldId: number, id: number) {
    const cached = details.value.get(id)
    if (cached) return cached
    const dto = await entitiesApi.getEntity(worldId, id)
    details.value.set(id, dto)
    return dto
  }

  async function update(worldId: number, id: number, body: EntityUpdatePayload) {
    const dto = await entitiesApi.updateEntity(worldId, id, body)
    details.value.set(id, dto)
    const i = entities.value.findIndex((e) => e.id === id)
    if (i >= 0) entities.value[i] = toEntity(dto)
    return dto
  }

  async function remove(worldId: number, entity: Entity) {
    await entitiesApi.deleteEntity(worldId, entity.id)
    entities.value = entities.value.filter((e) => e.id !== entity.id)
    relations.value = relations.value.filter(
      (r) => r.from !== entity.id && r.to !== entity.id,
    )
    details.value.delete(entity.id)
  }

  async function createRelation(
    worldId: number,
    body: RelationCreateRequest,
  ) {
    const dto = await relationsApi.createRelation(worldId, body)
    const rel = toRelation(dto)
    relations.value.push(rel)
    return rel
  }

  async function removeRelation(id: number) {
    await relationsApi.deleteRelation(id)
    relations.value = relations.value.filter((r) => r.id !== id)
  }

  function relationsFor(id: number) {
    return relations.value.filter(
      (r) => relationFromKey(r) === id || relationToKey(r) === id,
    )
  }

  return {
    entities,
    relations,
    details,
    byKey,
    loading,
    error,
    fetchForWorld,
    clear,
    create,
    fetchDetail,
    update,
    remove,
    createRelation,
    removeRelation,
    relationsFor,
  }
})
