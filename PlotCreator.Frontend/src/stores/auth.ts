import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import * as authApi from '@/services/auth'
import type { AuthUser, LoginRequest, RegisterRequest } from '@/types/auth'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<AuthUser | null>(null)
  const ready = ref(false)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => user.value !== null)

  async function fetchMe(): Promise<AuthUser | null> {
    loading.value = true
    error.value = null
    try {
      user.value = await authApi.me()
      return user.value
    } catch {
      user.value = null
      return null
    } finally {
      loading.value = false
      ready.value = true
    }
  }

  async function login(body: LoginRequest) {
    loading.value = true
    error.value = null
    try {
      user.value = await authApi.login(body)
      return user.value
    } catch (e: unknown) {
      user.value = null
      error.value = extractError(e) ?? 'Не удалось войти'
      throw e
    } finally {
      loading.value = false
    }
  }

  async function register(body: RegisterRequest) {
    loading.value = true
    error.value = null
    try {
      user.value = await authApi.register(body)
      return user.value
    } catch (e: unknown) {
      user.value = null
      error.value = extractError(e) ?? 'Не удалось зарегистрироваться'
      throw e
    } finally {
      loading.value = false
    }
  }

  async function logout() {
    try {
      await authApi.logout()
    } catch {
      // ignore — clearing local state is what matters
    }
    user.value = null
  }

  function clearLocalSession() {
    user.value = null
  }

  return {
    user,
    ready,
    loading,
    error,
    isAuthenticated,
    fetchMe,
    login,
    register,
    logout,
    clearLocalSession,
  }
})

function extractError(e: unknown): string | null {
  if (typeof e === 'object' && e !== null && 'response' in e) {
    const resp = (e as { response?: { data?: { errorForUser?: string; description?: string } } }).response
    return resp?.data?.errorForUser ?? resp?.data?.description ?? null
  }
  if (e instanceof Error) return e.message
  return null
}
