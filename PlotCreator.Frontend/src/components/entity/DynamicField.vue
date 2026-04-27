<script setup lang="ts">
import { computed } from 'vue'
import type { FieldDef } from '@/config/entityFields'

const props = defineProps<{
  def: FieldDef
  modelValue: unknown
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: unknown): void
}>()

const stringValue = computed(() =>
  props.modelValue === null || props.modelValue === undefined
    ? ''
    : String(props.modelValue),
)

const boolValue = computed(() => Boolean(props.modelValue))

function emitText(ev: Event) {
  const v = (ev.target as HTMLInputElement | HTMLTextAreaElement).value
  emit('update:modelValue', v === '' ? null : v)
}

function emitNumber(ev: Event) {
  const v = (ev.target as HTMLInputElement).value
  if (v === '') return emit('update:modelValue', null)
  const n = Number(v)
  emit('update:modelValue', Number.isNaN(n) ? null : n)
}

function emitDate(ev: Event) {
  const v = (ev.target as HTMLInputElement).value
  emit('update:modelValue', v === '' ? null : v)
}

function emitCheckbox(ev: Event) {
  emit('update:modelValue', (ev.target as HTMLInputElement).checked)
}
</script>

<template>
  <div class="field">
    <label>{{ def.label }}</label>

    <input
      v-if="def.kind === 'text'"
      type="text"
      :value="stringValue"
      @input="emitText"
    />

    <textarea
      v-else-if="def.kind === 'textarea'"
      rows="3"
      :value="stringValue"
      @input="emitText"
    />

    <input
      v-else-if="def.kind === 'number'"
      type="number"
      :value="stringValue"
      @input="emitNumber"
    />

    <input
      v-else-if="def.kind === 'date'"
      type="date"
      :value="stringValue"
      @input="emitDate"
    />

    <label v-else-if="def.kind === 'checkbox'" class="checkbox">
      <input type="checkbox" :checked="boolValue" @change="emitCheckbox" />
      <span>{{ def.label }}</span>
    </label>
  </div>
</template>

<style scoped>
.field {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.field > label {
  font-size: 11px;
  color: var(--muted);
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.field input[type='text'],
.field input[type='number'],
.field input[type='date'],
.field textarea {
  width: 100%;
  font-family: var(--font-serif);
  font-size: 14px;
  background: rgba(255, 255, 255, 0.6);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 7px 9px;
}

.field textarea {
  resize: vertical;
  min-height: 60px;
}

.checkbox {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13px;
  color: var(--muted);
}
</style>
