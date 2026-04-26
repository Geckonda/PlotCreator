<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header class="app-header">
    <h1 class="app-title">PlotCreator</h1>
    <nav class="app-nav">
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/books">Books</RouterLink>
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
  border-bottom: 1px solid var(--color-border);
}

.app-title {
  margin: 0;
  font-size: 1.25rem;
  font-weight: 600;
}

.app-nav {
  display: flex;
  gap: 1.25rem;
}

.app-nav a {
  color: var(--color-text);
  text-decoration: none;
}

.app-nav a.router-link-active {
  color: var(--color-accent);
  font-weight: 600;
}

.app-main {
  padding: 2rem;
  max-width: 960px;
  margin: 0 auto;
}
</style>
