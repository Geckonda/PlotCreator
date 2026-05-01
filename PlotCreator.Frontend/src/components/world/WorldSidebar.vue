<script setup lang="ts">
import { computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useEntityTypesStore } from '@/stores/entityTypes'
import { useWorldUiStore, type WorldViewMode } from '@/stores/worldUi'
import { useEntitiesStore } from '@/stores/entities'
import { useWorldsStore } from '@/stores/worlds'

const ui = useWorldUiStore()
const entitiesStore = useEntitiesStore()
const worldsStore = useWorldsStore()
const types = useEntityTypesStore()

const { view, activeType } = storeToRefs(ui)

const counts = computed(() => {
  const out: Record<string, number> = {}
  for (const t of types.types) out[t.key] = 0
  for (const e of entitiesStore.entities) {
    out[e.typeKey] = (out[e.typeKey] ?? 0) + 1
  }
  return out
})

function pickView(v: WorldViewMode) {
  view.value = v
  activeType.value = null
}

function pickType(typeKey: string) {
  view.value = 'grid'
  activeType.value = typeKey
}

function colorFor(typeKey: string, isActive: boolean): string {
  return isActive ? types.display(typeKey).color : 'inherit'
}
</script>

<template>
  <aside class="sidebar">
    <div class="sidebar__brand">
      <div class="sidebar__name">МАСТЕРСКАЯ</div>
    </div>

    <div class="sidebar__nav">
      <button
        class="nav-item"
        :class="{ 'nav-item--active': view === 'graph' && !activeType }"
        @click="pickView('graph')"
      >
        <span class="nav-item__icon">◈</span>
        <span class="nav-item__label">Граф связей</span>
      </button>

      <button
        class="nav-item"
        :class="{ 'nav-item--active': view === 'grid' && !activeType }"
        @click="pickView('grid')"
      >
        <span class="nav-item__icon">▦</span>
        <span class="nav-item__label">Все карточки</span>
      </button>

      <!-- <button
        class="nav-item"
        :class="{ 'nav-item--active': view === 'timeline' }"
        @click="pickView('timeline')"
      >
        <span class="nav-item__icon">◆</span>
        <span class="nav-item__label">Хронология</span>
      </button> -->

      <div class="sidebar__section">Сущности</div>

      <button
        v-for="t in types.types"
        :key="t.id"
        class="nav-item nav-item--sub"
        :class="{ 'nav-item--active': activeType === t.key }"
        @click="pickType(t.key)"
      >
        <span
          class="nav-item__icon"
          :style="{ color: colorFor(t.key, activeType === t.key) }"
        >
          {{ t.icon }}
        </span>
        <span class="nav-item__label">{{ t.label }}</span>
        <span class="nav-item__count">{{ counts[t.key] ?? 0 }}</span>
      </button>
    </div>

    <div class="sidebar__footer">
      {{ entitiesStore.entities.length }} сущностей ·
      {{ entitiesStore.relations.length }} связей
    </div>
  </aside>
</template>

<style scoped>
.sidebar {
  width: 250px;
  flex-shrink: 0;
  background: linear-gradient(to bottom, var(--surface), #d4c298);
  border-right: 1px solid var(--border);
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.sidebar__brand {
  padding: 22px 20px 18px;
  border-bottom: 1px solid var(--border);
}

.sidebar__name {
  font-family: var(--font-display);
  font-size: 17px;
  font-weight: 700;
  letter-spacing: 0.14em;
  color: var(--accent);
}

.sidebar__sub {
  font-size: 13px;
  color: var(--dim);
  margin-top: 4px;
  font-style: italic;
  letter-spacing: 0.04em;
}

.sidebar__nav {
  flex: 1;
  overflow-y: auto;
  padding: 12px 8px;
}

.sidebar__section {
  font-size: 11px;
  color: #8a6f3d;
  padding: 16px 10px 7px;
  font-family: var(--font-display);
  letter-spacing: 0.1em;
  text-transform: uppercase;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 10px;
  width: 100%;
  text-align: left;
  padding: 9px 16px;
  border-radius: 8px;
  margin-bottom: 2px;
  background: transparent;
  border: 1px solid transparent;
  color: var(--muted);
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.03em;
  transition: all 0.15s;
}

.nav-item--sub {
  padding: 8px 12px 8px 30px;
}

.nav-item:hover:not(.nav-item--active) {
  background: rgba(101, 67, 33, 0.06);
  color: var(--text);
}

.nav-item--active {
  background: rgba(122, 72, 36, 0.12);
  border-color: rgba(122, 72, 36, 0.26);
  color: #5d3a1a;
}

.nav-item__icon {
  font-size: 14px;
  opacity: 0.9;
}

.nav-item__label {
  flex: 1;
}

.nav-item__count {
  font-size: 12px;
  color: var(--dim);
  font-variant-numeric: tabular-nums;
}

.sidebar__footer {
  padding: 12px 18px;
  border-top: 1px solid var(--border);
  font-size: 12px;
  color: var(--dim);
  font-style: italic;
}
</style>
