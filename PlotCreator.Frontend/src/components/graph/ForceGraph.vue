<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import type { Entity, EntityKey, EntityType, Relation } from '@/types/entity'
import { entityKey, relationFromKey, relationToKey } from '@/types/entity'
import { ENTITY_TYPES, ENTITY_TYPE_LIST } from '@/config/entityTypes'
import { useElementSize } from '@/composables/useElementSize'
import { useForceSimulation } from '@/composables/useForceSimulation'
import NodeContextMenu from './NodeContextMenu.vue'
import RelationLabelPopover from './RelationLabelPopover.vue'
import ConfirmDialog from '@/components/ui/ConfirmDialog.vue'

const props = defineProps<{
  worldId: number
  entities: Entity[]
  relations: Relation[]
  selectedKey: EntityKey | null
  filterType: EntityType | null
}>()

const emit = defineEmits<{
  (e: 'select', entity: Entity): void
  (e: 'deselect'): void
  (e: 'delete-entity', entity: Entity): void
}>()

const containerRef = ref<HTMLElement | null>(null)
const svgRef = ref<SVGSVGElement | null>(null)
const hovered = ref<EntityKey | null>(null)

const view = ref({ x: 0, y: 0, k: 1 })
const isPanning = ref(false)
const MIN_ZOOM = 0.2
const MAX_ZOOM = 4

const linkSource = ref<EntityKey | null>(null)
const linkPointer = ref<{ x: number; y: number } | null>(null)
const linkHover = ref<EntityKey | null>(null)
const contextMenu = ref<{ entity: Entity; x: number; y: number } | null>(null)
const pendingRelation = ref<{
  from: Entity
  to: Entity
  x: number
  y: number
} | null>(null)
const pendingDelete = ref<Entity | null>(null)

const { width, height } = useElementSize(containerRef)

const entitiesRef = computed(() => props.entities)
const relationsRef = computed(() => props.relations)

const sim = useForceSimulation(entitiesRef, relationsRef, { width, height })
const { positions, tick, dragId, setDragPosition, start } = sim

onMounted(() => start())

const entityByKey = computed(() => {
  const m = new Map<EntityKey, Entity>()
  for (const e of props.entities) m.set(entityKey(e.type, e.id), e)
  return m
})

const activeSet = computed(() => {
  const keys = new Set<EntityKey>()
  for (const e of props.entities) {
    if (!props.filterType || e.type === props.filterType)
      keys.add(entityKey(e.type, e.id))
  }
  return keys
})

const connectedToHover = computed(() => {
  if (hovered.value === null) return null
  const keys = new Set<EntityKey>([hovered.value])
  for (const r of props.relations) {
    const fk = relationFromKey(r)
    const tk = relationToKey(r)
    if (fk === hovered.value || tk === hovered.value) {
      keys.add(fk)
      keys.add(tk)
    }
  }
  return keys
})

const connectedToSelected = computed(() => {
  if (props.selectedKey === null) return null
  const keys = new Set<EntityKey>([props.selectedKey])
  for (const r of props.relations) {
    const fk = relationFromKey(r)
    const tk = relationToKey(r)
    if (fk === props.selectedKey || tk === props.selectedKey) {
      keys.add(fk)
      keys.add(tk)
    }
  }
  return keys
})

interface RenderEdge {
  rel: Relation
  fp: { x: number; y: number }
  tp: { x: number; y: number }
  tp_adj: { x: number; y: number }
  mx: number
  my: number
  lx: number
  ly: number
  fromColor: string
  fromType: EntityType
  lit: boolean
  dimmed: boolean
}

