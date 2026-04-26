<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import type { Entity, EntityType, Relation } from '@/types/entity'
import { ENTITY_TYPES, ENTITY_TYPE_LIST } from '@/config/entityTypes'
import { useElementSize } from '@/composables/useElementSize'
import { useForceSimulation } from '@/composables/useForceSimulation'

const props = defineProps<{
  entities: Entity[]
  relations: Relation[]
  selectedId: string | null
  filterType: EntityType | null
}>()

const emit = defineEmits<{
  (e: 'select', entity: Entity): void
  (e: 'deselect'): void
}>()

const containerRef = ref<HTMLElement | null>(null)
const svgRef = ref<SVGSVGElement | null>(null)
const hovered = ref<string | null>(null)

const { width, height } = useElementSize(containerRef)

const entitiesRef = computed(() => props.entities)
const relationsRef = computed(() => props.relations)

const sim = useForceSimulation(entitiesRef, relationsRef, { width, height })
const { positions, tick, dragId, setDragPosition, start } = sim

onMounted(() => start())

const entityById = computed(() => {
  const m = new Map<string, Entity>()
  for (const e of props.entities) m.set(e.id, e)
  return m
})

const activeSet = computed(() => {
  const ids = new Set<string>()
  for (const e of props.entities) {
    if (!props.filterType || e.type === props.filterType) ids.add(e.id)
  }
  return ids
})

const connectedToHover = computed(() => {
  if (!hovered.value) return null
  const ids = new Set<string>([hovered.value])
  for (const r of props.relations) {
    if (r.from === hovered.value || r.to === hovered.value) {
      ids.add(r.from)
      ids.add(r.to)
    }
  }
  return ids
})

const connectedToSelected = computed(() => {
  if (!props.selectedId) return null
  const ids = new Set<string>([props.selectedId])
  for (const r of props.relations) {
    if (r.from === props.selectedId || r.to === props.selectedId) {
      ids.add(r.from)
      ids.add(r.to)
    }
  }
  return ids
})

interface RenderEdge {
  rel: Relation
  fp: { x: number; y: number }
  tp: { x: number; y: number }
  mx: number
  my: number
  lx: number
  ly: number
  fromColor: string
  lit: boolean
  dimmed: boolean
}

const renderEdges = computed<RenderEdge[]>(() => {
  void tick.value
  const out: RenderEdge[] = []
  for (const rel of props.relations) {
    const fp = positions[rel.from]
    const tp = positions[rel.to]
    if (!fp || !tp) continue
    const aOk = activeSet.value.has(rel.from)
    const bOk = activeSet.value.has(rel.to)
    if (!aOk && !bOk) continue
    const isHovLit =
      !!connectedToHover.value &&
      connectedToHover.value.has(rel.from) &&
      connectedToHover.value.has(rel.to)
    const isSelLit =
      !!connectedToSelected.value &&
      connectedToSelected.value.has(rel.from) &&
      connectedToSelected.value.has(rel.to)
    const lit = isHovLit || isSelLit
    const dimmed =
      !aOk ||
      !bOk ||
      (!!connectedToHover.value && !isHovLit && !isSelLit)
    const mx = (fp.x + tp.x) / 2 + (tp.y - fp.y) * 0.18
    const my = (fp.y + tp.y) / 2 - (tp.x - fp.x) * 0.18
    const lx = 0.25 * fp.x + 0.5 * mx + 0.25 * tp.x
    const ly = 0.25 * fp.y + 0.5 * my + 0.25 * tp.y
    const fromEntity = entityById.value.get(rel.from)
    const fromColor = fromEntity
      ? ENTITY_TYPES[fromEntity.type].color
      : '#7a4824'
    out.push({ rel, fp, tp, mx, my, lx, ly, fromColor, lit, dimmed })
  }
  return out
})

interface RenderNode {
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
    const p = positions[entity.id]
    if (!p) continue
    const cfg = ENTITY_TYPES[entity.type]
    const active = activeSet.value.has(entity.id)
    const isSel = props.selectedId === entity.id
    const isHov = hovered.value === entity.id
    const bright = isSel || isHov
    const dimmed =
      !active ||
      (!!connectedToHover.value &&
        !connectedToHover.value.has(entity.id) &&
        !isSel)
    out.push({
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

function onNodeMouseDown(evt: MouseEvent, id: string) {
  evt.stopPropagation()
  dragId.value = id
  const onMove = (ev: MouseEvent) => {
    const rect = svgRef.value?.getBoundingClientRect()
    if (!rect) return
    setDragPosition(id, ev.clientX - rect.left, ev.clientY - rect.top)
  }
  const onUp = () => {
    dragId.value = null
    window.removeEventListener('mousemove', onMove)
    window.removeEventListener('mouseup', onUp)
  }
  window.addEventListener('mousemove', onMove)
  window.addEventListener('mouseup', onUp)
}

function onBackgroundClick() {
  emit('deselect')
}

function onNodeClick(evt: MouseEvent, entity: Entity) {
  evt.stopPropagation()
  emit('select', entity)
}
</script>

<template>
  <div ref="containerRef" class="graph">
    <svg
      ref="svgRef"
      width="100%"
      height="100%"
      class="graph__svg"
      :class="{ 'graph__svg--grabbing': dragId !== null }"
      @click="onBackgroundClick"
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
      </defs>

      <g
        v-for="edge in renderEdges"
        :key="edge.rel.id"
        :opacity="edge.dimmed ? 0.08 : edge.lit ? 1 : 0.28"
        class="graph__edge-group"
      >
        <path
          :d="`M${edge.fp.x},${edge.fp.y} Q${edge.mx},${edge.my} ${edge.tp.x},${edge.tp.y}`"
          fill="none"
          :stroke="edge.lit ? edge.fromColor : 'rgba(101, 67, 33, 0.55)'"
          :stroke-width="edge.lit ? 2 : 1.1"
          :stroke-dasharray="edge.lit ? 'none' : '5 5'"
        />
        <text
          v-if="edge.lit"
          :x="edge.lx"
          :y="edge.ly"
          text-anchor="middle"
          dominant-baseline="middle"
          font-size="11"
          fill="rgba(58, 36, 20, 0.78)"
          class="graph__edge-label"
        >
          {{ edge.rel.label }}
        </text>
      </g>

      <g
        v-for="node in renderNodes"
        :key="node.entity.id"
        :transform="`translate(${node.x},${node.y})`"
        :opacity="node.dimmed ? 0.18 : 1"
        class="graph__node"
        @mousedown="onNodeMouseDown($event, node.entity.id)"
        @mouseenter="hovered = node.entity.id"
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
    </svg>

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
      Перетащите узел · Клик — подробности
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
}

.graph__svg--grabbing {
  cursor: grabbing;
}

.graph__edge-group {
  transition: opacity 0.2s;
}

.graph__edge-label {
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  pointer-events: none;
  user-select: none;
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
