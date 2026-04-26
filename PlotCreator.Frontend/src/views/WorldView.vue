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
import TimelineView from '@/components/views/TimelineView.vue'
import EntityDetailPanel from '@/components/entity/EntityDetailPanel.vue'
import EntityCreateModal from '@/components/entity/EntityCreateModal.vue'
import type { Entity } from '@/types/entity'

const route = useRoute()
const worldsStore = useWorldsStore()
const entitiesStore = useEntitiesStore()
const ui = useWorldUiStore()

const { view, activeType, search, showCreate } = storeToRefs(ui)

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

const selectedEntity = computed(() =>
  ui.selectedId ? entitiesStore.byId.get(ui.selectedId) ?? null : null,
)

function selectEntity(entity: Entity) {
  ui.selectedId = entity.id
}

function closeDetail() {
  ui.selectedId = null
}

function handleCreate(entity: Entity) {
  entitiesStore.add(entity)
  ui.showCreate = false
  ui.selectedId = entity.id
  ui.view = 'grid'
  ui.activeType = null
}

function handleDelete(entity: Entity) {
  entitiesStore.remove(entity.id)
  ui.selectedId = null
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

      <TimelineView
        v-else-if="view === 'timeline'"
        :entities="filteredEntities"
      />

      <EntityDetailPanel
        v-if="selectedEntity && view !== 'timeline'"
        :entity="selectedEntity"
        @close="closeDetail"
        @delete="handleDelete"
      />
    </div>

    <EntityCreateModal
      v-if="showCreate"
      @close="ui.showCreate = false"
      @save="handleCreate"
    />
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

</style>
