import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { EntityType } from '@/types/entity'

export type WorldViewMode = 'graph' | 'grid' | 'timeline'

export const useWorldUiStore = defineStore('worldUi', () => {
  const view = ref<WorldViewMode>('graph')
  const activeType = ref<EntityType | null>(null)
  const selectedId = ref<string | null>(null)
  const search = ref('')
  const showCreate = ref(false)

  function reset() {
    view.value = 'graph'
    activeType.value = null
    selectedId.value = null
    search.value = ''
    showCreate.value = false
  }

  return { view, activeType, selectedId, search, showCreate, reset }
})
