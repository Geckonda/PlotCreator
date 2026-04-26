<script setup lang="ts">
import { computed, watch } from 'vue'
import { useRoute } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useWorldsStore } from '@/stores/worlds'
import { useEntitiesStore } from '@/stores/entities'
import { useWorldUiStore } from '@/stores/worldUi'
import WorldTopbar from '@/components/world/WorldTopbar.vue'
import WorldSidebar from '@/components/world/WorldSidebar.vue'
import GridView from '@/components/views/GridView.vue'
import type { Entity } from '@/types/entity'

const route = useRoute()
const worldsStore = useWorldsStore()
const entitiesStore = useEntitiesStore()
const ui = useWorldUiStore()

const { view, activeType, search } = storeToRefs(ui)

const worldId = computed(() => String(route.params.id))

watch(
  worldId,
  (id) => {
    worldsStore.setCurrent(id)
    ui.reset()
  },
  { immediate: true },
)

const filteredEntities = computed(() => {
  const q = search.value.trim().toLowerCase()
  if (!q) return entitiesStore.entities
  return entitiesStore.entities.filter(
    (e) =>
      e.name.toLowerCase().includes(q) ||
      e.tags.some((t) => t.toLowerCase().includes(q)),
  )
})

function selectEntity(entity: Entity) {
  ui.selectedId = entity.id
}
</script>

<template>
  <div class="world">
    <WorldTopbar />

    <div class="world__body">
      <WorldSidebar />

      <GridView
        v-if="view === 'grid' || view === 'graph'"
        :entities="filteredEntities"
        :relations="entitiesStore.relations"
        :filter-type="activeType"
        @select="selectEntity"
      />

      <div v-else-if="view === 'timeline'" class="world__placeholder">
        Хронология (step 4)
      </div>
    </div>
  </div>
</template>

<style scoped>
.world {
  position: fixed;
  inset: 0;
  display: flex;
  flex-direction: column;
  background: var(--bg);
}

.world__body {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.world__placeholder {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--dim);
  font-style: italic;
}
</style>
