<script setup lang="ts">
import { onBeforeUnmount, watch } from 'vue'
import { useEditor, EditorContent } from '@tiptap/vue-3'
import type { JSONContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Image from '@tiptap/extension-image'
import Placeholder from '@tiptap/extension-placeholder'
import type { TipTapDoc } from '@/types/api'
import { emptyTipTapDoc } from '@/types/api'

const props = withDefaults(
  defineProps<{
    modelValue: TipTapDoc | null | undefined
    placeholder?: string
    editable?: boolean
  }>(),
  {
    placeholder: 'Начните печатать или используйте кнопки выше…',
    editable: true,
  },
)

const emit = defineEmits<{
  (e: 'update:modelValue', value: TipTapDoc): void
}>()

function normalize(doc: TipTapDoc | null | undefined): TipTapDoc {
  if (!doc || doc.type !== 'doc') return emptyTipTapDoc()
  return doc
}

const editor = useEditor({
  content: normalize(props.modelValue) as unknown as JSONContent,
  editable: props.editable,
  extensions: [
    StarterKit.configure({
      heading: { levels: [1, 2, 3] },
    }),
    Image.configure({ inline: false, allowBase64: false }),
    Placeholder.configure({ placeholder: props.placeholder }),
  ],
  onUpdate({ editor }) {
    emit('update:modelValue', editor.getJSON() as TipTapDoc)
  },
})

// Sync external value changes (e.g. when loading a different entity).
watch(
  () => props.modelValue,
  (val) => {
    const ed = editor.value
    if (!ed) return
    const next = normalize(val)
    const current = ed.getJSON()
    if (JSON.stringify(current) === JSON.stringify(next)) return
    ed.commands.setContent(next as unknown as JSONContent, { emitUpdate: false })
  },
)

watch(
  () => props.editable,
  (val) => {
    editor.value?.setEditable(val)
  },
)

onBeforeUnmount(() => {
  editor.value?.destroy()
})

function toggleBold() {
  editor.value?.chain().focus().toggleBold().run()
}
function toggleItalic() {
  editor.value?.chain().focus().toggleItalic().run()
}
function toggleStrike() {
  editor.value?.chain().focus().toggleStrike().run()
}
function toggleCode() {
  editor.value?.chain().focus().toggleCode().run()
}
function setHeading(level: 1 | 2 | 3) {
  editor.value?.chain().focus().toggleHeading({ level }).run()
}
function setParagraph() {
  editor.value?.chain().focus().setParagraph().run()
}
function toggleBulletList() {
  editor.value?.chain().focus().toggleBulletList().run()
}
function toggleOrderedList() {
  editor.value?.chain().focus().toggleOrderedList().run()
}
function toggleBlockquote() {
  editor.value?.chain().focus().toggleBlockquote().run()
}
function insertHr() {
  editor.value?.chain().focus().setHorizontalRule().run()
}
function insertImage() {
  const url = window.prompt('URL изображения:')
  if (!url) return
  editor.value?.chain().focus().setImage({ src: url }).run()
}

function isActive(name: string, attrs?: Record<string, unknown>): boolean {
  return editor.value ? editor.value.isActive(name, attrs) : false
}
</script>

<template>
  <div class="block-editor" :class="{ 'block-editor--readonly': !editable }">
    <div v-if="editable" class="block-editor__toolbar">
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('paragraph') }"
        title="Параграф"
        @click="setParagraph"
      >
        ¶
      </button>
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('heading', { level: 1 }) }"
        title="Заголовок 1"
        @click="setHeading(1)"
      >
        H1
      </button>
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('heading', { level: 2 }) }"
        title="Заголовок 2"
        @click="setHeading(2)"
      >
        H2
      </button>
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('heading', { level: 3 }) }"
        title="Заголовок 3"
        @click="setHeading(3)"
      >
        H3
      </button>

      <span class="tb-sep" />

      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('bold') }"
        title="Жирный"
        @click="toggleBold"
      >
        <b>B</b>
      </button>
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('italic') }"
        title="Курсив"
        @click="toggleItalic"
      >
        <i>I</i>
      </button>
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('strike') }"
        title="Зачёркнутый"
        @click="toggleStrike"
      >
        <s>S</s>
      </button>
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('code') }"
        title="Код"
        @click="toggleCode"
      >
        &lt;/&gt;
      </button>

      <span class="tb-sep" />

      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('bulletList') }"
        title="Список"
        @click="toggleBulletList"
      >
        •
      </button>
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('orderedList') }"
        title="Нумерованный список"
        @click="toggleOrderedList"
      >
        1.
      </button>
      <button
        type="button"
        class="tb-btn"
        :class="{ 'tb-btn--active': isActive('blockquote') }"
        title="Цитата"
        @click="toggleBlockquote"
      >
        ❝
      </button>

      <span class="tb-sep" />

      <button type="button" class="tb-btn" title="Изображение" @click="insertImage">
        🖼
      </button>
      <button type="button" class="tb-btn" title="Разделитель" @click="insertHr">
        —
      </button>
    </div>

    <EditorContent class="block-editor__content" :editor="editor" />
  </div>
