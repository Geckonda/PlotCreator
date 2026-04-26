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
  font-size: 13px;
  padding: 3px 9px;
  border-radius: 5px;
  background: rgba(101, 67, 33, 0.06);
  color: #8a6f3d;
  border: 1px solid rgba(101, 67, 33, 0.12);
  white-space: nowrap;
}
</style>