const renderEdges = computed<RenderEdge[]>(() => {
  void tick.value
  const out: RenderEdge[] = []
  for (const rel of props.relations) {
    const fk = relationFromKey(rel)
    const tk = relationToKey(rel)
    const fp = positions[fk]
    const tp = positions[tk]
    if (!fp || !tp) continue
    const aOk = activeSet.value.has(fk)
    const bOk = activeSet.value.has(tk)
    if (!aOk && !bOk) continue
    const isHovLit =
      !!connectedToHover.value &&
      connectedToHover.value.has(fk) &&
      connectedToHover.value.has(tk)
    const isSelLit =
      !!connectedToSelected.value &&
      connectedToSelected.value.has(fk) &&
      connectedToSelected.value.has(tk)
    const lit = isHovLit || isSelLit
    const dimmed =
      !aOk ||
      !bOk ||
      (!!connectedToHover.value && !isHovLit && !isSelLit)
    
    // Calculate adjusted target position to stop at node boundary
    const toEntity = entityByKey.value.get(tk)
    const targetRadius = toEntity ? ENTITY_TYPES[toEntity.type].radius : 12
    const dx = tp.x - fp.x
    const dy = tp.y - fp.y
    const dist = Math.sqrt(dx * dx + dy * dy)
    const tp_adj = dist > 0 ? {
      x: tp.x - (dx / dist) * (targetRadius + 2),
      y: tp.y - (dy / dist) * (targetRadius + 2)
    } : tp
    
    const mx = (fp.x + tp.x) / 2 + (tp.y - fp.y) * 0.18
    const my = (fp.y + tp.y) / 2 - (tp.x - fp.x) * 0.18
    const lx = 0.25 * fp.x + 0.5 * mx + 0.25 * tp.x
    const ly = 0.25 * fp.y + 0.5 * my + 0.25 * tp.y
    const fromEntity = entityByKey.value.get(fk)
    const fromColor = fromEntity
      ? ENTITY_TYPES[fromEntity.type].color
      : '#7a4824'
    const fromType = fromEntity?.type ?? 'Character'
    out.push({ rel, fp, tp: tp_adj, tp_adj, mx, my, lx, ly, fromColor, fromType, lit, dimmed })
  }
  return out
})

interface RenderNode {
  key: EntityKey
  entity: Entity
  cfg: (typeof ENTITY_TYPES)[EntityType]
  x: number
  y: number
  r: number
  active: boolean
  isSel: boolean
  isHov: boolean
  bright: boolean
  dimmed: boolean
}

const renderNodes = computed<RenderNode[]>(() => {
  void tick.value
  const out: RenderNode[] = []
  for (const entity of props.entities) {
    const k = entityKey(entity.type, entity.id)
    const p = positions[k]
    if (!p) continue
    const cfg = ENTITY_TYPES[entity.type]
    const active = activeSet.value.has(k)
    const isSel = props.selectedKey === k
    const isHov = hovered.value === k || linkHover.value === k
    const bright = isSel || isHov
    const dimmed =
      !active ||
      (!!connectedToHover.value &&
        !connectedToHover.value.has(k) &&
        !isSel)
    out.push({
      key: k,
      entity,
      cfg,
      x: p.x,
      y: p.y,
      r: cfg.radius,
      active,
      isSel,
      isHov,
      bright,
      dimmed,
    })
  }
  return out
})

const legendCounts = computed(() => {
  const out: Record<EntityType, number> = {} as Record<EntityType, number>
  for (const [type] of ENTITY_TYPE_LIST) out[type] = 0
  for (const e of props.entities) out[e.type]++
  return out
})

function truncate(name: string): string {
  return name.length > 16 ? name.slice(0, 15) + '…' : name
}

function clientToGraph(clientX: number, clientY: number) {
  const rect = svgRef.value?.getBoundingClientRect()
  if (!rect) return { x: 0, y: 0 }
  const sx = clientX - rect.left
  const sy = clientY - rect.top
  return {
    x: (sx - view.value.x) / view.value.k,
    y: (sy - view.value.y) / view.value.k,
  }
}

function findNodeAt(gx: number, gy: number): EntityKey | null {
  for (const node of renderNodes.value) {
    const dx = gx - node.x
    const dy = gy - node.y
    const r = node.r + 6
    if (dx * dx + dy * dy <= r * r) return node.key
  }
  return null
}

