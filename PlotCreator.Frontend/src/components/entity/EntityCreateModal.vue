<script setup lang="ts">
import { computed, reactive } from 'vue'
import type { EntityStatus, EntityType } from '@/types/entity'
import type { EntityCreatePayload } from '@/types/api'
import { ENTITY_TYPE_LIST, ENTITY_TYPES } from '@/config/entityTypes'
import { STATUS_LIST } from '@/config/statuses'

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'save', payload: EntityCreatePayload): void
}>()

interface Form {
  type: EntityType
  name: string
  desc: string
  tags: string
  status: EntityStatus
}

const form = reactive<Form>({
  type: 'character',
  name: '',
  desc: '',
  tags: '',
  status: 'draft',
})

const cfg = computed(() => ENTITY_TYPES[form.type])

const dialogStyle = computed(() => ({
  border: `1px solid ${cfg.value.color}3a`,
  boxShadow: `0 32px 80px rgba(74, 44, 26, 0.25), 0 0 60px ${cfg.value.color}10`,
}))

const headerStyle = computed(() => ({
  background: `linear-gradient(135deg, ${cfg.value.color}10, transparent)`,
}))

const submitStyle = computed(() => ({
  background: `${cfg.value.color}24`,
  border: `1px solid ${cfg.value.color}55`,
  color: cfg.value.color,
}))

function typeButtonStyle(type: EntityType) {
  const active = form.type === type
  const c = ENTITY_TYPES[type].color
  return {
    background: active ? `${c}22` : 'rgba(101, 67, 33, 0.04)',
    border: `1px solid ${active ? c + '55' : 'rgba(101, 67, 33, 0.12)'}`,
    color: active ? c : 'var(--muted)',
  }
}

function close() {
  emit('close')
}

function submit() {
  if (!form.name.trim()) return
  const payload: EntityCreatePayload = {
    type: form.type,
    name: form.name.trim(),
    desc: form.desc.trim() || undefined,
    tags: form.tags
      .split(',')
      .map((t) => t.trim())
      .filter(Boolean),
    status: form.status,
  }
  emit('save', payload)
}
</script>

<template>
  <div class="backdrop" @click.self="close">
    <div class="dialog" :style="dialogStyle">
      <header class="dialog__head" :style="headerStyle">
        <div>
          <div class="dialog__title">Новая сущность</div>
          <div class="dialog__sub">Мир Эред'Халь</div>
        </div>
        <button class="dialog__close" @click="close">×</button>
      </header>

      <div class="dialog__body">
        <div class="field">
          <label>Тип сущности</label>
          <div class="type-grid">
            <button
              v-for="[type, c] in ENTITY_TYPE_LIST"
              :key="type"
              type="button"
              class="type-btn"
              :style="typeButtonStyle(type)"
              @click="form.type = type"
            >
              {{ c.icon }} {{ c.label }}
            </button>
          </div>
        </div>

        <div class="field">
          <label>Название</label>
          <input
            v-model="form.name"
            placeholder="Введите название…"
            autofocus
            @keydown.enter="submit"
          />
        </div>

        <div class="field">
          <label>Описание</label>
          <textarea
            v-model="form.desc"
            rows="3"
            placeholder="Краткое описание…"
            class="field__textarea"
          />
        </div>

        <div class="field-row">
          <div class="field">
            <label>Теги</label>
            <input v-model="form.tags" placeholder="тег1, тег2…" />
          </div>
          <div class="field">
            <label>Статус</label>
            <select v-model="form.status">
              <option v-for="[k, v] in STATUS_LIST" :key="k" :value="k">
                {{ v.label }}
              </option>
            </select>
          </div>
        </div>
      </div>

      <footer class="dialog__foot">
        <button class="btn-cancel" @click="close">Отмена</button>
        <button
          class="btn-submit"
          :style="submitStyle"
          :disabled="!form.name.trim()"
          @click="submit"
        >
          Создать
        </button>
      </footer>
    </div>
  </div>
</template>

<style scoped>
.backdrop {
  position: fixed;
  inset: 0;
  background: rgba(58, 36, 20, 0.55);
  z-index: 200;
  display: flex;
  align-items: center;
  justify-content: center;
  animation: fadeIn 0.2s ease;
}

.dialog {
  width: 540px;
  max-width: calc(100vw - 32px);
  background: var(--card);
  border-radius: 14px;
  overflow: hidden;
  animation: fadeUp 0.3s cubic-bezier(0.2, 0.8, 0.2, 1);
}

.dialog__head {
  padding: 22px 26px 18px;
  border-bottom: 1px solid var(--border);
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.dialog__title {
  font-family: var(--font-display);
  font-size: 18px;
  font-weight: 600;
  margin-bottom: 4px;
}

.dialog__sub {
  font-size: 13px;
  color: var(--muted);
  font-style: italic;
}

.dialog__close {
  color: var(--muted);
  font-size: 24px;
  line-height: 1;
  padding: 0 4px;
  margin-top: 2px;
  transition: color 0.15s;
}

.dialog__close:hover {
  color: var(--text);
}

.dialog__body {
  padding: 22px 26px;
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 7px;
}

.field label {
  font-size: 12px;
  color: var(--muted);
  font-family: var(--font-display);
  letter-spacing: 0.08em;
  text-transform: uppercase;
}

.field__textarea {
  resize: vertical;
  min-height: 70px;
  font-family: var(--font-serif);
}

.field-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.type-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 7px;
}

.type-btn {
  padding: 7px 14px;
  border-radius: 20px;
  font-size: 13px;
  font-family: var(--font-display);
  transition: all 0.15s;
}

.dialog__foot {
  padding: 18px 26px;
  border-top: 1px solid var(--border);
  display: flex;
  gap: 10px;
  justify-content: flex-end;
}

.btn-cancel {
  padding: 10px 22px;
  border-radius: 8px;
  font-size: 13px;
  font-family: var(--font-display);
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
  transition: all 0.15s;
}

.btn-cancel:hover {
  color: var(--text);
  border-color: var(--border2);
}

.btn-submit {
  padding: 10px 26px;
  border-radius: 8px;
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  transition: filter 0.15s;
}

.btn-submit:hover:not(:disabled) {
  filter: brightness(1.1);
}

.btn-submit:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}
</style>
