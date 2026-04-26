<script setup lang="ts">
import { computed, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useWorldsStore } from '@/stores/worlds'
import { useEntitiesStore } from '@/stores/entities'

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
</script>

<template>
  <section>
    <h2>{{ worldsStore.current?.name ?? worldId }}</h2>
    <p v-if="worldsStore.current">
      <em>{{ worldsStore.current.genre }}</em> —
      {{ worldsStore.current.description }}
    </p>
    <p>
      {{ entitiesStore.entities.length }} entities,
      {{ entitiesStore.relations.length }} relations.
    </p>
    <p>Editor UI lands in step 3 (sidebar + topbar + grid view).</p>
  </section>
</template>
