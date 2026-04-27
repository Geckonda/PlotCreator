<template>
  <div class="tags-input-wrap">
    <div class="tags-input" :class="{ 'tags-input--focused': focused }">
      <span
        v-for="tag in modelValue"
        :key="tag"
        class="tags-input__tag"
      >
        {{ tag }}
        <button class="tags-input__remove" @click="remove(tag)">✕</button>
      </span>
      <input
        v-model="inputValue"
        class="tags-input__input"
        :placeholder="modelValue.length === 0 ? placeholder : ''"
        @keydown.enter.prevent="add"
        @focus="focused = true"
        @blur="focused = false"
      />
    </div>
    <span class="tags-input__hint">Нажмите Enter для добавления</span>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const props = withDefaults(defineProps<{
  modelValue: string[]
  placeholder?: string
}>(), {
  placeholder: 'Введите значение...'
})

const emit = defineEmits<{
  'update:modelValue': [value: string[]]
}>()

const inputValue = ref('')
const focused = ref(false)

function add() {
  const val = inputValue.value.trim().toLowerCase()
  if (val && !props.modelValue.includes(val)) {
    emit('update:modelValue', [...props.modelValue, val])
  }
  inputValue.value = ''
}

function remove(tag: string) {
  emit('update:modelValue', props.modelValue.filter(t => t !== tag))
}
</script>

<style scoped>
.tags-input-wrap {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.tags-input {
  display: flex;
  flex-wrap: wrap;
  gap: 0.4rem;
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--border);
  border-radius: 8px;
  background: rgba(0, 0, 0, 0.03);
  transition: border-color 0.15s, box-shadow 0.15s;
  cursor: text;
  max-height: 100px;
  overflow-y: auto;
}

.tags-input--focused {
  border-color: var(--accent);
}

.tags-input__tag {
  display: inline-flex;
  align-items: center;
  gap: 0.3rem;
  background: var(--dim);
  color: var(--text);
  font-size: 0.8rem;
  font-weight: 500;
  padding: 0.2rem 0.6rem;
  border-radius: 20px;
}

.tags-input__remove {
  background: none;
  border: none;
  color: var(--text);
  font-size: 0.7rem;
  padding: 0;
  line-height: 1;
  display: flex;
  align-items: center;
  cursor: pointer;
}

.tags-input__remove:hover {
  color: var(--accent);
}

.tags-input__input {
  border: none;
  outline: none;
  flex: 1;
  color: var(--text);
  background: transparent;
}

.tags-input__hint {
  font-size: 0.78rem;
  color: var(--dim);
}
</style>