<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useWorldsStore } from '@/stores/worlds'
import type { World } from '@/types/world'
import WorldCard from '@/components/home/WorldCard.vue'
import NewWorldCard from '@/components/home/NewWorldCard.vue'

const router = useRouter()
const worldsStore = useWorldsStore()
const { worlds, loading, error } = storeToRefs(worldsStore)

onMounted(() => worldsStore.fetchAll())

function enterWorld(world: World) {
  router.push({ name: 'world', params: { id: String(world.id) } })
}

async function newWorld() {
  const name = window.prompt('Название нового мира')
  if (!name) return
  try {
    const w = await worldsStore.create({ name, color: '#7a4824' })
    router.push({ name: 'world', params: { id: String(w.id) } })
  } catch (err) {
    console.error('Create world failed', err)
  }
}
</script>

<template>
  <div class="home">
    <header class="home__head">
      <div class="home__brand">WORLDFORGE</div>
      <div class="home__sub">Конструктор миров</div>
    </header>

    <main class="home__main">
      <section class="home__intro">
        <h1 class="home__title">Ваши миры</h1>
        <p class="home__lede">Выберите мир или создайте новый</p>
      </section>

      <div v-if="loading" class="home__state">Загрузка…</div>
      <div v-else-if="error" class="home__state home__state--err">{{ error }}</div>

      <div v-else class="home__grid">
        <div
          v-for="(w, i) in worlds"
          :key="w.id"
          class="home__cell"
          :style="{ animation: `fadeUp 0.4s ${i * 0.1 + 0.1}s both` }"
        >
          <WorldCard :world="w" @enter="enterWorld" />
        </div>

        <div class="home__cell" :style="{ animation: 'fadeUp 0.4s 0.3s both' }">
          <NewWorldCard @click="newWorld" />
        </div>
      </div>
    </main>
  </div>
</template>

<style scoped>
.home {
  position: fixed;
  inset: 0;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  background: radial-gradient(
      ellipse at 25% 25%,
      rgba(122, 72, 36, 0.06) 0%,
      transparent 55%
    ),
    radial-gradient(
      ellipse at 80% 75%,
      rgba(61, 107, 74, 0.04) 0%,
      transparent 45%
    ),
    var(--bg);
}

.home__head {
  padding: 28px 48px;
  border-bottom: 1px solid var(--border);
}

.home__brand {
  font-family: var(--font-display);
  font-size: 24px;
  font-weight: 700;
  letter-spacing: 0.14em;
  color: var(--accent);
}

.home__sub {
  font-size: 13px;
  color: var(--dim);
  margin-top: 4px;
  font-style: italic;
}

.home__main {
  flex: 1;
  overflow: auto;
  padding: 48px;
}

.home__intro {
  margin-bottom: 40px;
  animation: fadeUp 0.4s both;
}

.home__title {
  font-family: var(--font-display);
  font-size: 32px;
  font-weight: 600;
  margin-bottom: 8px;
  line-height: 1.2;
}

.home__lede {
  color: var(--muted);
  font-size: 16px;
  font-style: italic;
}

.home__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 22px;
  max-width: 960px;
}

.home__state {
  color: var(--muted);
  font-style: italic;
}

.home__state--err {
  color: #a02929;
}
</style>
