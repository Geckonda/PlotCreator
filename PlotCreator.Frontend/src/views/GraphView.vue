<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useWorldsStore } from '@/stores/worlds'
import { useEntitiesStore } from '@/stores/entities'
import { useEntityTypesStore } from '@/stores/entityTypes'
import { useGraphsStore } from '@/stores/graphs'
import GraphList from '@/components/graph/GraphList.vue'
import GraphCanvas from '@/components/graph/GraphCanvas.vue'
import EntityPickerPanel from '@/components/graph/EntityPickerPanel.vue'
import EntityDetailPanel from '@/components/entity/EntityDetailPanel.vue'
import type { Entity } from '@/types/entity'
import type { EdgeDirection } from '@/types/graph'

const route = useRoute()
const router = useRouter()
const worldsStore = useWorldsStore()
const entitiesStore = useEntitiesStore()
const entityTypesStore = useEntityTypesStore()
const graphsStore = useGraphsStore()

const worldId = computed(() => Number(route.params.id))
const routeGraphId = computed(() => {
  const v = route.params.graphId
  return v ? Number(v) : null
})

const showPicker = ref(false)
const selectedEntity = ref<Entity | null>(null)
const error = ref<string | null>(null)

const currentGraph = computed(() => graphsStore.current)
const isReadOnly = computed(() => currentGraph.value?.isSystemDefault ?? false)
const excludedEntityIds = computed(() =>
  currentGraph.value ? currentGraph.value.nodes.map((n) => n.entityId) : [],
)

watch(
  worldId,
  async (id) => {
    if (Number.isNaN(id)) return
    error.value = null
    worldsStore.setCurrent(id)
    showPicker.value = false
    selectedEntity.value = null
    graphsStore.clear()
    try {
      await Promise.all([
        entityTypesStore.ensureLoaded(),
        entitiesStore.fetchForWorld(id),
        graphsStore.fetchByWorld(id),
      ])
      await selectInitialGraph()
    } catch (e: unknown) {
      const err = e as any
      // If unauthorized (403), redirect to home
      if (err?.response?.status === 403) {
        router.replace({ name: 'home' })
        return
      }
      error.value = err?.response?.data?.errorForUser || 'Failed to load world'
      console.error('Failed to load world:', e)
    }
  },
  { immediate: true },
)

watch(routeGraphId, async (id) => {
  if (id !== null && id !== currentGraph.value?.id) {
    await loadGraph(id)
  }
})

async function selectInitialGraph() {
  const target = routeGraphId.value
    ?? graphsStore.defaultGraph?.id
    ?? graphsStore.summaries[0]?.id
    ?? null
  if (target !== null) await loadGraph(target)
}

async function loadGraph(id: number) {
  showPicker.value = false
  selectedEntity.value = null
  await graphsStore.fetchOne(id)
  if (Number(route.params.graphId) !== id) {
    router.replace({
      name: 'graph-view',
      params: { id: String(worldId.value), graphId: String(id) },
    })
  }
}

function selectGraph(id: number) {
  loadGraph(id)
}

async function createGraph(body: { name: string; description: string | null }) {
  try {
    const dto = await graphsStore.createGraph(worldId.value, body)
    await loadGraph(dto.id)
  } catch (e) {
    error.value = (e as Error).message
  }
}

async function deleteGraph(id: number) {
  if (!confirm('Удалить этот граф? Действие необратимо.')) return
  try {
    await graphsStore.deleteGraph(id)
    if (currentGraph.value === null) {
      const next = graphsStore.defaultGraph?.id ?? graphsStore.summaries[0]?.id ?? null
      if (next !== null) await loadGraph(next)
    }
  } catch (e) {
    error.value = (e as Error).message
  }
}

async function pickEntity(entity: Entity) {
  if (!currentGraph.value) return
  try {
    const existing = currentGraph.value.nodes.length
    const x = (existing % 6) * 180 + 60
    const y = Math.floor(existing / 6) * 120 + 60
    await graphsStore.addNode(currentGraph.value.id, {
      entityId: entity.id,
      x,
      y,
    })
  } catch (e) {
    error.value = (e as Error).message
  }
}

async function onAddEdge(payload: { fromNodeId: number; toNodeId: number; direction: EdgeDirection; label: string | null }) {
  if (!currentGraph.value) return
  try {
    await graphsStore.addEdge(currentGraph.value.id, payload)
  } catch (e) {
    error.value = (e as Error).message
  }
}

async function onUpdateEdge(edgeId: number, payload: { direction: EdgeDirection; label: string | null }) {
  if (!currentGraph.value) return
  try {
    await graphsStore.updateEdge(currentGraph.value.id, edgeId, payload)
  } catch (e) {
    error.value = (e as Error).message
  }
}

