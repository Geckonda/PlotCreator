<script setup lang="ts">
import { computed } from 'vue'
import type { EntityType } from '@/types/entity'
import { ENTITY_TYPES } from '@/config/entityTypes'

const props = defineProps<{
  type: EntityType
  tiny?: boolean
}>()

const cfg = computed(() => ENTITY_TYPES[props.type])

const style = computed(() => ({
  background: `${cfg.value.color}1a`,
  color: cfg.value.color,
  border: `1px solid ${cfg.value.color}33`,
}))
</script>

<template>
  <span class="badge" :class="{ 'badge--tiny': tiny }" :style="style">
    <span class="badge__icon">{{ cfg.icon }}</span>
    <span class="badge__label">{{ cfg.label }}</span>
  </span>
</template>

<style scoped>
.badge {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  font-size: 11px;
  padding: 2px 8px;
  border-radius: 99px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  white-space: nowrap;
  line-height: 1.4;
}

.badge--tiny {
  font-size: 10px;
  padding: 1px 6px;
}
</style>
