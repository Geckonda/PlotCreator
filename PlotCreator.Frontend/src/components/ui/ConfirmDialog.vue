<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref } from 'vue'

const props = withDefaults(
  defineProps<{
    title?: string
    message: string
    detail?: string
    confirmLabel?: string
    cancelLabel?: string
    tone?: 'default' | 'danger'
    busy?: boolean
  }>(),
  {
    title: 'Подтверждение',
    confirmLabel: 'Удалить',
    cancelLabel: 'Отмена',
    tone: 'default',
    busy: false,
  },
)

const emit = defineEmits<{
  (e: 'confirm'): void
  (e: 'cancel'): void
}>()

const confirmRef = ref<HTMLButtonElement | null>(null)

function onKey(ev: KeyboardEvent) {
  if (props.busy) return
  if (ev.key === 'Escape') {
    ev.stopPropagation()
    emit('cancel')
  } else if (ev.key === 'Enter') {
    ev.stopPropagation()
    emit('confirm')
  }
}

onMounted(() => {
  window.addEventListener('keydown', onKey, true)
  confirmRef.value?.focus()
})

onBeforeUnmount(() => {
  window.removeEventListener('keydown', onKey, true)
})

function onBackdrop() {
  if (props.busy) return
  emit('cancel')
}
</script>

<template>
  <Teleport to="body">
    <div class="confirm-backdrop" @mousedown.self="onBackdrop">
      <div
        class="confirm"
        :class="{ 'confirm--danger': tone === 'danger' }"
        role="dialog"
        aria-modal="true"
      >
        <header class="confirm__head">{{ title }}</header>
        <div class="confirm__body">
          <p class="confirm__msg">{{ message }}</p>
          <p v-if="detail" class="confirm__detail">{{ detail }}</p>
        </div>
        <footer class="confirm__actions">
          <button
            type="button"
            class="confirm__cancel"
            :disabled="busy"
            @click="emit('cancel')"
          >
            {{ cancelLabel }}
          </button>
          <button
            ref="confirmRef"
            type="button"
            class="confirm__confirm"
            :class="{ 'confirm__confirm--danger': tone === 'danger' }"
            :disabled="busy"
            @click="emit('confirm')"
          >
            {{ busy ? '…' : confirmLabel }}
          </button>
        </footer>
      </div>
    </div>
  </Teleport>
</template>

<style scoped>
.confirm-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(40, 24, 12, 0.42);
  backdrop-filter: blur(2px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
  animation: fade 0.16s ease;
}

.confirm {
  width: 380px;
  max-width: calc(100vw - 32px);
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 12px;
  box-shadow: 0 20px 50px rgba(74, 44, 26, 0.28);
  display: flex;
  flex-direction: column;
  overflow: hidden;
  animation: lift 0.2s cubic-bezier(0.2, 0.8, 0.2, 1);
}

.confirm__head {
  font-family: var(--font-display);
  font-size: 14px;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--muted);
  padding: 14px 18px;
  border-bottom: 1px solid var(--border);
}

.confirm--danger .confirm__head {
  color: #a02929;
  background: rgba(160, 41, 41, 0.05);
  border-bottom-color: rgba(160, 41, 41, 0.2);
}

.confirm__body {
  padding: 16px 18px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.confirm__msg {
  margin: 0;
  font-size: 14px;
  font-family: var(--font-serif);
  line-height: 1.45;
  color: var(--text);
}

.confirm__detail {
  margin: 0;
  font-size: 12px;
  color: var(--muted);
  font-style: italic;
  line-height: 1.4;
}

.confirm__actions {
  padding: 12px 18px 14px;
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  border-top: 1px solid var(--border);
}

.confirm__cancel,
.confirm__confirm {
  padding: 7px 16px;
  border-radius: 6px;
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  cursor: pointer;
  transition: filter 0.15s, opacity 0.15s, background 0.15s;
}

.confirm__cancel {
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
}

.confirm__cancel:hover:not(:disabled) {
  color: var(--text);
}

.confirm__confirm {
  background: rgba(122, 72, 36, 0.18);
  border: 1px solid rgba(122, 72, 36, 0.4);
  color: #5d3a1a;
}

.confirm__confirm:hover:not(:disabled) {
  filter: brightness(1.06);
}

.confirm__confirm--danger {
  background: rgba(160, 41, 41, 0.12);
  border-color: rgba(160, 41, 41, 0.5);
  color: #a02929;
}

.confirm__confirm:disabled,
.confirm__cancel:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

@keyframes fade {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes lift {
  from {
    transform: translateY(8px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
</style>
