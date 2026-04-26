<script setup lang="ts">
import { computed, ref } from 'vue'
import type { Entity, Relation } from '@/types/entity'
import { ENTITY_TYPES } from '@/config/entityTypes'
import TypeBadge from '@/components/ui/TypeBadge.vue'
import StatusPip from '@/components/ui/StatusPip.vue'
import EntityTag from '@/components/ui/EntityTag.vue'

const props = withDefaults(
  defineProps<{
    entity: Entity
    relations?: Relation[]
    compact?: boolean
  }>(),
  { relations: () => [], compact: false },
)

const emit = defineEmits<{
  (e: 'select', entity: Entity): void
}>()

const cfg = computed(() => ENTITY_TYPES[props.entity.type])
const hov = ref(false)

const connCount = computed(
  () =>
    props.relations.filter(
      (r) => r.from === props.entity.id || r.to === props.entity.id,
    ).length,
)

const showImg = computed(
  () =>
    !props.compact &&
    ['character', 'location', 'artifact'].includes(props.entity.type),
)

const cardStyle = computed(() => ({
  background: hov.value ? 'var(--card2)' : 'var(--card)',
  border: `1px solid ${hov.value ? cfg.value.color + '55' : 'var(--border)'}`,
  transform: hov.value ? 'translateY(-2px)' : 'none',
  boxShadow: hov.value
    ? `0 10px 28px ${cfg.value.color}20, 0 2px 6px rgba(74, 44, 26, 0.12)`
    : '0 2px 6px rgba(74, 44, 26, 0.08)',
}))

const titleStyle = computed(() => ({
  color: hov.value ? cfg.value.color : 'var(--text)',
}))

const artStyle = computed(() => ({
  background: `repeating-linear-gradient(48deg, ${cfg.value.color}09, ${cfg.value.color}09 2px, transparent 2px, transparent 14px)`,
  border: `1px solid ${cfg.value.color}18`,
}))

const connWord = computed(() => {
  const n = connCount.value
  if (n === 1) return 'связь'
  if (n < 5) return 'связи'
  return 'связей'
})
</script>

<template>
  <div
    class="card"
    :style="cardStyle"
    @click="emit('select', entity)"
    @mouseenter="hov = true"
    @mouseleave="hov = false"
  >
    <div v-if="showImg" class="card__art" :style="artStyle">
      <span class="card__art-icon" :style="{ color: cfg.color }">{{
        cfg.icon
      }}</span>
      <span class="card__art-tag" :style="{ color: cfg.color }">artwork</span>
    </div>

    <div class="card__head">
      <div class="card__title" :style="titleStyle">{{ entity.name }}</div>
      <StatusPip :status="entity.status" />
    </div>

    <TypeBadge :type="entity.type" tiny />

    <p v-if="!compact && entity.desc" class="card__desc">
      {{ entity.desc }}
    </p>

    <div class="card__tags">
      <EntityTag v-for="t in entity.tags.slice(0, 3)" :key="t" :label="t" />
    </div>

    <div
      v-if="connCount > 0"
      class="card__conn"
    >
      <span class="card__conn-icon" :style="{ color: cfg.color }">◈</span>
      {{ connCount }} {{ connWord }}
    </div>
  </div>
</template>

<style scoped>
.card {
  break-inside: avoid;
  margin-bottom: 18px;
  border-radius: 12px;
  padding: 18px;
  cursor: pointer;
  transition: all 0.22s cubic-bezier(0.2, 0.8, 0.2, 1);
}

.card__art {
  height: 100px;
  border-radius: 8px;
  margin-bottom: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
}

.card__art-icon {
  font-size: 28px;
  opacity: 0.18;
}

.card__art-tag {
  position: absolute;
  bottom: 6px;
  right: 10px;
  font-size: 11px;
  opacity: 0.35;
  font-family: monospace;
}

.card__head {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 8px;
  margin-bottom: 10px;
}

.card__title {
  font-family: var(--font-display);
  font-size: 16px;
  font-weight: 600;
  line-height: 1.3;
  flex: 1;
  transition: color 0.2s;
}

.card__desc {
  margin-top: 11px;
  font-size: 15px;
  color: #6b5235;
  line-height: 1.55;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  font-style: italic;
}

.card__tags {
  display: flex;
  flex-wrap: wrap;
  gap: 5px;
  margin-top: 11px;
}

.card__conn {
  margin-top: 11px;
  font-size: 13px;
  color: var(--dim);
  display: flex;
  align-items: center;
  gap: 5px;
}

.card__conn-icon {
  opacity: 0.5;
}
</style>
