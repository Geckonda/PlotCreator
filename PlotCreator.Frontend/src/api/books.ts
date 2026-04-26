import apiClient from './client'

export interface Book {
  id: number
  name: string
  description?: string
}

export const booksApi = {
  list(): Promise<Book[]> {
    return apiClient.get<Book[]>('/books').then((r) => r.data)
  },
  get(id: number): Promise<Book> {
    return apiClient.get<Book>(`/books/${id}`).then((r) => r.data)
  },
}
