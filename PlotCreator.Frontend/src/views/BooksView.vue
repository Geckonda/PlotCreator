<script setup lang="ts">
import { onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useBooksStore } from '@/stores/books'

const store = useBooksStore()
const { books, loading, error } = storeToRefs(store)

onMounted(() => {
  store.fetchBooks()
})
</script>

<template>
  <section>
    <h2>Books</h2>

    <p v-if="loading">Loading...</p>
    <p v-else-if="error" class="error">{{ error }}</p>
    <ul v-else-if="books.length" class="book-list">
      <li v-for="book in books" :key="book.id">
        <strong>{{ book.name }}</strong>
        <span v-if="book.description"> — {{ book.description }}</span>
      </li>
    </ul>
    <p v-else>No books yet.</p>
  </section>
</template>

<style scoped>
.book-list {
  list-style: none;
  padding: 0;
}

.book-list li {
  padding: 0.5rem 0;
  border-bottom: 1px solid var(--color-border);
}

.error {
  color: #c0392b;
}
</style>
