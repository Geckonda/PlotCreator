<script setup lang="ts">
import { ref, watch } from 'vue'
import type { EdgeDirection } from '@/types/graph'

const props = defineProps<{
  initialDirection: EdgeDirection
  initialLabel?: string | null
  showDelete?: boolean
  busy?: boolean
}>()

const emit = defineEmits<{
  (e: 'save', payload: { direction: EdgeDirection; label: string | null }): void
  (e: 'delete'): void
  (e: 'cancel'): void
}>()

const direction = ref<EdgeDirection>(props.initialDirection)
const label = ref(props.initialLabel ?? '')

watch(
  () => [props.initialDirection, props.initialLabel],
  ([d, l]) => {
    direction.value = d as EdgeDirection
    label.value = (l as string | null | undefined) ?? ''
  },
)

const options: Array<[EdgeDirection, string, string]> = [
  ['forward', 'A → B', 'Стрелка к B'],
  ['backward', 'B → A', 'Стрелка к A'],
  ['both', 'A ⇔ B', 'Двунаправленная'],
  ['none', 'A — B', 'Без направления'],
]

function save() {
  const trimmed = label.value.trim()
  emit('save', { direction: direction.value, label: trimmed === '' ? null : trimmed })
}
</script>

<template>
  <div class="edge-pop">
    <div class="edge-pop__title">Связь</div>

    <div class="edge-pop__group">
      <label
        v-for="[val, lbl, desc] in options"
        :key="val"
        class="edge-pop__opt"
        :class="{ 'edge-pop__opt--active': direction === val }"
      >
        <input v-model="direction" type="radio" :value="val" />
        <span class="edge-pop__opt-lbl">{{ lbl }}</span>
        <span class="edge-pop__opt-desc">{{ desc }}</span>
      </label>
    </div>

    <div class="edge-pop__field">
      <label>Подпись</label>
      <input
        v-model="label"
        type="text"
        placeholder="например: соратник, враг…"
        @keydown.enter="save"
      />
    </div>

    <div class="edge-pop__actions">
      <button
        v-if="showDelete"
        type="button"
        class="edge-pop__del"
        :disabled="busy"
        @click="emit('delete')"
      >
        Удалить
      </button>
      <div class="edge-pop__spacer" />
      <button type="button" class="edge-pop__cancel" :disabled="busy" @click="emit('cancel')">
        Отмена
      </button>
      <button type="button" class="edge-pop__save" :disabled="busy" @click="save">
        Сохранить
      </button>
    </div>
  </div>
</template>

<style scoped>
.edge-pop {
  width: 240px;
  padding: 12px;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 8px;
  box-shadow: 0 12px 28px rgba(74, 44, 26, 0.18);
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.edge-pop__title {
  font-family: var(--font-display);
  font-size: 12px;
  letter-spacing: 0.1em;
  color: var(--muted);
  text-transform: uppercase;
}

.edge-pop__group {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.edge-pop__opt {
  display: grid;
  grid-template-columns: auto auto 1fr;
  align-items: center;
  gap: 8px;
  padding: 5px 7px;
  border-radius: 6px;
  cursor: pointer;
  font-size: 13px;
  border: 1px solid transparent;
}

.edge-pop__opt:hover {
  background: rgba(101, 67, 33, 0.05);
}

.edge-pop__opt--active {
  background: rgba(122, 72, 36, 0.1);
  border-color: rgba(122, 72, 36, 0.28);
}

.edge-pop__opt-lbl {
  font-family: var(--font-display);
}

.edge-pop__opt-desc {
  font-size: 11px;
  color: var(--muted);
  text-align: right;
}

.edge-pop__field {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.edge-pop__field label {
  font-size: 11px;
  color: var(--muted);
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.edge-pop__field input {
  width: 100%;
  font-family: var(--font-serif);
  font-size: 13px;
  background: rgba(255, 255, 255, 0.6);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 6px 8px;
}

.edge-pop__actions {
  display: flex;
  gap: 6px;
  align-items: center;
}

.edge-pop__spacer {
  flex: 1;
}

.edge-pop__del,
.edge-pop__cancel,
.edge-pop__save {
  padding: 5px 12px;
  border-radius: 6px;
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  transition: filter 0.15s, opacity 0.15s;
}

.edge-pop__del {
  background: transparent;
  border: 1px solid rgba(160, 41, 41, 0.4);
  color: #a02929;
}

.edge-pop__cancel {
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
}

.edge-pop__save {
  background: rgba(122, 72, 36, 0.18);
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
}

.edge-pop__save:hover:not(:disabled) {
  filter: brightness(1.05);
}

.edge-pop__save:disabled,
.edge-pop__cancel:disabled,
.edge-pop__del:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}
</style>
