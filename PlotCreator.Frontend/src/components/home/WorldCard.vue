<script setup lang="ts">
import { computed, ref } from 'vue'
import type { World } from '@/types/world'

const props = defineProps<{
  world: World
}>()

const emit = defineEmits<{
  (e: 'enter', world: World): void
}>()

const hov = ref(false)

const cardStyle = computed(() => {
  const c = props.world.color
  return {
    border: `1px solid ${hov.value && props.world.active ? c + '5a' : c + '28'}`,
    transform: hov.value && props.world.active ? 'translateY(-4px)' : 'none',
    boxShadow:
      hov.value && props.world.active
        ? `0 18px 44px ${c}1f, 0 4px 10px rgba(74, 44, 26, 0.10)`
        : '0 2px 6px rgba(74, 44, 26, 0.08)',
    opacity: props.world.active ? 1 : 0.5,
    cursor: props.world.active ? 'pointer' : 'default',
  }
})

const bannerStyle = computed(() => {
  const c = props.world.color
  return {
    background: `repeating-linear-gradient(55deg, ${c}10, ${c}10 2px, transparent 2px, transparent 18px), repeating-linear-gradient(-55deg, ${c}08, ${c}08 2px, transparent 2px, transparent 18px)`,
  }
})

function onClick() {
  if (props.world.active) emit('enter', props.world)
}
</script>

<template>
  <div
    class="card"
    :style="cardStyle"
    @click="onClick"
    @mouseenter="hov = true"
    @mouseleave="hov = false"
  >
    <div class="card__banner" :style="bannerStyle">
      <span class="card__banner-glyph" :style="{ color: world.color }">◈</span>
      <span class="card__banner-tag" :style="{ color: world.color }">
        world art
      </span>
    </div>

    <div class="card__body">
      <div class="card__head">
        <div class="card__name">{{ world.name }}</div>
        <span v-if="world.active" class="card__active-badge">АКТИВНЫЙ</span>
      </div>
      <div class="card__genre" :style="{ color: world.color }">
        {{ world.genre }}
      </div>
      <p class="card__desc">{{ world.description }}</p>
      <div class="card__count">{{ world.entitiesCount }} сущностей</div>
    </div>
  </div>
</template>

<style scoped>
.card {
  background: var(--card);
  border-radius: 14px;
  overflow: hidden;
  transition: all 0.25s cubic-bezier(0.2, 0.8, 0.2, 1);
}

.card__banner {
  height: 140px;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
}

.card__banner-glyph {
  font-size: 48px;
  opacity: 0.18;
  font-family: var(--font-display);
}

.card__banner-tag {
  position: absolute;
  bottom: 9px;
  right: 14px;
  font-size: 11px;
  opacity: 0.45;
  font-family: monospace;
}

.card__body {
  padding: 18px 20px 20px;
}

.card__head {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 6px;
}

.card__name {
  font-family: var(--font-display);
  font-size: 18px;
  font-weight: 600;
}

.card__active-badge {
  font-size: 11px;
  color: #3d6b4a;
  border: 1px solid rgba(61, 107, 74, 0.4);
  border-radius: 99px;
  padding: 2px 9px;
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  background: rgba(61, 107, 74, 0.06);
}

.card__genre {
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.08em;
  margin-bottom: 10px;
  text-transform: uppercase;
}

.card__desc {
  font-size: 15px;
  color: var(--muted);
  line-height: 1.6;
  margin-bottom: 12px;
  font-style: italic;
}

.card__count {
  font-size: 13px;
  color: var(--dim);
}
</style>
