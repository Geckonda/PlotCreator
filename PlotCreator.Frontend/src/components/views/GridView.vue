<script setup lang="ts">
import { computed } from 'vue'
import type { Entity, Relation } from '@/types/entity'
import { useEntityTypesStore } from '@/stores/entityTypes'
import EntityCard from '@/components/entity/EntityCard.vue'

const props = defineProps<{
  entities: Entity[]
  relations: Relation[]
  filterType: string | null
}>()

const emit = defineEmits<{
  (e: 'select', entity: Entity): void
}>()

const types = useEntityTypesStore()
const cfg = computed(() => (props.filterType ? types.display(props.filterType) : null))

const items = computed(() =>
  props.filterType
    ? props.entities.filter((e) => e.typeKey === props.filterType)
    : props.entities,
)
</script>

<template>
  <div class="grid">
    <header class="grid__head">
      <span v-if="cfg" class="grid__icon" :style="{ color: cfg.color }">
        {{ cfg.icon }}
      </span>
      <span class="grid__title">{{ cfg ? cfg.label : 'Все сущности' }}</span>
      <span class="grid__count">{{ items.length }} записей</span>
    </header>

    <div v-if="items.length" class="masonry">
      <div
        v-for="(entity, i) in items"
        :key="entity.id"
        class="masonry__item"
        :style="{ animation: `fadeUp 0.35s ${i * 0.04}s both` }"
      >
        <EntityCard
          :entity="entity"
          :relations="relations"
          @select="emit('select', $event)"
        />
      </div>
    </div>

    <div v-else class="grid__empty">
      Нет записей в этой категории
    </div>
  </div>
</template>

<style scoped>
.grid {
  flex: 1;
  overflow-y: auto;
  padding: 28px;
}

.grid__head {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 22px;
}

.grid__icon {
  font-size: 22px;
}

.grid__title {
  font-family: var(--font-display);
  font-size: 22px;
  font-weight: 600;
}

.grid__count {
  font-size: 13px;
  color: var(--muted);
  margin-left: auto;
}

.masonry {
  columns: 3 280px;
  column-gap: 18px;
}

.masonry__item {
  break-inside: avoid;
  margin-bottom: 18px;
}

.grid__empty {
  text-align: center;
  padding: 70px 24px;
  color: var(--dim);
  font-style: italic;
  font-size: 15px;
}
</style>