function onNodeMouseDown(evt: MouseEvent, key: EntityKey) {
  evt.stopPropagation()
  if (evt.button === 0) {
    dragId.value = key
    const onMove = (ev: MouseEvent) => {
      const p = clientToGraph(ev.clientX, ev.clientY)
      setDragPosition(key, p.x, p.y)
    }
    const onUp = () => {
      dragId.value = null
      window.removeEventListener('mousemove', onMove)
      window.removeEventListener('mouseup', onUp)
    }
    window.addEventListener('mousemove', onMove)
    window.addEventListener('mouseup', onUp)
    return
  }
  if (evt.button === 2) {
    evt.preventDefault()
    contextMenu.value = null
    pendingRelation.value = null
    const sourceEntity = entityByKey.value.get(key)
    if (!sourceEntity) return
    const startX = evt.clientX
    const startY = evt.clientY
    let moved = false

    const onMove = (ev: MouseEvent) => {
      const dx = ev.clientX - startX
      const dy = ev.clientY - startY
      if (!moved && dx * dx + dy * dy > 16) {
        moved = true
        linkSource.value = key
      }
      if (moved) {
        const p = clientToGraph(ev.clientX, ev.clientY)
        linkPointer.value = p
        const hit = findNodeAt(p.x, p.y)
        linkHover.value = hit && hit !== key ? hit : null
      }
    }

    const cleanup = () => {
      linkSource.value = null
      linkPointer.value = null
      linkHover.value = null
      window.removeEventListener('mousemove', onMove)
      window.removeEventListener('mouseup', onUp)
      window.removeEventListener('keydown', onKey, true)
    }

    const onUp = (ev: MouseEvent) => {
      const targetKey = linkHover.value
      if (!moved) {
        const rect = containerRef.value?.getBoundingClientRect()
        const cx = rect ? ev.clientX - rect.left : ev.clientX
        const cy = rect ? ev.clientY - rect.top : ev.clientY
        contextMenu.value = { entity: sourceEntity, x: cx, y: cy }
      } else if (targetKey) {
        const targetEntity = entityByKey.value.get(targetKey)
        if (targetEntity) {
          const rect = containerRef.value?.getBoundingClientRect()
          const cx = rect ? ev.clientX - rect.left : ev.clientX
          const cy = rect ? ev.clientY - rect.top : ev.clientY
          pendingRelation.value = {
            from: sourceEntity,
            to: targetEntity,
            x: cx,
            y: cy,
          }
        }
      }
      cleanup()
    }

    const onKey = (ev: KeyboardEvent) => {
      if (ev.key === 'Escape') {
        ev.stopPropagation()
        cleanup()
      }
    }

    window.addEventListener('mousemove', onMove)
    window.addEventListener('mouseup', onUp)
    window.addEventListener('keydown', onKey, true)
  }
}

function onContextMenuOpenDetails() {
  if (!contextMenu.value) return
  emit('select', contextMenu.value.entity)
}

function onContextMenuDelete() {
  if (!contextMenu.value) return
  pendingDelete.value = contextMenu.value.entity
}

function onConfirmDelete() {
  if (!pendingDelete.value) return
  emit('delete-entity', pendingDelete.value)
  pendingDelete.value = null
}

function onCancelDelete() {
  pendingDelete.value = null
}

function closeContextMenu() {
  contextMenu.value = null
}

function onRelationCreated() {
  pendingRelation.value = null
}

function onRelationCancel() {
  pendingRelation.value = null
}

watch(view, () => {
  contextMenu.value = null
  pendingRelation.value = null
})

const linkSourcePos = computed(() => {
  if (!linkSource.value) return null
  const p = positions[linkSource.value]
  return p ? { x: p.x, y: p.y } : null
})

const linkSourceColor = computed(() => {
  if (!linkSource.value) return '#7a4824'
  const e = entityByKey.value.get(linkSource.value)
  return e ? ENTITY_TYPES[e.type].color : '#7a4824'
})

function onSvgMouseDown(evt: MouseEvent) {
  if (evt.button !== 0) return
  const startX = evt.clientX
  const startY = evt.clientY
  const baseX = view.value.x
  const baseY = view.value.y
  let moved = false
  const onMove = (ev: MouseEvent) => {
    const dx = ev.clientX - startX
    const dy = ev.clientY - startY
    if (!moved && dx * dx + dy * dy > 9) {
      moved = true
      isPanning.value = true
    }
    if (moved) {
      view.value = { x: baseX + dx, y: baseY + dy, k: view.value.k }
    }
  }
  const onUp = () => {
    window.removeEventListener('mousemove', onMove)
    window.removeEventListener('mouseup', onUp)
    if (moved) {
      isPanning.value = false
    } else {
      emit('deselect')
    }
  }
  window.addEventListener('mousemove', onMove)
  window.addEventListener('mouseup', onUp)
}

function onWheel(evt: WheelEvent) {
  evt.preventDefault()
  const rect = svgRef.value?.getBoundingClientRect()
  if (!rect) return
  const mx = evt.clientX - rect.left
  const my = evt.clientY - rect.top
  const factor = Math.exp(-evt.deltaY * 0.0015)
  const k0 = view.value.k
  const k1 = Math.min(MAX_ZOOM, Math.max(MIN_ZOOM, k0 * factor))
  if (k1 === k0) return
  const x = mx - ((mx - view.value.x) / k0) * k1
  const y = my - ((my - view.value.y) / k0) * k1
  view.value = { x, y, k: k1 }
}

