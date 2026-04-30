<script setup lang="ts">
import { computed, ref } from 'vue'
import { useEntitiesStore } from '@/stores/entities'
import { useEntityTypesStore } from '@/stores/entityTypes'
import { useEntitySearch } from '@/composables/useEntitySearch'
import type { Entity } from '@/types/entity'

const props = defineProps<{
  /** Ids already on the graph; they're hidden from the picker. */
  excludedIds: number[]
  busy?: boolean
}>()

const emit = defineEmits<{
  (e: 'pick', entity: Entity): void
  (e: 'close'): void
}>()

const entitiesStore = useEntitiesStore()
const types = useEntityTypesStore()

const query = ref('')

const allEntities = computed(() => entitiesStore.entities)
const excluded = computed(() => new Set(props.excludedIds))
const matches = useEntitySearch(allEntities, query, { excludeIds: excluded })

function pick(e: Entity) {
  emit('pick', e)
}
</script>

<template>
  <aside class="picker">
    <header class="picker__head">
      <div class="picker__title">Добавить сущность</div>
      <button class="picker__close" type="button" @click="emit('close')">×</button>
    </header>

    <div class="picker__search">
      <span class="picker__search-icon">⌕</span>
      <input
        v-model="query"
        type="text"
        placeholder="Поиск по имени или тегам…"
        autofocus
      />
    </div>

    <div class="picker__hint">
      Кликните, чтобы добавить — окно останется открытым.
    </div>

    <ul v-if="matches.length" class="picker__list">
      <li
        v-for="m in matches"
        :key="m.id"
        class="picker__item"
        :class="{ 'picker__item--busy': busy }"
        @click="!busy && pick(m)"
      >
        <span
          class="picker__icon"
          :style="{ color: types.display(m.typeKey).color }"
        >
          {{ types.display(m.typeKey).icon }}
        </span>
        <div class="picker__text">
          <div class="picker__name">{{ m.name }}</div>
          <div v-if="m.tags.length" class="picker__tags">
            {{ m.tags.join(' · ') }}
          </div>
        </div>
        <span class="picker__plus">+</span>
      </li>
    </ul>
    <div v-else class="picker__empty">
      {{ query ? 'Ничего не найдено.' : 'Все сущности уже на графе.' }}
    </div>

    <footer class="picker__foot">
      <button class="picker__done" type="button" @click="emit('close')">
        Готово
      </button>
    </footer>
  </aside>
</template>

<style scoped>
.picker {
  width: 320px;
  flex-shrink: 0;
  background: linear-gradient(160deg, var(--card2), var(--surface));
  border-left: 1px solid var(--border);
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.picker__head {
  padding: 16px 18px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid var(--border);
}

.picker__title {
  font-family: var(--font-display);
  font-size: 14px;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--text);
}

.picker__close {
  color: var(--muted);
  font-size: 22px;
  line-height: 1;
  padding: 0 4px;
}

.picker__close:hover {
  color: var(--text);
}

.picker__search {
  position: relative;
  margin: 12px 14px 6px;
}

.picker__search input {
  width: 100%;
  padding: 8px 10px 8px 30px;
  font-size: 13px;
  font-family: var(--font-serif);
  background: rgba(255, 255, 255, 0.6);
  border: 1px solid var(--border);
  border-radius: 6px;
}

.picker__search-icon {
  position: absolute;
  left: 9px;
  top: 50%;
  transform: translateY(-50%);
  color: var(--dim);
  font-size: 13px;
  pointer-events: none;
}

.picker__hint {
  margin: 0 14px 8px;
  font-size: 11px;
  color: var(--muted);
  font-style: italic;
}

.picker__list {
  list-style: none;
  margin: 0;
  padding: 0 8px 8px;
  overflow-y: auto;
  flex: 1;
}

.picker__item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 8px 10px;
  border-radius: 7px;
  cursor: pointer;
  transition: background 0.12s;
}

.picker__item:hover {
  background: rgba(101, 67, 33, 0.08);
}

.picker__item--busy {
  opacity: 0.5;
  cursor: progress;
}

.picker__icon {
  font-size: 15px;
  flex-shrink: 0;
}

.picker__text {
  flex: 1;
  min-width: 0;
}

.picker__name {
  font-family: var(--font-display);
  font-size: 13px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.picker__tags {
  font-size: 11px;
  color: var(--muted);
  margin-top: 2px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.picker__plus {
  color: var(--muted);
  font-size: 16px;
  line-height: 1;
  padding: 0 4px;
}

.picker__item:hover .picker__plus {
  color: #5d3a1a;
}

.picker__empty {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0 14px;
  font-size: 13px;
  color: var(--muted);
  font-style: italic;
}

.picker__foot {
  padding: 12px 14px;
  border-top: 1px solid var(--border);
}

.picker__done {
  width: 100%;
  padding: 8px 0;
  border-radius: 7px;
  background: rgba(122, 72, 36, 0.18);
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
  font-family: var(--font-display);
  font-size: 12px;
  letter-spacing: 0.06em;
}

.picker__done:hover {
  filter: brightness(1.05);
}
</style>
