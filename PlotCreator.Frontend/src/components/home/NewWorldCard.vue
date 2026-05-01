<script setup lang="ts">
defineEmits<{
  (e: 'click'): void
}>()
</script>

<template>
  <button class="new-card" type="button" @click="$emit('click')">
    <span class="new-card__plus">+</span>
    <span class="new-card__label">Новый мир</span>
  </button>
</template>

<style scoped>
.new-card {
  background: transparent;
  border: 1px dashed rgba(122, 72, 36, 0.28);
  border-radius: 14px;
  padding: 40px 20px;
  min-height: 270px;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 12px;
  color: var(--muted);
  transition: all 0.2s;
  cursor: pointer;
}

.new-card:hover {
  border-color: rgba(122, 72, 36, 0.5);
  color: var(--accent);
  background: rgba(122, 72, 36, 0.04);
}

.new-card__plus {
  font-size: 32px;
  line-height: 1;
}

.new-card__label {
  font-family: var(--font-display);
  font-size: 14px;
  letter-spacing: 0.06em;
}
</style>
