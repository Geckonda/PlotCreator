<script setup lang="ts">
import { computed, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useWorldsStore } from '@/stores/worlds'
import { useEntitiesStore } from '@/stores/entities'
import { ENTITY_TYPE_LIST } from '@/config/entityTypes'
import { STATUS_LIST } from '@/config/statuses'
import TypeBadge from '@/components/ui/TypeBadge.vue'
import StatusPip from '@/components/ui/StatusPip.vue'
import EntityTag from '@/components/ui/EntityTag.vue'

const route = useRoute()
const worldsStore = useWorldsStore()
const entitiesStore = useEntitiesStore()

const worldId = computed(() => String(route.params.id))

watch(
  worldId,
  (id) => {
    worldsStore.setCurrent(id)
  },
  { immediate: true },
)

const previewEntities = computed(() => entitiesStore.entities.slice(0, 4))
</script>

<template>
  <section class="world">
    <header class="world__head">
      <h2>{{ worldsStore.current?.name ?? worldId }}</h2>
      <p v-if="worldsStore.current" class="world__meta">
        <em>{{ worldsStore.current.genre }}</em> —
        {{ worldsStore.current.description }}
      </p>
      <p class="world__counts">
        {{ entitiesStore.entities.length }} entities ·
        {{ entitiesStore.relations.length }} relations
      </p>
    </header>

    <section class="showcase">
      <h3 class="showcase__title">Atoms preview</h3>

      <div class="showcase__row">
        <span class="showcase__label">TypeBadge</span>
        <TypeBadge v-for="[type] in ENTITY_TYPE_LIST" :key="type" :type="type" />
      </div>

      <div class="showcase__row">
        <span class="showcase__label">TypeBadge tiny</span>
        <TypeBadge
          v-for="[type] in ENTITY_TYPE_LIST"
          :key="type"
          :type="type"
          tiny
        />
      </div>

      <div class="showcase__row">
        <span class="showcase__label">StatusPip</span>
        <StatusPip
          v-for="[status] in STATUS_LIST"
          :key="status"
          :status="status"
          with-label
        />
      </div>

      <div class="showcase__row">
        <span class="showcase__label">EntityTag</span>
        <EntityTag
          v-for="t in ['протагонист', 'маг', 'изгнанница']"
          :key="t"
          :label="t"
        />
      </div>
    </section>

    <section class="showcase">
      <h3 class="showcase__title">Sample entities</h3>
      <ul class="entity-list">
        <li v-for="e in previewEntities" :key="e.id">
          <div class="entity-list__head">
            <strong>{{ e.name }}</strong>
            <StatusPip :status="e.status" />
          </div>
          <div class="entity-list__row">
            <TypeBadge :type="e.type" tiny />
            <EntityTag v-for="t in e.tags" :key="t" :label="t" />
          </div>
        </li>
      </ul>
    </section>

    <p class="next">Next: step 3 — sidebar + topbar + grid view.</p>
  </section>
</template>

<style scoped>
.world__head h2 {
  font-family: var(--font-display);
  font-size: 1.5rem;
  letter-spacing: 0.06em;
  margin-bottom: 6px;
}

.world__meta {
  color: var(--muted);
  font-style: italic;
  margin-bottom: 4px;
}

.world__counts {
  color: var(--dim);
  font-size: 0.85rem;
}

.showcase {
  margin-top: 2rem;
  padding: 18px;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 10px;
}

.showcase__title {
  font-family: var(--font-display);
  font-size: 0.85rem;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: var(--muted);
  margin-bottom: 14px;
}

.showcase__row {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 8px;
  padding: 6px 0;
}

.showcase__label {
  font-family: var(--font-display);
  font-size: 0.7rem;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--dim);
  width: 130px;
  flex-shrink: 0;
}

.entity-list {
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.entity-list__head {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 6px;
  font-family: var(--font-display);
}

.entity-list__row {
  display: flex;
  flex-wrap: wrap;
  gap: 6px;
}

.next {
  margin-top: 2rem;
  color: var(--dim);
  font-style: italic;
  font-size: 0.85rem;
}
</style>
