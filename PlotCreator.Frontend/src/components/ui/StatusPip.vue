<script setup lang="ts">
import { computed } from 'vue'
import type { EntityStatus } from '@/types/entity'
import { STATUSES } from '@/config/statuses'

const props = defineProps<{
  status: EntityStatus
  withLabel?: boolean
}>()

const cfg = computed(() => STATUSES[props.status] ?? STATUSES.draft)

const dotStyle = computed(() => ({
  background: cfg.value.color,
  boxShadow: `0 0 6px ${cfg.value.color}80`,
}))
</script>

<template>
  <span class="pip">
    <span class="pip__dot" :style="dotStyle" />
    <span v-if="withLabel" class="pip__label" :style="{ color: cfg.color }">
      {{ cfg.label }}
    </span>
  </span>
</template>

<style scoped>
.pip {
  display: inline-flex;
  align-items: center;
  gap: 5px;
}

.pip__dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  display: inline-block;
  flex-shrink: 0;
}

.pip__label {
  font-size: 11px;
}
</style>
