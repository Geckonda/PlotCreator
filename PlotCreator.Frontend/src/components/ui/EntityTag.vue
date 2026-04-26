<script setup lang="ts">
defineProps<{
  label: string
}>()
</script>

<template>
  <span class="tag">#{{ label }}</span>
</template>

<style scoped>
.tag {
  font-size: 11px;
  padding: 2px 7px;
  border-radius: 4px;
  background: rgba(255, 255, 255, 0.04);
  color: #7056a5;
  border: 1px solid rgba(255, 255, 255, 0.06);
  white-space: nowrap;
}
</style>
