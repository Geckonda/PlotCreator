import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { World } from '@/types/world'
import { SEED_WORLDS } from '@/data/seed'

export const useWorldsStore = defineStore('worlds', () => {
  const worlds = ref<World[]>(SEED_WORLDS)
  const currentId = ref<string | null>(null)

  const current = computed(() =>
    worlds.value.find((w) => w.id === currentId.value) ?? null,
  )

  function setCurrent(id: string | null) {
    currentId.value = id
  }

  return { worlds, currentId, current, setCurrent }
})
