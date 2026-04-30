<script setup lang="ts">
import { computed } from 'vue'
import { Handle, Position, type NodeProps } from '@vue-flow/core'
import { useEntityTypesStore } from '@/stores/entityTypes'

interface CircleNodeData {
  entityId: number
  entityName: string
  entityTypeKey: string | null
}

const props = defineProps<NodeProps<CircleNodeData>>()

const types = useEntityTypesStore()

const cfg = computed(() =>
  props.data.entityTypeKey
    ? types.display(props.data.entityTypeKey)
    : { color: '#7a4824', icon: '◆', label: '', radius: 14 },
)

const radius = computed(() => Math.max(14, cfg.value.radius || 14))
const diameter = computed(() => radius.value * 2)
const wrapperSize = computed(() => diameter.value + 18)

const fillColor = computed(() => `${cfg.value.color}1c`)
const fillBright = computed(() => `${cfg.value.color}38`)
const isBright = computed(() => props.selected)

function truncate(name: string): string {
  return name.length > 16 ? name.slice(0, 15) + '…' : name
}
</script>

<template>
  <div
    class="cnode"
    :style="{
      width: `${wrapperSize}px`,
      height: `${wrapperSize}px`,
    }"
  >
    <Handle
      type="target"
      :position="Position.Top"
      class="cnode__handle cnode__handle--target"
    />
    <Handle
      type="source"
      :position="Position.Top"
      class="cnode__handle cnode__handle--source"
    />

    <svg
      class="cnode__svg"
      :width="wrapperSize"
      :height="wrapperSize"
      :viewBox="`-${wrapperSize / 2} -${wrapperSize / 2} ${wrapperSize} ${wrapperSize}`"
    >
      <circle
        :r="radius + 3"
        fill="none"
        :stroke="cfg.color"
        :stroke-width="isBright ? 1.6 : 0.9"
        :opacity="isBright ? 0.6 : 0.28"
      />
      <circle
        :r="radius"
        :fill="isBright ? fillBright : fillColor"
        :stroke="cfg.color"
        :stroke-width="isBright ? 2.2 : 1.4"
      />
      <text
        text-anchor="middle"
        dominant-baseline="central"
        :font-size="radius > 18 ? 14 : 12"
        :fill="cfg.color"
        :opacity="isBright ? 1 : 0.85"
        class="cnode__icon"
      >
        {{ cfg.icon }}
      </text>
    </svg>

    <div
      class="cnode__name"
      :style="{
        top: `${wrapperSize / 2 + radius + 4}px`,
        color: isBright ? 'var(--text)' : 'rgba(58, 36, 20, 0.7)',
      }"
    >
      {{ truncate(data.entityName) }}
    </div>
  </div>
</template>

<style scoped>
.cnode {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  background: transparent;
  border: none;
  pointer-events: auto;
}

.cnode__svg {
  pointer-events: none;
  overflow: visible;
}

.cnode__icon {
  font-family: var(--font-display);
  user-select: none;
}

.cnode__name {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.02em;
  white-space: nowrap;
  pointer-events: none;
  user-select: none;
}

.cnode__handle {
  width: 10px;
  height: 10px;
  background: transparent;
  border: none;
  opacity: 0;
  /* Переместить Handle в центр узла */
  /* top: 50% !important;
  left: 50% !important;
  transform: translate(-50%, -50%) !important; */
}

.cnode :deep(.vue-flow__handle) {
  background: transparent;
  border: none;
}

.cnode:hover :deep(.vue-flow__handle) {
  background: rgba(122, 72, 36, 0.45);
  border: 1px solid rgba(122, 72, 36, 0.7);
  opacity: 1;
}
</style>
