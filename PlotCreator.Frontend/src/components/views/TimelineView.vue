<script setup lang="ts">
import { computed } from 'vue'
import type { Entity } from '@/types/entity'
import { ENTITY_TYPES } from '@/config/entityTypes'
import TypeBadge from '@/components/ui/TypeBadge.vue'

const props = defineProps<{
  entities: Entity[]
}>()

const TIME_PATTERN = /лет|год|недавно|глава/i

const events = computed(() =>
  props.entities.filter((e) => e.type === 'event' || e.type === 'episode'),
)

const lore = computed(() => props.entities.filter((e) => e.type === 'lore'))

function timeTag(entity: Entity): string {
  return entity.tags.find((t) => TIME_PATTERN.test(t)) ?? '—'
}

function dotStyle(entity: Entity, square = false) {
  const color = ENTITY_TYPES[entity.type].color
  const base = {
    background: color,
    boxShadow: `0 0 10px ${color}88`,
  }
  return square
    ? { ...base, borderRadius: '2px', transform: 'rotate(45deg)' }
    : base
}
</script>

<template>
  <div class="timeline">
    <header class="timeline__head">
      <span class="timeline__icon">◆</span>
      Хронология
    </header>

    <div class="timeline__track">
      <div class="timeline__spine" />

      <div
        v-for="(e, i) in events"
        :key="e.id"
        class="entry"
        :style="{ animation: `fadeUp 0.35s ${i * 0.07}s both` }"
      >
        <div class="entry__time">{{ timeTag(e) }}</div>
        <div class="entry__dot" :style="dotStyle(e)" />
        <div class="entry__body">
          <div class="entry__name">{{ e.name }}</div>
          <TypeBadge :type="e.type" tiny />
          <p v-if="e.desc" class="entry__desc">{{ e.desc }}</p>
        </div>
      </div>

      <div
        v-for="(e, i) in lore"
        :key="e.id"
        class="entry entry--faded"
        :style="{
          animation: `fadeUp 0.35s ${(events.length + i) * 0.07}s both`,
        }"
      >
        <div class="entry__time">{{ timeTag(e) }}</div>
        <div class="entry__dot" :style="dotStyle(e, true)" />
        <div class="entry__body">
          <div class="entry__name">{{ e.name }}</div>
          <TypeBadge :type="e.type" tiny />
          <p v-if="e.desc" class="entry__desc">{{ e.desc }}</p>
        </div>
      </div>

      <div v-if="!events.length && !lore.length" class="timeline__empty">
        Нет событий, эпизодов или лора
      </div>
    </div>
  </div>
</template>

<style scoped>
.timeline {
  flex: 1;
  overflow-y: auto;
  padding: 28px 56px;
}

.timeline__head {
  font-family: var(--font-display);
  font-size: 22px;
  font-weight: 600;
  margin-bottom: 32px;
  display: flex;
  align-items: center;
  gap: 12px;
}

.timeline__icon {
  color: #b8860b;
}

.timeline__track {
  position: relative;
  max-width: 760px;
}

.timeline__spine {
  position: absolute;
  left: 160px;
  top: 8px;
  bottom: 8px;
  width: 1px;
  background: linear-gradient(
    to bottom,
    transparent,
    rgba(101, 67, 33, 0.32) 8%,
    rgba(101, 67, 33, 0.32) 92%,
    transparent
  );
}

.entry {
  display: flex;
  align-items: flex-start;
  margin-bottom: 28px;
  position: relative;
}

.entry--faded {
  opacity: 0.65;
}

.entry__time {
  width: 160px;
  text-align: right;
  padding-right: 24px;
  padding-top: 4px;
  flex-shrink: 0;
  font-size: 13px;
  color: var(--muted);
  font-style: italic;
  line-height: 1.4;
}

.entry__dot {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  flex-shrink: 0;
  margin-top: 6px;
  margin-left: -6px;
  z-index: 1;
}

.entry__body {
  padding-left: 22px;
  flex: 1;
}

.entry__name {
  font-family: var(--font-display);
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 7px;
}

.entry__desc {
  font-size: 15px;
  color: var(--muted);
  line-height: 1.6;
  margin-top: 9px;
  font-style: italic;
}

.timeline__empty {
  text-align: center;
  padding: 60px 20px;
  color: var(--dim);
  font-style: italic;
  font-size: 15px;
}
</style>
