<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useWorldUiStore, type WorldViewMode } from '@/stores/worldUi'

const router = useRouter()
const route = useRoute()
const ui = useWorldUiStore()
const { view, search } = storeToRefs(ui)

const tabs: Array<[WorldViewMode, string, string]> = [
  ['graph', '◈', 'Граф'],
  ['grid', '▦', 'Карточки'],
  ['timeline', '◆', 'Хронология'],
]

function selectTab(id: WorldViewMode) {
  view.value = id
  if (id !== 'grid') ui.activeType = null
}

function goBack() {
  router.push({ name: 'home' })
}

function openGraphs() {
  const id = route.params.id
  if (!id) return
  router.push({ name: 'graph-view', params: { id: String(id) } })
}
</script>

<template>
  <div class="topbar">
    <button class="topbar__back" @click="goBack">← Миры</button>

    <span class="topbar__sep" />

    <button
      v-for="[id, ico, lbl] in tabs"
      :key="id"
      class="topbar__tab"
      :class="{ 'topbar__tab--active': view === id }"
      @click="selectTab(id)"
    >
      {{ ico }} {{ lbl }}
    </button>

    <button class="topbar__tab" @click="openGraphs">
      ⌬ Графы
    </button>

    <div class="topbar__spacer" />

    <div class="topbar__search">
      <span class="topbar__search-icon">⌕</span>
      <input v-model="search" placeholder="Поиск…" />
    </div>

    <button class="topbar__create" @click="ui.showCreate = true">
      + Создать
    </button>
  </div>
</template>

<style scoped>
.topbar {
  height: 60px;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 0 18px;
  border-bottom: 1px solid var(--border);
  background: var(--surface);
  z-index: 10;
}

.topbar__back {
  color: var(--muted);
  padding: 6px 12px;
  border-radius: 7px;
  font-size: 13px;
  font-family: var(--font-display);
  border: 1px solid transparent;
  letter-spacing: 0.03em;
  transition: all 0.15s;
}

.topbar__back:hover {
  color: var(--accent);
  border-color: var(--border);
}

.topbar__sep {
  width: 1px;
  height: 22px;
  background: var(--border);
}

.topbar__tab {
  padding: 6px 14px;
  border-radius: 7px;
  font-size: 13px;
  font-family: var(--font-display);
  background: transparent;
  border: 1px solid transparent;
  color: var(--muted);
  display: flex;
  align-items: center;
  gap: 6px;
  letter-spacing: 0.03em;
  transition: all 0.15s;
}

.topbar__tab:hover {
  color: var(--text);
}

.topbar__tab--active {
  background: rgba(122, 72, 36, 0.12);
  border-color: rgba(122, 72, 36, 0.28);
  color: #5d3a1a;
}

.topbar__spacer {
  flex: 1;
}

.topbar__search {
  position: relative;
}

.topbar__search input {
  width: 230px;
  padding-left: 32px;
  height: 36px;
  font-size: 14px;
}

.topbar__search-icon {
  position: absolute;
  left: 10px;
  top: 50%;
  transform: translateY(-50%);
  color: var(--dim);
  font-size: 14px;
  pointer-events: none;
}

.topbar__create {
  padding: 8px 20px;
  border-radius: 8px;
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  background: linear-gradient(135deg, rgba(122, 72, 36, 0.2), rgba(184, 134, 11, 0.12));
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
  transition: all 0.15s;
}

.topbar__create:hover {
  background: linear-gradient(135deg, rgba(122, 72, 36, 0.32), rgba(184, 134, 11, 0.2));
}
</style>
