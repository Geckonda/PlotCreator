import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Entity, Relation } from '@/types/entity'
import { SEED_ENTITIES, SEED_RELATIONS } from '@/data/seed'

export const useEntitiesStore = defineStore('entities', () => {
  const entities = ref<Entity[]>(SEED_ENTITIES)
  const relations = ref<Relation[]>(SEED_RELATIONS)

  const byId = computed(() => {
    const m = new Map<string, Entity>()
    for (const e of entities.value) m.set(e.id, e)
    return m
  })

  function add(entity: Entity) {
    entities.value.push(entity)
  }

  function update(id: string, patch: Partial<Entity>) {
    const i = entities.value.findIndex((e) => e.id === id)
    if (i >= 0) entities.value[i] = { ...entities.value[i], ...patch }
  }

  function remove(id: string) {
    entities.value = entities.value.filter((e) => e.id !== id)
    relations.value = relations.value.filter(
      (r) => r.from !== id && r.to !== id,
    )
  }

  function relationsFor(id: string) {
    return relations.value.filter((r) => r.from === id || r.to === id)
  }

  return { entities, relations, byId, add, update, remove, relationsFor }
})
