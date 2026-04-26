<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header class="app-header">
    <h1 class="app-title">PlotCreator</h1>
    <nav class="app-nav">
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/world/ered-hal">World</RouterLink>
      <RouterLink to="/about">About</RouterLink>
    </nav>
  </header>

  <main class="app-main">
    <RouterView />
  </main>
</template>

<style scoped>
.app-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 2rem;
  border-bottom: 1px solid var(--border);
}

.app-title {
  margin: 0;
  font-family: var(--font-display);
  font-size: 1.5rem;
  font-weight: 700;
  letter-spacing: 0.14em;
  color: var(--accent);
}

.app-nav {
  display: flex;
  gap: 1.5rem;
}

.app-nav a {
  color: var(--muted);
  text-decoration: none;
  font-family: var(--font-display);
  font-size: 1rem;
  letter-spacing: 0.04em;
  transition: color 0.15s;
}

.app-nav a:hover {
  color: var(--text);
}

.app-nav a.router-link-active {
  color: var(--accent);
}

.app-main {
  padding: 2rem;
  max-width: 960px;
  margin: 0 auto;
}
</style>