function onNodeClick(evt: MouseEvent, entity: Entity) {
  evt.stopPropagation()
  emit('select', entity)
}

const viewTransform = computed(
  () => `translate(${view.value.x},${view.value.y}) scale(${view.value.k})`
)
</script>

<template>
  <div ref="containerRef" class="graph">
    <svg
      ref="svgRef"
      width="100%"
      height="100%"
      class="graph__svg"
      :class="{
        'graph__svg--grabbing': dragId !== null,
        'graph__svg--panning': isPanning,
      }"
      @mousedown="onSvgMouseDown"
      @wheel="onWheel"
      @contextmenu.prevent
    >
      <defs>
        <radialGradient
          v-for="[type, cfg] in ENTITY_TYPE_LIST"
          :key="type"
          :id="`ng-${type}`"
          cx="50%"
          cy="50%"
          r="50%"
        >
          <stop offset="0%" :stop-color="cfg.color" stop-opacity="0.4" />
          <stop offset="100%" :stop-color="cfg.color" stop-opacity="0.05" />
        </radialGradient>

        <marker
          v-for="[type, cfg] in ENTITY_TYPE_LIST"
          :key="`arrow-${type}`"
          :id="`arrow-${type}`"
          markerWidth="10"
          markerHeight="10"
          refX="8"
          refY="3"
          orient="auto"
          markerUnits="strokeWidth"
        >
          <path d="M0,0 L0,6 L9,3 z" :fill="cfg.color" />
        </marker>
      </defs>

      <g :transform="viewTransform">
      <g
        v-for="edge in renderEdges"
        :key="edge.rel.id"
        :opacity="edge.dimmed ? 0.12 : 1"
        class="graph__edge-group"
      >
        <path
          :d="`M${edge.fp.x},${edge.fp.y} Q${edge.mx},${edge.my} ${edge.tp_adj.x},${edge.tp_adj.y}`"
          fill="none"
          :stroke="edge.lit ? edge.fromColor : 'rgba(101, 67, 33, 0.65)'"
          :stroke-width="edge.lit ? 2.2 : 1.2"
          :stroke-dasharray="edge.lit ? 'none' : '5 5'"
          stroke-linecap="round"
          :marker-end="`url(#arrow-${edge.fromType})`"
        />
        <text
          v-if="!edge.dimmed"
          :x="edge.lx"
          :y="edge.ly"
          text-anchor="middle"
          dominant-baseline="middle"
          :font-size="edge.lit ? 12 : 11"
          :font-weight="edge.lit ? 600 : 500"
          :fill="edge.lit ? edge.fromColor : 'rgba(50, 30, 16, 0.9)'"
          class="graph__edge-label"
        >
          {{ edge.rel.label }}
        </text>
      </g>

      <g
        v-for="node in renderNodes"
        :key="node.key"
        :transform="`translate(${node.x},${node.y})`"
        :opacity="node.dimmed ? 0.18 : 1"
        class="graph__node"
        @mousedown="onNodeMouseDown($event, node.key)"
        @mouseenter="hovered = node.key"
        @mouseleave="hovered = null"
        @click="onNodeClick($event, node.entity)"
      >
        <circle
          v-if="node.bright"
          :r="node.r + 16"
          :fill="`url(#ng-${node.entity.type})`"
          opacity="0.8"
        />

        <circle
          v-if="node.isSel"
          :r="node.r + 9"
          fill="none"
          :stroke="node.cfg.color"
          stroke-width="1.5"
          opacity="0.55"
          stroke-dasharray="4 6"
          class="graph__sel-ring"
        />

        <circle
          :r="node.r + 3"
          fill="none"
          :stroke="node.cfg.color"
          :stroke-width="node.bright ? 1.6 : 0.9"
          :opacity="node.bright ? 0.6 : 0.28"
        />

        <circle
          :r="node.r"
          :fill="node.bright ? node.cfg.color + '38' : node.cfg.color + '1c'"
          :stroke="node.cfg.color"
          :stroke-width="node.bright ? 2.2 : 1.4"
        />

        <text
          text-anchor="middle"
          dominant-baseline="central"
          :font-size="node.r > 18 ? 14 : 12"
          :fill="node.cfg.color"
          :opacity="node.bright ? 1 : 0.85"
          class="graph__node-icon"
        >
          {{ node.cfg.icon }}
        </text>

        <text
          :y="node.r + 18"
          text-anchor="middle"
          :font-size="node.bright ? 13 : 12"
          :fill="node.bright ? 'var(--text)' : 'rgba(58, 36, 20, 0.7)'"
          class="graph__node-name"
        >
          {{ truncate(node.entity.name) }}
        </text>
      </g>

      <line
        v-if="linkSourcePos && linkPointer"
        :x1="linkSourcePos.x"
        :y1="linkSourcePos.y"
        :x2="linkPointer.x"
        :y2="linkPointer.y"
        :stroke="linkSourceColor"
        stroke-width="2"
        stroke-dasharray="6 4"
        stroke-linecap="round"
        opacity="0.85"
        class="graph__draglink"
      />
      </g>
    </svg>

    <NodeContextMenu
      v-if="contextMenu"
      :entity="contextMenu.entity"
      :x="contextMenu.x"
      :y="contextMenu.y"
      @open-details="onContextMenuOpenDetails"
      @delete="onContextMenuDelete"
      @close="closeContextMenu"
    />

    <RelationLabelPopover
      v-if="pendingRelation"
      :world-id="worldId"
      :from="pendingRelation.from"
      :to="pendingRelation.to"
      :x="pendingRelation.x"
      :y="pendingRelation.y"
      @created="onRelationCreated"
      @cancel="onRelationCancel"
    />

    <ConfirmDialog
      v-if="pendingDelete"
      tone="danger"
      title="Удаление"
      :message="`Удалить «${pendingDelete.name}»?`"
      detail="Все связи этой сущности также будут удалены."
      confirm-label="Удалить"
      @confirm="onConfirmDelete"
      @cancel="onCancelDelete"
    />

    <div class="graph__legend">
      <div
        v-for="[type, cfg] in ENTITY_TYPE_LIST"
        :key="type"
        v-show="legendCounts[type] > 0"
        class="legend-item"
        :style="{ borderColor: `${cfg.color}38` }"
      >
        <span class="legend-item__icon" :style="{ color: cfg.color }">
          {{ cfg.icon }}
        </span>
        <span class="legend-item__label" :style="{ color: cfg.color }">
          {{ cfg.label }}
        </span>
        <span class="legend-item__count">{{ legendCounts[type] }}</span>
      </div>
    </div>

    <div class="graph__hint">
      ЛКМ — узел/панорама · Колесо — масштаб · ПКМ — меню или связь
    </div>
  </div>