</template>

<style scoped>
.block-editor {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.block-editor__toolbar {
  display: flex;
  flex-wrap: wrap;
  gap: 4px;
  align-items: center;
  padding: 6px 8px;
  background: rgba(101, 67, 33, 0.04);
  border: 1px solid var(--border);
  border-radius: 8px;
  position: sticky;
  top: 0;
  z-index: 2;
}

.tb-btn {
  background: transparent;
  border: 1px solid transparent;
  border-radius: 5px;
  padding: 4px 9px;
  font-size: 13px;
  font-family: var(--font-display);
  color: var(--muted);
  cursor: pointer;
  transition: background 0.12s, color 0.12s, border-color 0.12s;
  min-width: 28px;
  line-height: 1.2;
}

.tb-btn:hover {
  background: rgba(101, 67, 33, 0.08);
  color: var(--text);
}

.tb-btn--active {
  background: rgba(122, 72, 36, 0.18);
  border-color: rgba(122, 72, 36, 0.32);
  color: #5d3a1a;
}

.tb-sep {
  width: 1px;
  align-self: stretch;
  margin: 0 4px;
  background: var(--border);
}

.block-editor__content {
  border: 1px solid var(--border);
  border-radius: 8px;
  padding: 12px 14px;
  background: rgba(255, 255, 255, 0.55);
  min-height: 140px;
}

.block-editor--readonly .block-editor__content {
  background: transparent;
  border-color: transparent;
  padding-left: 0;
  padding-right: 0;
}

.block-editor__content :deep(.ProseMirror) {
  outline: none;
  font-family: var(--font-serif);
  font-size: 15px;
  line-height: 1.6;
  color: var(--text);
  min-height: 120px;
}

.block-editor__content :deep(.ProseMirror p.is-editor-empty:first-child::before) {
  content: attr(data-placeholder);
  float: left;
  color: var(--dim);
  pointer-events: none;
  height: 0;
  font-style: italic;
}

.block-editor__content :deep(.ProseMirror h1) {
  font-family: var(--font-display);
  font-size: 26px;
  font-weight: 700;
  margin: 18px 0 8px;
  letter-spacing: 0.01em;
}

.block-editor__content :deep(.ProseMirror h2) {
  font-family: var(--font-display);
  font-size: 21px;
  font-weight: 600;
  margin: 16px 0 6px;
}

.block-editor__content :deep(.ProseMirror h3) {
  font-family: var(--font-display);
  font-size: 17px;
  font-weight: 600;
  margin: 14px 0 4px;
}

.block-editor__content :deep(.ProseMirror p) {
  margin: 8px 0;
}

.block-editor__content :deep(.ProseMirror ul),
.block-editor__content :deep(.ProseMirror ol) {
  padding-left: 22px;
  margin: 8px 0;
}

.block-editor__content :deep(.ProseMirror blockquote) {
  border-left: 3px solid rgba(122, 72, 36, 0.45);
  padding: 4px 12px;
  margin: 10px 0;
  font-style: italic;
  color: var(--muted);
  background: rgba(101, 67, 33, 0.04);
  border-radius: 0 6px 6px 0;
}

.block-editor__content :deep(.ProseMirror hr) {
  border: none;
  border-top: 1px solid var(--border);
  margin: 16px 0;
}

.block-editor__content :deep(.ProseMirror img) {
  max-width: 100%;
  height: auto;
  border-radius: 6px;
  border: 1px solid var(--border);
  display: block;
  margin: 10px 0;
}

.block-editor__content :deep(.ProseMirror code) {
  background: rgba(101, 67, 33, 0.08);
  padding: 1px 5px;
  border-radius: 3px;
  font-size: 0.92em;
  font-family: monospace;
}
</style>