async function onRemoveEdge(edgeId: number) {
  if (!currentGraph.value) return
  try {
    await graphsStore.removeEdge(currentGraph.value.id, edgeId)
  } catch (e) {
    error.value = (e as Error).message
  }
}

async function onUpdateNodePosition(nodeId: number, x: number, y: number) {
  if (!currentGraph.value) return
  try {
    await graphsStore.updateNodePosition(currentGraph.value.id, nodeId, x, y)
  } catch (e) {
    error.value = (e as Error).message
  }
}

function onSelectEntity(entityId: number) {
  selectedEntity.value = entitiesStore.byKey.get(entityId) ?? null
}

function closeDetail() {
  selectedEntity.value = null
}

function backToWorld() {
  router.push({ name: 'world', params: { id: String(worldId.value) } })
}
</script>

<template>
  <div class="gv">
    <header class="gv__bar">
      <button class="gv__back" @click="backToWorld">← В мир</button>
      <span class="gv__sep" />
      <div class="gv__name">
        {{ currentGraph?.name ?? '—' }}
        <span v-if="isReadOnly" class="gv__badge">только для чтения</span>
      </div>
      <div class="gv__spacer" />
      <button
        v-if="currentGraph && !isReadOnly"
        class="gv__add"
        @click="showPicker = !showPicker"
      >
        {{ showPicker ? '× Скрыть' : '+ Добавить сущность' }}
      </button>
    </header>

    <div v-if="error" class="gv__err">{{ error }}</div>

    <div class="gv__body">
      <GraphList
        :graphs="graphsStore.summaries"
        :selected-id="currentGraph?.id ?? null"
        :busy="graphsStore.loadingList"
        @select="selectGraph"
        @create="createGraph"
        @delete="deleteGraph"
      />

      <div v-if="!currentGraph" class="gv__empty">
        <p v-if="graphsStore.loadingCurrent">Загрузка графа…</p>
        <p v-else>Выберите граф слева или создайте новый.</p>
      </div>

      <GraphCanvas
        v-else
        :graph="currentGraph"
        :read-only="isReadOnly"
        @select-entity="onSelectEntity"
        @add-edge="onAddEdge"
        @update-edge="onUpdateEdge"
        @remove-edge="onRemoveEdge"
        @update-node-position="onUpdateNodePosition"
      />

      <EntityPickerPanel
        v-if="showPicker && currentGraph && !isReadOnly"
        :excluded-ids="excludedEntityIds"
        @pick="pickEntity"
        @close="showPicker = false"
      />

      <EntityDetailPanel
        v-if="selectedEntity"
        :entity="selectedEntity"
        :world-id="worldId"
        @close="closeDetail"
        @delete="closeDetail"
      />
    </div>
  </div>
</template>

<style scoped>
.gv {
  position: fixed;
  inset: 0;
  display: flex;
  flex-direction: column;
  background: var(--bg);
}

.gv__bar {
  height: 60px;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 0 18px;
  border-bottom: 1px solid var(--border);
  background: var(--surface);
  z-index: 10;
}

.gv__back {
  color: var(--muted);
  padding: 6px 12px;
  border-radius: 7px;
  font-size: 13px;
  font-family: var(--font-display);
  border: 1px solid transparent;
  letter-spacing: 0.03em;
}

.gv__back:hover {
  color: var(--accent);
  border-color: var(--border);
}

.gv__sep {
  width: 1px;
  height: 22px;
  background: var(--border);
}

.gv__name {
  font-family: var(--font-display);
  font-size: 15px;
  color: var(--text);
  display: flex;
  align-items: center;
  gap: 10px;
}

.gv__badge {
  font-size: 10px;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--muted);
  background: rgba(184, 134, 11, 0.12);
  border: 1px solid rgba(184, 134, 11, 0.4);
  border-radius: 4px;
  padding: 2px 6px;
}

.gv__spacer {
  flex: 1;
}

.gv__add {
  padding: 8px 16px;
  border-radius: 8px;
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  background: linear-gradient(135deg, rgba(122, 72, 36, 0.2), rgba(184, 134, 11, 0.12));
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
}

.gv__add:hover {
  filter: brightness(1.05);
}

.gv__err {
  padding: 8px 18px;
  background: rgba(160, 41, 41, 0.06);
  border-bottom: 1px solid rgba(160, 41, 41, 0.25);
  color: #a02929;
  font-size: 12px;
}

.gv__body {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.gv__empty {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--muted);
  font-size: 14px;
  font-style: italic;
}
</style>
