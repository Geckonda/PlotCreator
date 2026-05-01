<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const router = useRouter()

const form = reactive({
  nickname: '',
  login: '',
  email: '',
  password: '',
  passwordConfirm: '',
})
const submitting = ref(false)
const localError = ref<string | null>(null)

const passwordMismatch = computed(
  () =>
    form.password.length > 0 &&
    form.passwordConfirm.length > 0 &&
    form.password !== form.passwordConfirm,
)

async function submit() {
  if (submitting.value) return
  if (passwordMismatch.value) {
    localError.value = 'Пароли не совпадают'
    return
  }
  submitting.value = true
  localError.value = null
  try {
    await auth.register({
      nickname: form.nickname.trim(),
      login: form.login.trim(),
      email: form.email.trim(),
      password: form.password,
      passwordConfirm: form.passwordConfirm,
    })
    router.replace('/')
  } catch {
    localError.value = auth.error ?? 'Не удалось зарегистрироваться'
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <div class="auth">
    <form class="auth__card" @submit.prevent="submit">
      <h1 class="auth__title">Регистрация</h1>
      <p class="auth__sub">Создайте аккаунт, чтобы начать вести свои миры.</p>

      <div class="auth__field">
        <label>Никнейм</label>
        <input
          v-model="form.nickname"
          type="text"
          autofocus
          required
          maxlength="80"
        />
      </div>

      <div class="auth__field">
        <label>Логин</label>
        <input
          v-model="form.login"
          type="text"
          autocomplete="username"
          required
          maxlength="40"
        />
      </div>

      <div class="auth__field">
        <label>Email</label>
        <input
          v-model="form.email"
          type="email"
          autocomplete="email"
          required
        />
      </div>

      <div class="auth__field">
        <label>Пароль</label>
        <input
          v-model="form.password"
          type="password"
          autocomplete="new-password"
          minlength="6"
          required
        />
      </div>

      <div class="auth__field">
        <label>Повторите пароль</label>
        <input
          v-model="form.passwordConfirm"
          type="password"
          autocomplete="new-password"
          required
        />
      </div>

      <div v-if="passwordMismatch" class="auth__err">Пароли не совпадают</div>
      <div v-else-if="localError" class="auth__err">{{ localError }}</div>

      <button
        class="auth__btn"
        type="submit"
        :disabled="submitting || passwordMismatch"
      >
        {{ submitting ? 'Создаём…' : 'Создать аккаунт' }}
      </button>

      <div class="auth__alt">
        Уже есть аккаунт?
        <RouterLink to="/login">Войти</RouterLink>
      </div>
    </form>
  </div>
</template>

<style scoped>
.auth {
  position: fixed;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
  background: var(--bg);
  overflow-y: auto;
}

.auth__card {
  width: 100%;
  max-width: 420px;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 14px;
  padding: 28px 28px 22px;
  display: flex;
  flex-direction: column;
  gap: 12px;
  box-shadow: 0 10px 30px rgba(74, 44, 26, 0.12);
  margin: 24px 0;
}

.auth__title {
  margin: 0;
  font-family: var(--font-display);
  font-size: 24px;
  font-weight: 700;
  color: var(--text);
  letter-spacing: 0.02em;
}

.auth__sub {
  margin: 0 0 4px;
  color: var(--muted);
  font-size: 13px;
}

.auth__field {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.auth__field label {
  font-family: var(--font-display);
  font-size: 11px;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--muted);
}

.auth__field input {
  width: 100%;
  font-family: var(--font-serif);
  font-size: 14px;
  background: rgba(255, 255, 255, 0.7);
  border: 1px solid var(--border);
  border-radius: 7px;
  padding: 9px 11px;
}

.auth__btn {
  margin-top: 4px;
  padding: 10px 16px;
  border-radius: 8px;
  background: rgba(122, 72, 36, 0.18);
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
  font-family: var(--font-display);
  font-size: 14px;
  letter-spacing: 0.04em;
  cursor: pointer;
  transition: filter 0.15s, opacity 0.15s;
}

.auth__btn:hover:not(:disabled) {
  filter: brightness(1.06);
}

.auth__btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.auth__err {
  padding: 8px 10px;
  background: rgba(160, 41, 41, 0.06);
  border: 1px solid rgba(160, 41, 41, 0.25);
  color: #a02929;
  font-size: 12px;
  border-radius: 6px;
}

.auth__alt {
  text-align: center;
  font-size: 13px;
  color: var(--muted);
  margin-top: 4px;
}

.auth__alt a {
  color: var(--accent);
  text-decoration: none;
}

.auth__alt a:hover {
  text-decoration: underline;
}
</style>
