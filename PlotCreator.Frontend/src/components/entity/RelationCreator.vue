<script setup lang="ts">
import { computed, ref } from 'vue'
import type { Entity } from '@/types/entity'
import { ENTITY_TYPES } from '@/config/entityTypes'
import { useEntitiesStore } from '@/stores/entities'

const props = defineProps<{
  worldId: number
  from: Entity
}>()

const emit = defineEmits<{
  (e: 'created'): void
  (e: 'cancel'): void
}>()

const entitiesStore = useEntitiesStore()
const search = ref('')
const target = ref<Entity | null>(null)
const label = ref('')
const saving = ref(false)
const error = ref<string | null>(null)

const matches = computed(() => {
  const q = search.value.trim().toLowerCase()
  if (!q) return [] as Entity[]
  const out: Entity[] = []
  for (const e of entitiesStore.entities) {
    if (e.id === props.from.id && e.type === props.from.type) continue
    const inName = e.name.toLowerCase().includes(q)
    const inTags = e.tags.some((t) => t.toLowerCase().includes(q))
    if (inName || inTags) out.push(e)
    if (out.length >= 8) break
  }
  return out
})

function pick(e: Entity) {
  target.value = e
  search.value = ''
}

function clearTarget() {
  target.value = null
}

const canSave = computed(
  () => target.value !== null && label.value.trim() !== '' && !saving.value,
)

async function save() {
  if (!canSave.value || !target.value) return
  saving.value = true
  error.value = null
  try {
    await entitiesStore.createRelation(props.worldId, {
      fromId: props.from.id,
      fromType: props.from.type,
      toId: target.value.id,
      toType: target.value.type,
      label: label.value.trim(),
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
  <div class="creator">
    <div class="creator__row">
      <label class="creator__lbl">Цель</label>
      <div v-if="target" class="creator__chip">
        <span
          class="creator__chip-icon"
          :style="{ color: ENTITY_TYPES[target.type].color }"
        >
          {{ ENTITY_TYPES[target.type].icon }}
        </span>
        <span class="creator__chip-name">{{ target.name }}</span>
        <button
          type="button"
          class="creator__chip-clear"
          @click="clearTarget"
        >
          ×
        </button>
      </div>
      <div v-else class="creator__search">
        <input
          v-model="search"
          type="text"
          placeholder="Поиск сущности…"
          autofocus
        />
        <ul v-if="matches.length" class="creator__matches">
          <li
            v-for="m in matches"
            :key="`${m.type}:${m.id}`"
            class="creator__match"
            @click="pick(m)"
          >
            <span
              class="creator__match-icon"
              :style="{ color: ENTITY_TYPES[m.type].color }"
            >
              {{ ENTITY_TYPES[m.type].icon }}
            </span>
            <span class="creator__match-name">{{ m.name }}</span>
          </li>
        </ul>
      </div>
    </div>

    <div class="creator__row">
      <label class="creator__lbl">Подпись</label>
      <input
        v-model="label"
        type="text"
        placeholder="например: соратник, враг, родом из…"
        @keydown.enter="save"
      />
    </div>

    <div v-if="error" class="creator__err">{{ error }}</div>

    <div class="creator__actions">
      <button class="creator__cancel" type="button" @click="emit('cancel')">
        Отмена
      </button>
      <button
        class="creator__save"
        type="button"
        :disabled="!canSave"
        @click="save"
      >
        Сохранить
      </button>
    </div>
  </div>
</template>

<style scoped>
.creator {
  background: rgba(101, 67, 33, 0.04);
  border: 1px solid var(--border);
  border-radius: 8px;
  padding: 12px;
  margin-bottom: 10px;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.creator__row {
  display: flex;
  flex-direction: column;
  gap: 5px;
  position: relative;
}

.creator__lbl {
  font-size: 11px;
  color: var(--muted);
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.creator__search input,
.creator__row > input {
  width: 100%;
  font-family: var(--font-serif);
  font-size: 14px;
  background: rgba(255, 255, 255, 0.6);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 7px 9px;
}

.creator__matches {
  list-style: none;
  margin: 4px 0 0;
  padding: 0;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 6px;
  max-height: 200px;
  overflow-y: auto;
  box-shadow: 0 6px 16px rgba(74, 44, 26, 0.12);
}

.creator__match {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 7px 10px;
  cursor: pointer;
  font-size: 13px;
  transition: background 0.12s;
}

.creator__match:hover {
  background: rgba(101, 67, 33, 0.06);
}

.creator__match-icon {
  font-size: 13px;
}

.creator__chip {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 6px 10px;
  background: rgba(255, 255, 255, 0.6);
  border: 1px solid var(--border);
  border-radius: 6px;
}

.creator__chip-icon {
  font-size: 13px;
}

.creator__chip-name {
  flex: 1;
  font-size: 13px;
  font-family: var(--font-display);
}

.creator__chip-clear {
  color: var(--muted);
  font-size: 16px;
  line-height: 1;
  padding: 0 4px;
}

.creator__chip-clear:hover {
  color: #a02929;
}

.creator__err {
  font-size: 12px;
  color: #a02929;
}

.creator__actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}

.creator__cancel,
.creator__save {
  padding: 6px 14px;
  border-radius: 6px;
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  transition: filter 0.15s, opacity 0.15s;
}

.creator__cancel {
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
}

.creator__cancel:hover {
  color: var(--text);
}

.creator__save {
  background: rgba(122, 72, 36, 0.18);
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
}

.creator__save:hover:not(:disabled) {
  filter: brightness(1.05);
}

.creator__save:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}
</style>
