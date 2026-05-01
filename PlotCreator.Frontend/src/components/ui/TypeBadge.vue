<script setup lang="ts">
import { computed } from 'vue'
import { useEntityTypesStore } from '@/stores/entityTypes'

const props = defineProps<{
  typeKey: string
  tiny?: boolean
}>()

const types = useEntityTypesStore()
const cfg = computed(() => types.display(props.typeKey))

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
  gap: 5px;
  font-size: 13px;
  padding: 3px 10px;
  border-radius: 99px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  white-space: nowrap;
  line-height: 1.4;
}

.badge--tiny {
  font-size: 12px;
  padding: 2px 8px;
}
</style>
