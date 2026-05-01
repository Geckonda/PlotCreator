<script setup lang="ts">
import BlockEditor from '@/components/blocks/BlockEditor.vue'
import type { ContentSection, TipTapDoc } from '@/types/api'
import { emptyTipTapDoc } from '@/types/api'
import type { EntityRef } from '@/composables/tiptapEntityHighlight'

const props = withDefaults(
  defineProps<{
    modelValue: ContentSection[]
    entities?: EntityRef[]
    relatedIds?: ReadonlySet<number>
    excludeId?: number | null
  }>(),
  {
    entities: () => [],
    relatedIds: () => new Set<number>(),
    excludeId: null,
  },
)

const emit = defineEmits<{
  (e: 'update:modelValue', val: ContentSection[]): void
  (
    e: 'mention-click',
    payload: { entityId: number; x: number; y: number },
  ): void
}>()

function makeId(): string {
  if (typeof crypto !== 'undefined' && 'randomUUID' in crypto) {
    return crypto.randomUUID()
  }
  return `s-${Date.now()}-${Math.random().toString(36).slice(2, 9)}`
}

function setSection(index: number, updater: (s: ContentSection) => ContentSection) {
  const next = props.modelValue.map((s, i) => (i === index ? updater(s) : s))
  emit('update:modelValue', next)
}

function onTitleInput(index: number, ev: Event) {
  const value = (ev.target as HTMLInputElement).value
  setSection(index, (s) => ({ ...s, title: value }))
}

function onContentUpdate(index: number, content: TipTapDoc) {
  setSection(index, (s) => ({ ...s, content }))
}

function addBlock() {
  const next: ContentSection[] = [
    ...props.modelValue,
    { id: makeId(), title: '', content: emptyTipTapDoc() },
  ]
  emit('update:modelValue', next)
}

function removeBlock(index: number) {
  const next = props.modelValue.filter((_, i) => i !== index)
  emit('update:modelValue', next)
}

function onMention(payload: { entityId: number; x: number; y: number }) {
  emit('mention-click', payload)
}
</script>

<template>
  <div class="xblocks">
    <div
      v-for="(section, i) in modelValue"
      :key="section.id"
      class="xblock"
    >
      <header class="xblock__head">
        <input
          class="xblock__title"
          type="text"
          :value="section.title"
          :placeholder="`Блок ${i + 1}`"
          @input="onTitleInput(i, $event)"
        />
        <button
          class="xblock__del"
          type="button"
          title="Удалить блок"
          @click="removeBlock(i)"
        >
          ×
        </button>
      </header>
      <BlockEditor
        :model-value="section.content"
        :entities="entities"
        :related-ids="relatedIds"
        :exclude-id="excludeId"
        @update:model-value="onContentUpdate(i, $event)"
        @mention-click="onMention"
      />
    </div>

    <button class="xblocks__add" type="button" @click="addBlock">
      + Добавить блок
    </button>
  </div>
</template>

<style scoped>
.xblocks {
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.xblock {
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding: 12px 14px 14px;
  background: rgba(101, 67, 33, 0.04);
  border: 1px solid var(--border);
  border-radius: 10px;
}

.xblock__head {
  display: flex;
  align-items: center;
  gap: 8px;
}

.xblock__title {
  flex: 1;
  background: transparent;
  border: none;
  border-bottom: 1px dashed transparent;
  font-family: var(--font-display);
  font-size: 16px;
  font-weight: 600;
  letter-spacing: 0.02em;
  color: var(--text);
  padding: 4px 2px;
  transition: border-color 0.15s;
}

.xblock__title:focus {
  outline: none;
  border-bottom-color: rgba(101, 67, 33, 0.35);
}

.xblock__title::placeholder {
  color: var(--dim);
  font-style: italic;
}

.xblock__del {
  width: 28px;
  height: 28px;
  border-radius: 6px;
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
  font-size: 16px;
  line-height: 1;
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.xblock__del:hover {
  color: #a02929;
  border-color: rgba(160, 41, 41, 0.4);
}

.xblocks__add {
  align-self: flex-start;
  padding: 7px 14px;
  border-radius: 7px;
  background: transparent;
  border: 1px dashed var(--border2);
  color: var(--muted);
  font-family: var(--font-display);
  font-size: 12px;
  letter-spacing: 0.04em;
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s, background 0.15s;
}

.xblocks__add:hover {
  color: var(--text);
  border-color: var(--accent);
  background: rgba(122, 72, 36, 0.06);
}
</style>
