<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import type { Entity } from '@/types/entity'
import { useEntityTypesStore } from '@/stores/entityTypes'

const props = defineProps<{
  entity: Entity
  x: number
  y: number
}>()

const emit = defineEmits<{
  (e: 'open-details'): void
  (e: 'delete'): void
  (e: 'close'): void
}>()

const types = useEntityTypesStore()
const cfg = computed(() => types.display(props.entity.typeKey))
const rootRef = ref<HTMLElement | null>(null)

function onWindowMouseDown(ev: MouseEvent) {
  const el = rootRef.value
  if (!el) return
  if (ev.target instanceof Node && el.contains(ev.target)) return
  emit('close')
}

function onKey(ev: KeyboardEvent) {
  if (ev.key === 'Escape') {
    ev.stopPropagation()
    emit('close')
  }
}

onMounted(() => {
  window.addEventListener('mousedown', onWindowMouseDown, true)
  window.addEventListener('keydown', onKey)
})

onBeforeUnmount(() => {
  window.removeEventListener('mousedown', onWindowMouseDown, true)
  window.removeEventListener('keydown', onKey)
})

function pickOpen() {
  emit('open-details')
  emit('close')
}

function pickDelete() {
  emit('delete')
  emit('close')
}
</script>

<template>
  <div
    ref="rootRef"
    class="ctx-menu"
    :style="{ left: `${x}px`, top: `${y}px` }"
    @contextmenu.prevent
  >
    <div class="ctx-menu__header">
      <span class="ctx-menu__icon" :style="{ color: cfg.color }">
        {{ cfg.icon }}
      </span>
      <span class="ctx-menu__name">{{ entity.name }}</span>
    </div>
    <button type="button" class="ctx-menu__item" @click="pickOpen">
      <span class="ctx-menu__item-icon">🔍</span>
      <span>Открыть подробнее</span>
    </button>
    <button
      type="button"
      class="ctx-menu__item ctx-menu__item--danger"
      @click="pickDelete"
    >
      <span class="ctx-menu__item-icon">🗑</span>
      <span>Удалить</span>
    </button>
  </div>
</template>

<style scoped>
.ctx-menu {
  position: absolute;
  min-width: 180px;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 8px;
  padding: 6px;
  box-shadow: 0 8px 22px rgba(74, 44, 26, 0.18);
  display: flex;
  flex-direction: column;
  gap: 2px;
  z-index: 30;
  font-family: var(--font-serif);
}

.ctx-menu__header {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 6px 8px 8px;
  border-bottom: 1px solid var(--border);
  margin-bottom: 4px;
}

.ctx-menu__icon {
  font-size: 14px;
}

.ctx-menu__name {
  font-family: var(--font-display);
  font-size: 13px;
  letter-spacing: 0.02em;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 220px;
}

.ctx-menu__item {
  display: flex;
  align-items: center;
  gap: 9px;
  padding: 7px 9px;
  border-radius: 5px;
  background: transparent;
  border: none;
  font-size: 13px;
  font-family: var(--font-serif);
  color: var(--text);
  text-align: left;
  cursor: pointer;
  transition: background 0.12s, color 0.12s;
}

.ctx-menu__item:hover {
  background: rgba(101, 67, 33, 0.08);
}

.ctx-menu__item--danger:hover {
  background: rgba(160, 41, 41, 0.1);
  color: #a02929;
}

.ctx-menu__item-icon {
  font-size: 13px;
  width: 16px;
  text-align: center;
}
</style>
