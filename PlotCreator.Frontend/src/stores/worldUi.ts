import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { EntityKey } from '@/types/entity'

export type WorldViewMode = 'graph' | 'grid' | 'timeline'

export const useWorldUiStore = defineStore('worldUi', () => {
  const view = ref<WorldViewMode>('graph')
  const activeType = ref<string | null>(null)
  const selectedKey = ref<EntityKey | null>(null)
  const search = ref('')
  const searchIncludeRelated = ref(false)
  const showCreate = ref(false)

  function reset() {
    view.value = 'graph'
    activeType.value = null
    selectedKey.value = null
    search.value = ''
    searchIncludeRelated.value = false
    showCreate.value = false
  }

  return { view, activeType, selectedKey, search, searchIncludeRelated, showCreate, reset }
})
