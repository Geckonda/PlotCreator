<script setup lang="ts">
import { computed, ref } from 'vue'
import {
  VueFlow,
  MarkerType,
  ConnectionMode,
  type Connection,
  type Edge,
  type Node,
  type NodeDragEvent,
  type EdgeMouseEvent,
  type NodeMouseEvent,
} from '@vue-flow/core'
import CircleNode from './CircleNode.vue'
import EdgePopover from './EdgePopover.vue'
import { useEntityTypesStore } from '@/stores/entityTypes'
import type { GraphDetail, GraphEdgeData, EdgeDirection } from '@/types/graph'

const props = defineProps<{
  graph: GraphDetail
  readOnly?: boolean
}>()

const emit = defineEmits<{
  (e: 'select-entity', entityId: number): void
  (e: 'add-edge', payload: { fromNodeId: number; toNodeId: number; direction: EdgeDirection; label: string | null }): void
  (e: 'update-edge', edgeId: number, payload: { direction: EdgeDirection; label: string | null }): void
  (e: 'remove-edge', edgeId: number): void
  (e: 'update-node-position', nodeId: number, x: number, y: number): void
  (e: 'remove-node', nodeId: number): void
}>()

const types = useEntityTypesStore()

function markersFor(direction: EdgeDirection) {
  return {
    markerStart: direction === 'backward' || direction === 'both' ? { type: MarkerType.ArrowClosed } : undefined,
    markerEnd: direction === 'forward' || direction === 'both' ? { type: MarkerType.ArrowClosed } : undefined,
  }
}

const nodeTypes = { circle: CircleNode }

const nodes = computed<Node[]>(() =>
  props.graph.nodes.map((n) => ({
    id: String(n.id),
    type: 'circle',
    position: { x: n.x, y: n.y },
    data: {
      entityId: n.entityId,
      entityName: n.entityName,
      entityTypeKey: n.entityTypeKey,
    },
  })),
)

const edges = computed<Edge[]>(() =>
  props.graph.edges.map((e) => {
    const m = markersFor(e.direction)
    return {
      id: String(e.id),
      source: String(e.fromNodeId),
      target: String(e.toNodeId),
      label: e.label ?? undefined,
      data: { direction: e.direction, raw: e },
      type: 'default',
      ...m,
      style: {
        stroke: e.direction === 'none' ? 'rgba(101, 67, 33, 0.55)' : '#7a4824',
        strokeWidth: 1.6,
        strokeDasharray: e.direction === 'none' ? '6 5' : undefined,
      },
    }
  }),
)

type Popover =
  | { mode: 'create'; from: number; to: number; x: number; y: number }
  | { mode: 'edit'; edge: GraphEdgeData; x: number; y: number }
  | null

const popover = ref<Popover>(null)
const popoverBusy = ref(false)

function clientCoords(evt: MouseEvent | TouchEvent): { x: number; y: number } {
  if ('clientX' in evt) return { x: evt.clientX, y: evt.clientY }
  const t = evt.touches[0] ?? evt.changedTouches[0]
  return t ? { x: t.clientX, y: t.clientY } : { x: 0, y: 0 }
}

function onConnect(c: Connection) {
  if (props.readOnly) return
  const fromId = Number(c.source)
  const toId = Number(c.target)
  if (Number.isNaN(fromId) || Number.isNaN(toId) || fromId === toId) return
  popover.value = {
    mode: 'create',
    from: fromId,
    to: toId,
    x: window.innerWidth / 2 - 120,
    y: window.innerHeight / 2 - 140,
  }
}

function onEdgeClick(payload: EdgeMouseEvent) {
  if (props.readOnly) return
  const raw = (payload.edge.data as { raw?: GraphEdgeData })?.raw
  if (!raw || raw.id < 0) return
  const pos = clientCoords(payload.event)
  popover.value = { mode: 'edit', edge: raw, x: pos.x, y: pos.y }
}