</template>

<style scoped>
.graph {
  flex: 1;
  position: relative;
  overflow: hidden;
  background: radial-gradient(
      ellipse at 45% 40%,
      rgba(122, 72, 36, 0.06) 0%,
      transparent 65%
    ),
    var(--bg);
}

.graph__svg {
  display: block;
  cursor: grab;
}

.graph__svg--grabbing,
.graph__svg--panning {
  cursor: grabbing;
}

.graph__edge-group {
  transition: opacity 0.2s;
}

.graph__draglink {
  pointer-events: none;
}

.graph__edge-label {
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  pointer-events: none;
  user-select: none;
  paint-order: stroke;
  stroke: var(--bg);
  stroke-width: 4px;
  stroke-linecap: round;
  stroke-linejoin: round;
}

.graph__node {
  cursor: pointer;
  transition: opacity 0.2s;
}

.graph__node-icon,
.graph__node-name {
  pointer-events: none;
  user-select: none;
}

.graph__node-name {
  font-family: var(--font-display);
  letter-spacing: 0.02em;
}

.graph__sel-ring {
  animation: spin 8s linear infinite;
  transform-origin: center;
  transform-box: fill-box;
}

.graph__legend {
  position: absolute;
  bottom: 18px;
  left: 18px;
  display: flex;
  flex-wrap: wrap;
  gap: 7px;
  max-width: 360px;
}

.legend-item {
  display: flex;
  align-items: center;
  gap: 6px;
  background: rgba(246, 236, 211, 0.92);
  border: 1px solid;
  border-radius: 20px;
  padding: 4px 11px;
  font-size: 12px;
  backdrop-filter: blur(6px);
}

.legend-item__icon {
  font-size: 12px;
}

.legend-item__label {
  font-family: var(--font-display);
  opacity: 0.85;
}

.legend-item__count {
  color: var(--dim);
  font-variant-numeric: tabular-nums;
}

.graph__hint {
  position: absolute;
  bottom: 18px;
  right: 18px;
  font-size: 12px;
  color: var(--dim);
  font-style: italic;
}
</style>
