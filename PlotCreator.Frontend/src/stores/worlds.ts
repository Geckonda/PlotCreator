import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { World } from '@/types/world'
import type { WorldCreateRequest, WorldUpdateRequest } from '@/types/api'
import { toWorld } from '@/types/api'
import * as api from '@/services/worlds'

export const useWorldsStore = defineStore('worlds', () => {
  const worlds = ref<World[]>([])
  const currentId = ref<number | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const current = computed(
    () => worlds.value.find((w) => w.id === currentId.value) ?? null,
  )

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      const dtos = await api.listWorlds()
      worlds.value = dtos.map(toWorld)
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Failed to load worlds'
    } finally {
      loading.value = false
    }
  }

  async function create(body: WorldCreateRequest) {
    const dto = await api.createWorld(body)
    const w = toWorld(dto)
    worlds.value.push(w)
    return w
  }

  async function update(id: number, body: WorldUpdateRequest) {
    const dto = await api.updateWorld(id, body)
    const w = toWorld(dto)
    const i = worlds.value.findIndex((x) => x.id === id)
    if (i >= 0) worlds.value[i] = w
    return w
  }

  async function remove(id: number) {
    await api.deleteWorld(id)
    worlds.value = worlds.value.filter((w) => w.id !== id)
    if (currentId.value === id) currentId.value = null
  }

  function setCurrent(id: number | null) {
    currentId.value = id
  }

  return {
    worlds,
    currentId,
    current,
    loading,
    error,
    fetchAll,
    create,
    update,
    remove,
    setCurrent,
  }
})
