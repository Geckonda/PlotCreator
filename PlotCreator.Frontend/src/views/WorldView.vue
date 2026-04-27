<script setup lang="ts">
import { computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useWorldsStore } from '@/stores/worlds'
import { useEntitiesStore } from '@/stores/entities'
import { useWorldUiStore } from '@/stores/worldUi'
import WorldTopbar from '@/components/world/WorldTopbar.vue'
import WorldSidebar from '@/components/world/WorldSidebar.vue'
import GridView from '@/components/views/GridView.vue'
import TimelineView from '@/components/views/TimelineView.vue'
import ForceGraph from '@/components/graph/ForceGraph.vue'
import EntityDetailPanel from '@/components/entity/EntityDetailPanel.vue'
import EntityCreateModal from '@/components/entity/EntityCreateModal.vue'
import type { Entity } from '@/types/entity'
import { entityKey } from '@/types/entity'
import type { EntityCreatePayload } from '@/types/api'

const route = useRoute()
const router = useRouter()
const worldsStore = useWorldsStore()
const entitiesStore = useEntitiesStore()
const ui = useWorldUiStore()

const { view, activeType, search, showCreate } = storeToRefs(ui)

const worldId = computed(() => Number(route.params.id))

watch(
  worldId,
  async (id) => {
    if (Number.isNaN(id)) return
    worldsStore.setCurrent(id)
    ui.reset()
    entitiesStore.clear()
    await entitiesStore.fetchForWorld(id)
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
  ui.selectedKey !== null
    ? entitiesStore.byKey.get(ui.selectedKey) ?? null
    : null,
)

function selectEntity(entity: Entity) {
  ui.selectedKey = entityKey(entity.type, entity.id)
}

function goToDetail(entity: Entity) {
  router.push({
    name: 'entity-detail',
    params: {
      id: worldId.value,
      type: entity.type,
      entityId: entity.id,
    },
  })
}

function closeDetail() {
  ui.selectedKey = null
}

function closeCreate() {
  showCreate.value = false
}

async function handleCreate(payload: EntityCreatePayload) {
  try {
    const entity = await entitiesStore.create(worldId.value, payload)
    ui.selectedKey = entityKey(entity.type, entity.id)
    ui.view = 'grid'
    ui.activeType = null
  } catch (err) {
    console.error('Create failed', err)
  } finally {
    closeCreate()
  }
}

async function handleDelete(entity: Entity) {
  try {
    await entitiesStore.remove(entity)
    ui.selectedKey = null
  } catch (err) {
    console.error('Delete failed', err)
  }
}
</script>

<template>
  <div class="world">
    <WorldTopbar />

    <div class="world__body">
      <WorldSidebar />

      <ForceGraph
        v-if="view === 'graph'"
        :world-id="worldId"
        :entities="filteredEntities"
        :relations="entitiesStore.relations"
        :selected-key="ui.selectedKey"
        :filter-type="activeType"
        @select="selectEntity"
        @open-details="goToDetail"
        @deselect="closeDetail"
        @delete-entity="handleDelete"
      />

      <GridView
        v-else-if="view === 'grid'"
        :entities="filteredEntities"
        :relations="entitiesStore.relations"
        :filter-type="activeType"
        @select="goToDetail"
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
      @close="closeCreate"
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
