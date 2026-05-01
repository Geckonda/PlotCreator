<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted } from 'vue'
import { useEntityTypesStore } from '@/stores/entityTypes'
import type { Entity } from '@/types/entity'

const props = defineProps<{
  entity: Entity
  x: number
  y: number
  alreadyRelated: boolean
  busy?: boolean
}>()

const emit = defineEmits<{
  (e: 'create-relation'): void
  (e: 'open-entity'): void
  (e: 'close'): void
}>()

const types = useEntityTypesStore()
const cfg = computed(() => types.display(props.entity.typeKey))

const style = computed(() => {
  const POP_W = 240
  const POP_H = 160
  const x = Math.min(Math.max(props.x, 12), window.innerWidth - POP_W - 12)
  const y = Math.min(Math.max(props.y, 12), window.innerHeight - POP_H - 12)
  return { left: `${x}px`, top: `${y}px` }
})

function onDocClick(ev: MouseEvent) {
  const t = ev.target as HTMLElement | null
  if (t && t.closest('.ementp')) return
  emit('close')
}

function onKey(ev: KeyboardEvent) {
  if (ev.key === 'Escape') emit('close')
}

onMounted(() => {
  setTimeout(() => {
    window.addEventListener('mousedown', onDocClick)
    window.addEventListener('keydown', onKey)
  }, 0)
})

onBeforeUnmount(() => {
  window.removeEventListener('mousedown', onDocClick)
  window.removeEventListener('keydown', onKey)
})
</script>

<template>
  <div class="ementp" :style="style">
    <div class="ementp__head">
      <span class="ementp__icon" :style="{ color: cfg.color }">{{ cfg.icon }}</span>
      <span class="ementp__name">{{ entity.name }}</span>
      <span
        v-if="alreadyRelated"
        class="ementp__badge"
        :style="{ borderColor: `${cfg.color}55`, color: cfg.color }"
      >
        связано
      </span>
    </div>

    <div class="ementp__actions">
      <button
        type="button"
        class="ementp__btn"
        :disabled="busy || alreadyRelated"
        :title="alreadyRelated ? 'Связь уже существует' : 'Создать связь'"
        @click="emit('create-relation')"
      >
        + Создать связь
      </button>
      <button
        type="button"
        class="ementp__btn ementp__btn--ghost"
        :disabled="busy"
        @click="emit('open-entity')"
      >
        ↗ Открыть сущность
      </button>
    </div>
  </div>
</template>

<style scoped>
.ementp {
  position: fixed;
  z-index: 60;
  width: 240px;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 8px;
  box-shadow: 0 14px 32px rgba(74, 44, 26, 0.22);
  padding: 12px;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.ementp__head {
  display: flex;
  align-items: center;
  gap: 8px;
}

.ementp__icon {
  font-size: 14px;
}

.ementp__name {
  flex: 1;
  font-family: var(--font-display);
  font-size: 13px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.ementp__badge {
  font-size: 10px;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  border: 1px solid;
  border-radius: 4px;
  padding: 1px 6px;
}

.ementp__actions {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.ementp__btn {
  width: 100%;
  padding: 7px 10px;
  border-radius: 6px;
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  text-align: left;
  background: rgba(122, 72, 36, 0.14);
  border: 1px solid rgba(122, 72, 36, 0.34);
  color: #5d3a1a;
  cursor: pointer;
  transition: filter 0.15s, opacity 0.15s;
}

.ementp__btn:hover:not(:disabled) {
  filter: brightness(1.08);
}

.ementp__btn:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.ementp__btn--ghost {
  background: transparent;
  border-color: var(--border);
  color: var(--muted);
}

.ementp__btn--ghost:hover:not(:disabled) {
  color: var(--text);
}
</style>
