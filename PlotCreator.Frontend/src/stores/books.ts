import { defineStore } from 'pinia'
import { ref } from 'vue'
import { booksApi, type Book } from '@/api/books'

export const useBooksStore = defineStore('books', () => {
  const books = ref<Book[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchBooks() {
    loading.value = true
    error.value = null
    try {
      books.value = await booksApi.list()
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to load books'
    } finally {
      loading.value = false
    }
  }

  return { books, loading, error, fetchBooks }
})