function onNodeClick(payload: NodeMouseEvent) {
  const data = payload.node.data as { entityId?: number }
  if (data?.entityId) emit('select-entity', data.entityId)
}

function onNodeDragStop(payload: NodeDragEvent) {
  if (props.readOnly) return
  const node = Array.isArray(payload.nodes) ? payload.nodes[0] : payload.node
  if (!node) return
  const id = Number(node.id)
  if (Number.isNaN(id) || id < 0) return
  emit('update-node-position', id, node.position.x, node.position.y)
}

async function popoverSave(payload: { direction: EdgeDirection; label: string | null }) {
  if (!popover.value) return
  popoverBusy.value = true
  try {
    if (popover.value.mode === 'create') {
      emit('add-edge', {
        fromNodeId: popover.value.from,
        toNodeId: popover.value.to,
        direction: payload.direction,
        label: payload.label,
      })
    } else {
      emit('update-edge', popover.value.edge.id, payload)
    }
    popover.value = null
  } finally {
    popoverBusy.value = false
  }
}

async function popoverDelete() {
  if (!popover.value || popover.value.mode !== 'edit') return
  popoverBusy.value = true
  try {
    emit('remove-edge', popover.value.edge.id)
    popover.value = null
  } finally {
    popoverBusy.value = false
  }
}

function popoverCancel() {
  popover.value = null
}

const popoverStyle = computed(() => {
  if (!popover.value) return {}
  const maxX = window.innerWidth - 260
  const maxY = window.innerHeight - 320
  return {
    left: `${Math.min(Math.max(popover.value.x, 12), maxX)}px`,
    top: `${Math.min(Math.max(popover.value.y, 12), maxY)}px`,
  }
})
</script>

<template>
  <div class="canvas">
    <VueFlow
      :nodes="nodes"
      :edges="edges"
      :node-types="nodeTypes"
      :nodes-draggable="!readOnly"
      :nodes-connectable="!readOnly"
      :elements-selectable="true"
      :fit-view-on-init="true"
      :connection-mode="ConnectionMode.Loose"
      :default-edge-options="{ type: 'default' }"
      @connect="onConnect"
      @edge-click="onEdgeClick"
      @node-click="onNodeClick"
      @node-drag-stop="onNodeDragStop"
    />

    <div v-if="popover" class="canvas__pop" :style="popoverStyle">
      <EdgePopover
        v-if="popover.mode === 'create'"
        :initial-direction="'forward'"
        :initial-label="''"
        :busy="popoverBusy"
        @save="popoverSave"
        @cancel="popoverCancel"
      />
      <EdgePopover
        v-else
        :initial-direction="popover.edge.direction"
        :initial-label="popover.edge.label"
        :show-delete="true"
        :busy="popoverBusy"
        @save="popoverSave"
        @delete="popoverDelete"
        @cancel="popoverCancel"
      />
    </div>
  </div>
</template>

<style scoped>
.canvas {
  position: relative;
  flex: 1;
  min-width: 0;
  background: radial-gradient(
      ellipse at 45% 40%,
      rgba(122, 72, 36, 0.06) 0%,
      transparent 65%
    ),
    var(--bg);
}

.canvas :deep(.vue-flow__container),
.canvas :deep(.vue-flow__pane),
.canvas :deep(.vue-flow__viewport) {
  background: transparent;
}

.canvas :deep(.vue-flow__node) {
  background: transparent;
  border: none;
  padding: 0;
  box-shadow: none;
}

.canvas :deep(.vue-flow__edge-path) {
  stroke: #7a4824;
}

.canvas :deep(.vue-flow__edge-text) {
  font-family: var(--font-display);
  font-size: 11px;
  fill: #5d3a1a;
  paint-order: stroke;
  stroke: var(--bg);
  stroke-width: 4px;
  stroke-linecap: round;
  stroke-linejoin: round;
}

.canvas :deep(.vue-flow__edge-textbg) {
  fill: transparent;
}

.canvas__pop {
  position: fixed;
  z-index: 50;
}
</style>
