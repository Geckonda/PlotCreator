<script setup lang="ts">
import { computed } from 'vue'
import type { Entity } from '@/types/entity'
import { ENTITY_TYPES } from '@/config/entityTypes'
import { useEntitiesStore } from '@/stores/entities'
import TypeBadge from '@/components/ui/TypeBadge.vue'
import StatusPip from '@/components/ui/StatusPip.vue'
import EntityTag from '@/components/ui/EntityTag.vue'

const props = defineProps<{
  entity: Entity
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'edit', entity: Entity): void
  (e: 'delete', entity: Entity): void
}>()

const entitiesStore = useEntitiesStore()
const cfg = computed(() => ENTITY_TYPES[props.entity.type])

const connRels = computed(() => entitiesStore.relationsFor(props.entity.id))

const showImg = computed(() =>
  ['character', 'location', 'artifact'].includes(props.entity.type),
)

const imgLabel = computed(() => {
  if (props.entity.type === 'character') return '[ portrait ]'
  if (props.entity.type === 'location') return '[ map / artwork ]'
  return '[ illustration ]'
})

const panelStyle = computed(() => ({
  borderLeft: `1px solid ${cfg.value.color}45`,
}))

const headerStyle = computed(() => ({
  background: `linear-gradient(135deg, ${cfg.value.color}14, transparent)`,
  borderBottom: `1px solid ${cfg.value.color}28`,
}))

const artStyle = computed(() => ({
  background: `repeating-linear-gradient(135deg, ${cfg.value.color}10, ${cfg.value.color}10 3px, transparent 3px, transparent 16px)`,
  border: `1px solid ${cfg.value.color}30`,
}))

const editStyle = computed(() => ({
  background: `${cfg.value.color}1c`,
  color: cfg.value.color,
  border: `1px solid ${cfg.value.color}48`,
}))

function getOther(rel: { from: string; to: string }) {
  const otherId = rel.from === props.entity.id ? rel.to : rel.from
  return entitiesStore.byId.get(otherId) ?? null
}

function getDirection(rel: { from: string }) {
  return rel.from === props.entity.id ? '→' : '←'
}
</script>

<template>
  <aside class="panel" :style="panelStyle">
    <header class="panel__head" :style="headerStyle">
      <div class="panel__head-row">
        <TypeBadge :type="entity.type" />
        <button class="panel__close" @click="emit('close')">×</button>
      </div>
      <h3 class="panel__name">{{ entity.name }}</h3>
      <StatusPip :status="entity.status" with-label />
    </header>

    <div v-if="showImg" class="panel__art" :style="artStyle">
      <span class="panel__art-icon" :style="{ color: cfg.color }">
        {{ cfg.icon }}
      </span>
      <span class="panel__art-tag">{{ imgLabel }}</span>
    </div>

    <div class="panel__body">
      <p v-if="entity.desc" class="panel__desc">{{ entity.desc }}</p>
      <div class="panel__tags">
        <EntityTag v-for="t in entity.tags" :key="t" :label="t" />
      </div>
    </div>

    <section v-if="connRels.length" class="panel__conns">
      <div class="panel__conns-title">Связи · {{ connRels.length }}</div>
      <ul class="panel__conn-list">
        <li
          v-for="rel in connRels"
          :key="rel.id"
          class="panel__conn"
        >
          <template v-if="getOther(rel)">
            <span
              class="panel__conn-icon"
              :style="{ color: ENTITY_TYPES[getOther(rel)!.type].color }"
            >
              {{ ENTITY_TYPES[getOther(rel)!.type].icon }}
            </span>
            <div class="panel__conn-text">
              <div class="panel__conn-name">{{ getOther(rel)!.name }}</div>
              <div class="panel__conn-rel">{{ getDirection(rel) }} {{ rel.label }}</div>
            </div>
          </template>
        </li>
      </ul>
    </section>

    <footer class="panel__actions">
      <button
        class="panel__edit"
        :style="editStyle"
        @click="emit('edit', entity)"
      >
        Редактировать
      </button>
      <button class="panel__delete" @click="emit('delete', entity)">✕</button>
    </footer>
  </aside>
</template>

<style scoped>
.panel {
  width: 340px;
  flex-shrink: 0;
  background: linear-gradient(160deg, var(--card2), var(--surface));
  display: flex;
  flex-direction: column;
  overflow-y: auto;
  animation: slideR 0.28s cubic-bezier(0.2, 0.8, 0.2, 1);
}

.panel__head {
  padding: 22px 22px 18px;
}

.panel__head-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 12px;
}

.panel__close {
  color: var(--muted);
  font-size: 24px;
  line-height: 1;
  padding: 0 4px;
  transition: color 0.15s;
}

.panel__close:hover {
  color: var(--text);
}

.panel__name {
  font-family: var(--font-display);
  font-size: 21px;
  font-weight: 600;
  line-height: 1.3;
  margin-bottom: 10px;
}

.panel__art {
  height: 140px;
  margin: 16px 18px 0;
  border-radius: 9px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 6px;
}

.panel__art-icon {
  font-size: 34px;
  opacity: 0.22;
}

.panel__art-tag {
  font-size: 11px;
  color: var(--dim);
  font-family: monospace;
}

.panel__body {
  padding: 18px 22px;
}

.panel__desc {
  font-size: 16px;
  color: #6b5235;
  line-height: 1.7;
  margin-bottom: 16px;
  font-style: italic;
}

.panel__tags {
  display: flex;
  flex-wrap: wrap;
  gap: 5px;
}

.panel__conns {
  padding: 16px 22px 18px;
  border-top: 1px solid var(--border);
}

.panel__conns-title {
  font-family: var(--font-display);
  font-size: 12px;
  letter-spacing: 0.1em;
  color: var(--muted);
  margin-bottom: 12px;
  text-transform: uppercase;
}

.panel__conn-list {
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.panel__conn {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 9px 12px;
  border-radius: 8px;
  background: rgba(101, 67, 33, 0.04);
  border: 1px solid var(--border);
  transition: background 0.15s;
}

.panel__conn:hover {
  background: rgba(101, 67, 33, 0.08);
}

.panel__conn-icon {
  font-size: 14px;
  flex-shrink: 0;
}

.panel__conn-text {
  flex: 1;
  min-width: 0;
}

.panel__conn-name {
  font-size: 14px;
  font-family: var(--font-display);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.panel__conn-rel {
  font-size: 12px;
  color: var(--muted);
  margin-top: 2px;
}

.panel__actions {
  margin-top: auto;
  padding: 16px 22px;
  border-top: 1px solid var(--border);
  display: flex;
  gap: 10px;
}

.panel__edit {
  flex: 1;
  padding: 9px 0;
  border-radius: 8px;
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  transition: filter 0.15s;
}

.panel__edit:hover {
  filter: brightness(1.1);
}

.panel__delete {
  padding: 9px 18px;
  border-radius: 8px;
  background: transparent;
  color: var(--muted);
  border: 1px solid var(--border);
  font-size: 14px;
  font-family: var(--font-display);
  transition: all 0.15s;
}

.panel__delete:hover {
  color: #a02929;
  border-color: rgba(160, 41, 41, 0.4);
}
</style>
