import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { EntityKey, EntityType } from '@/types/entity'

export type WorldViewMode = 'graph' | 'grid' | 'timeline'

export const useWorldUiStore = defineStore('worldUi', () => {
  const view = ref<WorldViewMode>('graph')
  const activeType = ref<EntityType | null>(null)
  const selectedKey = ref<EntityKey | null>(null)
  const search = ref('')
  const showCreate = ref(false)

  function reset() {
    view.value = 'graph'
    activeType.value = null
    selectedKey.value = null
    search.value = ''
    showCreate.value = false
  }

  return { view, activeType, selectedKey, search, showCreate, reset }
})
