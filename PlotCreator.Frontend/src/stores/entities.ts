import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Entity, Relation } from '@/types/entity'
import type {
  EntityCreatePayload,
  RelationCreateRequest,
} from '@/types/api'
import { toEntity, toRelation } from '@/types/api'
import * as entitiesApi from '@/services/entities'
import * as relationsApi from '@/services/relations'

export const useEntitiesStore = defineStore('entities', () => {
  const entities = ref<Entity[]>([])
  const relations = ref<Relation[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const byId = computed(() => {
    const m = new Map<number, Entity>()
    for (const e of entities.value) m.set(e.id, e)
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
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Failed to load world data'
    } finally {
      loading.value = false
    }
  }

  function clear() {
    entities.value = []
    relations.value = []
  }

  async function create(worldId: number, payload: EntityCreatePayload) {
    const dto = await entitiesApi.createEntity(worldId, payload)
    const entity = toEntity(dto)
    entities.value.push(entity)
    return entity
  }

  async function remove(entity: Entity) {
    await entitiesApi.deleteEntity(entity.type, entity.id)
    entities.value = entities.value.filter((e) => e.id !== entity.id)
    relations.value = relations.value.filter(
      (r) => r.from !== entity.id && r.to !== entity.id,
    )
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
    return relations.value.filter((r) => r.from === id || r.to === id)
  }

  return {
    entities,
    relations,
    byId,
    loading,
    error,
    fetchForWorld,
    clear,
    create,
    remove,
    createRelation,
    removeRelation,
    relationsFor,
  }
})
