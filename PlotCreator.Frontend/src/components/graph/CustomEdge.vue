<script setup lang="ts">
import { computed } from 'vue'
import type { EdgeProps } from '@vue-flow/core'
import type { EdgeDirection } from '@/types/graph'

interface EdgeData {
  direction: EdgeDirection
  raw?: {
    id: number
    fromNodeId: number
    toNodeId: number
    direction: EdgeDirection
    label: string | null
  }
  fromRadius?: number
  toRadius?: number
  fromWrapperSize?: number
  toWrapperSize?: number
}

const props = defineProps<EdgeProps<EdgeData>>()

const direction = computed((): EdgeDirection => props.data?.direction ?? 'forward')

// Handle на Position.Top смещает координаты на половину высоты контейнера
// Вычисляем центр узла (sourceX/Y указывает на верхнюю часть контейнера)
const sourceCenter = computed(() => ({
  x: props.sourceX,
  y: props.sourceY + (props.data?.fromWrapperSize ?? 28) / 2,
}))

const targetCenter = computed(() => ({
  x: props.targetX,
  y: props.targetY + (props.data?.toWrapperSize ?? 28) / 2,
}))

// Радиусы узлов из data (с небольшим запасом)
const sourceRadius = computed(() => (props.data?.fromRadius ?? 14) + 3)
const targetRadius = computed(() => (props.data?.toRadius ?? 14) + 3)

// Вектор от source к target (между центрами)
const vector = computed(() => {
  const dx = targetCenter.value.x - sourceCenter.value.x
  const dy = targetCenter.value.y - sourceCenter.value.y
  const dist = Math.sqrt(dx * dx + dy * dy)
  if (dist === 0) return { dx: 0, dy: 0, dist: 0, unitX: 0, unitY: 0 }
  return {
    dx,
    dy,
    dist,
    unitX: dx / dist,
    unitY: dy / dist,
  }
})

// Отодвигаем начальную точку от центра source на его радиус (в направлении target)
const sourceAdjusted = computed(() => ({
  x: sourceCenter.value.x + vector.value.unitX * sourceRadius.value,
  y: sourceCenter.value.y + vector.value.unitY * sourceRadius.value,
}))

// Отодвигаем конечную точку от центра target на его радиус (назад от source)
const targetAdjusted = computed(() => ({
  x: targetCenter.value.x - vector.value.unitX * targetRadius.value,
  y: targetCenter.value.y - vector.value.unitY * targetRadius.value,
}))

// Контрольная точка для красивой кривой Bezier
// Смещаем перпендикулярно к линии связи
const controlPoint = computed(() => {
  const midX = (sourceAdjusted.value.x + targetAdjusted.value.x) / 2
  const midY = (sourceAdjusted.value.y + targetAdjusted.value.y) / 2
  const dx = targetAdjusted.value.x - sourceAdjusted.value.x
  const dy = targetAdjusted.value.y - sourceAdjusted.value.y
  return {
    x: midX + dy * 0.18,
    y: midY - dx * 0.18,
  }
})

// Позиция для текста подписи на кривой
const labelPosition = computed(() => {
  const q1 = 0.25
  const q2 = 0.5
  const q3 = 0.25
  return {
    x: q1 * sourceAdjusted.value.x + q2 * controlPoint.value.x + q3 * targetAdjusted.value.x,
    y: q1 * sourceAdjusted.value.y + q2 * controlPoint.value.y + q3 * targetAdjusted.value.y,
  }
})

// SVG path команда для quadratic Bezier
const pathD = computed(() => {
  const sx = Math.round(sourceAdjusted.value.x)
  const sy = Math.round(sourceAdjusted.value.y)
  const cx = Math.round(controlPoint.value.x)
  const cy = Math.round(controlPoint.value.y)
  const tx = Math.round(targetAdjusted.value.x)
  const ty = Math.round(targetAdjusted.value.y)
  return `M${sx},${sy} Q${cx},${cy} ${tx},${ty}`
})

// Определяем маркеры для стрелок в зависимости от направления
const markers = computed(() => {
  const color = (props.style?.stroke as string) ?? '#7a4824'
  
  return {
    hasStart: direction.value === 'backward' || direction.value === 'both',
    hasEnd: direction.value === 'forward' || direction.value === 'both',
    color,
  }
})

// Стиль линии
const pathStyle = computed(() => ({
  stroke: props.style?.stroke ?? '#7a4824',
  strokeWidth: props.style?.strokeWidth ?? 1.6,
  strokeDasharray: direction.value === 'none' ? '6 5' : undefined,
  ...props.style,
}))
</script>

<template>
  <g class="custom-edge">
    <!-- Стрелки (маркеры) -->
    <defs>
      <marker
        v-if="markers.hasStart"
        id="arrow-start"
        markerWidth="10"
        markerHeight="10"
        refX="2"
        refY="3"
        orient="auto"
        markerUnits="strokeWidth"
      >
        <path d="M9,0 L9,6 L0,3 z" :fill="markers.color" />
      </marker>
      <marker
        v-if="markers.hasEnd"
        id="arrow-end"
        markerWidth="10"
        markerHeight="10"
        refX="8"
        refY="3"
        orient="auto"
        markerUnits="strokeWidth"
      >
        <path d="M0,0 L0,6 L9,3 z" :fill="markers.color" />
      </marker>
    </defs>

    <!-- Основная линия -->
    <path
      :d="pathD"
      fill="none"
      :marker-start="markers.hasStart ? 'url(#arrow-start)' : undefined"
      :marker-end="markers.hasEnd ? 'url(#arrow-end)' : undefined"
      :stroke="pathStyle.stroke"
      :stroke-width="pathStyle.strokeWidth"
      :stroke-dasharray="pathStyle.strokeDasharray"
      class="custom-edge__path"
      stroke-linecap="round"
      stroke-linejoin="round"
      @click="$emit('click', $event)"
    />

    <!-- Подпись на кривой -->
    <text
      v-if="label"
      :x="labelPosition.x"
      :y="labelPosition.y"
      text-anchor="middle"
      dominant-baseline="middle"
      font-size="11"
      :fill="markers.color"
      class="custom-edge__label"
      pointer-events="none"
      user-select="none"
    >
      {{ label }}
    </text>
  </g>
</template>

<style scoped>
.custom-edge {
  pointer-events: visibleStroke;
}

.custom-edge__path {
  cursor: pointer;
  transition: stroke-width 0.2s ease;
}

.custom-edge__path:hover {
  stroke-width: 2.2;
  filter: brightness(1.1);
}

.custom-edge__label {
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  paint-order: stroke;
  stroke: var(--bg);
  stroke-width: 4px;
  stroke-linecap: round;
  stroke-linejoin: round;
  pointer-events: none;
  user-select: none;
}
</style>
