<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref } from 'vue'
import type { Entity } from '@/types/entity'
import { useEntityTypesStore } from '@/stores/entityTypes'
import { useEntitiesStore } from '@/stores/entities'

const props = defineProps<{
  worldId: number
  from: Entity
  to: Entity
  x: number
  y: number
}>()

const emit = defineEmits<{
  (e: 'created'): void
  (e: 'cancel'): void
}>()

const entitiesStore = useEntitiesStore()
const types = useEntityTypesStore()
const rootRef = ref<HTMLElement | null>(null)
const inputRef = ref<HTMLInputElement | null>(null)
const label = ref('')
const saving = ref(false)
const error = ref<string | null>(null)

function onWindowMouseDown(ev: MouseEvent) {
  const el = rootRef.value
  if (!el) return
  if (ev.target instanceof Node && el.contains(ev.target)) return
  emit('cancel')
}

function onKey(ev: KeyboardEvent) {
  if (ev.key === 'Escape') {
    ev.stopPropagation()
    emit('cancel')
  }
}

onMounted(() => {
  window.addEventListener('mousedown', onWindowMouseDown, true)
  window.addEventListener('keydown', onKey)
  inputRef.value?.focus()
})

onBeforeUnmount(() => {
  window.removeEventListener('mousedown', onWindowMouseDown, true)
  window.removeEventListener('keydown', onKey)
})

async function save() {
  const trimmed = label.value.trim()
  if (saving.value) return
  saving.value = true
  error.value = null
  try {
    await entitiesStore.createRelation(props.worldId, {
      fromId: props.from.id,
      toId: props.to.id,
      label: trimmed,
    })
    emit('created')
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось сохранить'
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <div
    ref="rootRef"
    class="rl-pop"
    :style="{ left: `${x}px`, top: `${y}px` }"
    @contextmenu.prevent
    @mousedown.stop
  >
    <div class="rl-pop__pair">
      <span
        class="rl-pop__chip"
        :style="{ color: types.display(from.typeKey).color }"
      >
        {{ types.display(from.typeKey).icon }} {{ from.name }}
      </span>
      <span class="rl-pop__arrow">→</span>
      <span
        class="rl-pop__chip"
        :style="{ color: types.display(to.typeKey).color }"
      >
        {{ types.display(to.typeKey).icon }} {{ to.name }}
      </span>
    </div>

    <input
      ref="inputRef"
      v-model="label"
      type="text"
      placeholder="например: соратник, враг, родом из…"
      class="rl-pop__input"
      @keydown.enter="save"
    />

    <div v-if="error" class="rl-pop__err">{{ error }}</div>

    <div class="rl-pop__actions">
      <button class="rl-pop__cancel" type="button" @click="emit('cancel')">
        Отмена
      </button>
      <button
        class="rl-pop__save"
        type="button"
        :disabled="saving"
        @click="save"
      >
        Сохранить
      </button>
    </div>
  </div>
</template>

<style scoped>
.rl-pop {
  position: absolute;
  min-width: 280px;
  max-width: 340px;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 10px;
  padding: 12px;
  box-shadow: 0 10px 26px rgba(74, 44, 26, 0.2);
  display: flex;
  flex-direction: column;
  gap: 9px;
  z-index: 30;
}

.rl-pop__pair {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.02em;
}

.rl-pop__chip {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 130px;
}

.rl-pop__arrow {
  color: var(--muted);
  font-size: 13px;
}

.rl-pop__input {
  width: 100%;
  font-family: var(--font-serif);
  font-size: 14px;
  background: rgba(255, 255, 255, 0.6);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 7px 9px;
}

.rl-pop__err {
  font-size: 12px;
  color: #a02929;
}

.rl-pop__actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}

.rl-pop__cancel,
.rl-pop__save {
  padding: 6px 14px;
  border-radius: 6px;
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  transition: filter 0.15s, opacity 0.15s;
}

.rl-pop__cancel {
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
  cursor: pointer;
}

.rl-pop__cancel:hover {
  color: var(--text);
}

.rl-pop__save {
  background: rgba(122, 72, 36, 0.18);
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
  cursor: pointer;
}

.rl-pop__save:hover:not(:disabled) {
  filter: brightness(1.05);
}

.rl-pop__save:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}
</style>
