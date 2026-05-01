<script setup lang="ts">
import { ref } from 'vue'
import type { GraphSummary } from '@/types/graph'

const props = defineProps<{
  graphs: GraphSummary[]
  selectedId: number | null
  busy?: boolean
}>()

const emit = defineEmits<{
  (e: 'select', id: number): void
  (e: 'create', body: { name: string; description: string | null }): void
  (e: 'delete', id: number): void
}>()

const showCreate = ref(false)
const newName = ref('')
const newDesc = ref('')

function startCreate() {
  showCreate.value = true
  newName.value = ''
  newDesc.value = ''
}

function cancelCreate() {
  showCreate.value = false
}

function submitCreate() {
  const name = newName.value.trim()
  if (!name) return
  emit('create', { name, description: newDesc.value.trim() || null })
  showCreate.value = false
}

function isDefault(g: GraphSummary) {
  return g.isSystemDefault
}
</script>

<template>
  <aside class="glist">
    <header class="glist__head">
      <div class="glist__title">Графы</div>
      <button class="glist__add" :disabled="busy" @click="startCreate">+</button>
    </header>

    <div v-if="showCreate" class="glist__create">
      <input
        v-model="newName"
        type="text"
        placeholder="Название графа"
        autofocus
        @keydown.enter="submitCreate"
        @keydown.escape="cancelCreate"
      />
      <textarea
        v-model="newDesc"
        rows="2"
        placeholder="Описание (опционально)"
      />
      <div class="glist__create-actions">
        <button class="glist__btn-cancel" @click="cancelCreate">Отмена</button>
        <button class="glist__btn-save" :disabled="!newName.trim()" @click="submitCreate">
          Создать
        </button>
      </div>
    </div>

    <ul class="glist__list">
      <li
        v-for="g in props.graphs"
        :key="g.id"
        class="glist__item"
        :class="{
          'glist__item--active': g.id === props.selectedId,
          'glist__item--default': isDefault(g),
        }"
        @click="emit('select', g.id)"
      >
        <span class="glist__icon">{{ isDefault(g) ? '✦' : '◇' }}</span>
        <div class="glist__text">
          <div class="glist__name">{{ g.name }}</div>
          <div v-if="g.description" class="glist__desc">{{ g.description }}</div>
        </div>
        <button
          v-if="!isDefault(g)"
          class="glist__del"
          title="Удалить граф"
          @click.stop="emit('delete', g.id)"
        >
          ×
        </button>
        <span v-else class="glist__lock" title="Системный граф (только для чтения)">🔒</span>
      </li>
    </ul>
  </aside>
</template>

<style scoped>
.glist {
  width: 240px;
  flex-shrink: 0;
  background: var(--surface);
  border-right: 1px solid var(--border);
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.glist__head {
  padding: 14px 16px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid var(--border);
}

.glist__title {
  font-family: var(--font-display);
  font-size: 12px;
  letter-spacing: 0.1em;
  color: var(--muted);
  text-transform: uppercase;
}

.glist__add {
  width: 24px;
  height: 24px;
  border-radius: 6px;
  border: 1px solid var(--border);
  background: transparent;
  color: var(--muted);
  font-size: 16px;
  line-height: 1;
}

.glist__add:hover {
  color: var(--text);
  border-color: var(--border2);
}

.glist__create {
  padding: 12px 14px;
  display: flex;
  flex-direction: column;
  gap: 6px;
  border-bottom: 1px solid var(--border);
  background: rgba(101, 67, 33, 0.04);
}

.glist__create input,
.glist__create textarea {
  width: 100%;
  font-family: var(--font-serif);
  font-size: 13px;
  background: rgba(255, 255, 255, 0.7);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 6px 8px;
}

.glist__create textarea {
  resize: vertical;
}

.glist__create-actions {
  display: flex;
  justify-content: flex-end;
  gap: 6px;
}

.glist__btn-cancel,
.glist__btn-save {
  padding: 4px 10px;
  border-radius: 6px;
  font-size: 12px;
  font-family: var(--font-display);
}

.glist__btn-cancel {
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
}

.glist__btn-save {
  background: rgba(122, 72, 36, 0.18);
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
}

.glist__btn-save:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.glist__list {
  list-style: none;
  margin: 0;
  padding: 8px;
  overflow-y: auto;
  flex: 1;
  gap: 5px;
  display: flex;
  flex-direction: column;
}

.glist__item {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 10px;
  border-radius: 7px;
  cursor: pointer;
  transition: background 0.12s;
  border: 1px solid var(--border);
}

.glist__item:hover {
  background: rgba(101, 67, 33, 0.06);
}

.glist__item--active {
  background: rgba(122, 72, 36, 0.1);
  border-color: rgba(122, 72, 36, 0.28);
}

.glist__item--default {
  border-color: rgba(184, 134, 11, 0.35);
}

.glist__icon {
  font-size: 13px;
  color: var(--muted);
  flex-shrink: 0;
}

.glist__item--active .glist__icon {
  color: #5d3a1a;
}

.glist__text {
  flex: 1;
  min-width: 0;
}

.glist__name {
  font-family: var(--font-display);
  font-size: 13px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.glist__desc {
  font-size: 11px;
  color: var(--muted);
  margin-top: 2px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.glist__del {
  color: var(--dim);
  font-size: 16px;
  line-height: 1;
  padding: 0 4px;
}

.glist__del:hover {
  color: #a02929;
}

.glist__lock {
  font-size: 11px;
  opacity: 0.6;
}
</style>
