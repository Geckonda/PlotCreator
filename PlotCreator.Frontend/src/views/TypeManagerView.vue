<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import type {
  EntityTypeDto,
  EntityTypeCreateRequest,
  EntityTypeUpdateRequest,
  PropertyDef,
  FieldKind,
} from '@/types/api'
import { useEntityTypesStore } from '@/stores/entityTypes'
import ConfirmDialog from '@/components/ui/ConfirmDialog.vue'

const router = useRouter()
const types = useEntityTypesStore()

const selectedId = ref<number | null>(null)
const error = ref<string | null>(null)
const saving = ref(false)
const showCreate = ref(false)
const pendingDelete = ref<EntityTypeDto | null>(null)

interface FormState {
  label: string
  color: string
  icon: string
  radius: number
  propertySchema: PropertyDef[]
}

const form = reactive<FormState>({
  label: '',
  color: '#7a4824',
  icon: '◯',
  radius: 18,
  propertySchema: [],
})

const original = ref<string>('')

function snapshot() {
  return JSON.stringify(form)
}

const dirty = computed(() => snapshot() !== original.value)

const selected = computed<EntityTypeDto | null>(() =>
  selectedId.value !== null
    ? types.types.find((t) => t.id === selectedId.value) ?? null
    : null,
)

function applyDto(dto: EntityTypeDto) {
  form.label = dto.label
  form.color = dto.color ?? '#7a4824'
  form.icon = dto.icon ?? '◯'
  form.radius = dto.radius
  form.propertySchema = (dto.propertySchema ?? []).map((p) => ({ ...p }))
  original.value = snapshot()
}

watch(selected, (dto) => {
  if (dto) applyDto(dto)
})

onMounted(async () => {
  try {
    await types.ensureLoaded()
    if (types.types.length > 0 && selectedId.value === null) {
      selectedId.value = types.types[0].id
    }
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось загрузить типы'
  }
})

function selectType(id: number) {
  selectedId.value = id
}

async function save() {
  if (!selected.value || !dirty.value || saving.value) return
  saving.value = true
  error.value = null
  try {
    const body: EntityTypeUpdateRequest = {
      label: form.label.trim(),
      color: form.color,
      icon: form.icon,
      radius: form.radius,
      propertySchema: form.propertySchema,
    }
    await types.update(selected.value.id, body)
    original.value = snapshot()
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось сохранить'
  } finally {
    saving.value = false
  }
}

function reset() {
  if (selected.value) applyDto(selected.value)
}

function addProperty() {
  const baseKey = `field${form.propertySchema.length + 1}`
  let key = baseKey
  let i = 1
  while (form.propertySchema.some((p) => p.key === key)) {
    i++
    key = `${baseKey}_${i}`
  }
  form.propertySchema.push({
    key,
    label: 'Новое свойство',
    kind: 'text',
  })
}

function removeProperty(idx: number) {
  form.propertySchema.splice(idx, 1)
}

function moveProperty(idx: number, dir: -1 | 1) {
  const j = idx + dir
  if (j < 0 || j >= form.propertySchema.length) return
  const arr = form.propertySchema
  ;[arr[idx], arr[j]] = [arr[j], arr[idx]]
}

function updateProperty(idx: number, patch: Partial<PropertyDef>) {
  form.propertySchema[idx] = { ...form.propertySchema[idx], ...patch }
}

function setOptions(idx: number, raw: string) {
  const opts = raw
    .split(',')
    .map((s) => s.trim())
    .filter((s) => s.length > 0)
  updateProperty(idx, { options: opts.length ? opts : undefined })
}

const KIND_LABELS: Record<FieldKind, string> = {
  text: 'Текст',
  textarea: 'Текст (большой)',
  number: 'Число',
  date: 'Дата',
  checkbox: 'Флажок',
  select: 'Выбор из списка',
  image: 'Изображение (URL)',
}

const KIND_LIST: FieldKind[] = [
  'text',
  'textarea',
  'number',
  'date',
  'checkbox',
  'select',
  'image',
]

// ── Create new type ──────────────────────────────────────────────────────
const createForm = reactive<EntityTypeCreateRequest>({
  key: '',
  label: '',
  color: '#7a4824',
  icon: '◯',
  radius: 18,
  propertySchema: [],
})
const createBusy = ref(false)
const createError = ref<string | null>(null)

function openCreate() {
  createForm.key = ''
  createForm.label = ''
  createForm.color = '#7a4824'
  createForm.icon = '◯'
  createForm.radius = 18
  createForm.propertySchema = []
  createError.value = null
  showCreate.value = true
}

async function submitCreate() {
  if (createBusy.value) return
  if (!createForm.key.trim() || !createForm.label.trim()) {
    createError.value = 'Заполните ключ и название'
    return
  }
  createBusy.value = true
  createError.value = null
  try {
    const dto = await types.create({
      key: createForm.key.trim().toLowerCase(),
      label: createForm.label.trim(),
      color: createForm.color,
      icon: createForm.icon,
      radius: createForm.radius,
      propertySchema: [],
    })
    showCreate.value = false
    selectedId.value = dto.id
  } catch (e: unknown) {
    createError.value = e instanceof Error ? e.message : 'Не удалось создать'
  } finally {
    createBusy.value = false
  }
}

// ── Delete ───────────────────────────────────────────────────────────────
function askDelete(t: EntityTypeDto) {
  pendingDelete.value = t
}

async function confirmDelete() {
  if (!pendingDelete.value) return
  const target = pendingDelete.value
  try {
    await types.remove(target.id)
    if (selectedId.value === target.id) {
      selectedId.value = types.types[0]?.id ?? null
    }
    pendingDelete.value = null
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось удалить'
    pendingDelete.value = null
  }
}

function cancelDelete() {
  pendingDelete.value = null
}

function goBack() {
  router.push({ name: 'home' })
}
</script>

<template>
  <div class="tm">
    <header class="tm__bar">
      <button class="tm__back" @click="goBack">← Главная</button>
      <h1 class="tm__title">Типы сущностей</h1>
      <div class="tm__bar-spacer" />
    </header>

    <div class="tm__body">
      <aside class="tm__list">
        <div class="tm__list-head">
          Ваши типы
          <span class="tm__list-count">{{ types.types.length }}</span>
        </div>

        <div v-if="types.loading" class="tm__list-state">Загрузка…</div>

        <ul v-else class="tm__list-items">
          <li
            v-for="t in types.types"
            :key="t.id"
            class="tm__list-item"
            :class="{ 'tm__list-item--active': selectedId === t.id }"
            :style="
              selectedId === t.id
                ? { borderColor: `${t.color ?? '#7a4824'}55`, background: `${t.color ?? '#7a4824'}10` }
                : {}
            "
            @click="selectType(t.id)"
          >
            <span class="tm__list-icon" :style="{ color: t.color ?? '#7a4824' }">
              {{ t.icon }}
            </span>
            <span class="tm__list-label">
              {{ t.label }}
              <span v-if="t.isSystemDefault" class="tm__list-badge">def</span>
            </span>
            <button
              class="tm__list-del"
              title="Удалить"
              @click.stop="askDelete(t)"
            >
              ×
            </button>
          </li>
        </ul>

        <button class="tm__list-create" @click="openCreate">
          + Новый тип
        </button>
      </aside>

      <main class="tm__editor">
        <div v-if="!selected" class="tm__editor-empty">
          Выберите тип слева или создайте новый.
        </div>

        <template v-else>
          <div class="tm__editor-head">
            <h2 class="tm__editor-title">
              <span :style="{ color: form.color }">{{ form.icon }}</span>
              {{ form.label || '—' }}
            </h2>
            <div class="tm__editor-actions">
              <button class="tm__btn" :disabled="!dirty" @click="reset">
                Сбросить
              </button>
              <button
                class="tm__btn tm__btn--primary"
                :style="{
                  background: `${form.color}24`,
                  color: form.color,
                  borderColor: `${form.color}55`,
                }"
                :disabled="!dirty || saving"
                @click="save"
              >
                {{ saving ? 'Сохранение…' : 'Сохранить' }}
              </button>
            </div>
          </div>

          <div v-if="error" class="tm__err">{{ error }}</div>

          <section class="tm__section">
            <div class="tm__section-title">Внешний вид</div>
            <div class="tm__row">
              <div class="tm__field">
                <label>Ключ</label>
                <input
                  type="text"
                  :value="selected.key"
                  disabled
                  class="tm__input tm__input--mono"
                />
                <div class="tm__hint">Изменить нельзя после создания.</div>
              </div>
              <div class="tm__field tm__field--grow">
                <label>Название</label>
                <input
                  v-model="form.label"
                  type="text"
                  class="tm__input"
                />
              </div>
            </div>
            <div class="tm__row">
              <div class="tm__field">
                <label>Цвет</label>
                <input
                  v-model="form.color"
                  type="color"
                  class="tm__input tm__input--color"
                />
              </div>
              <div class="tm__field">
                <label>Иконка</label>
                <input
                  v-model="form.icon"
                  type="text"
                  maxlength="4"
                  class="tm__input tm__input--icon"
                />
              </div>
              <div class="tm__field">
                <label>Радиус (граф)</label>
                <input
                  v-model.number="form.radius"
                  type="number"
                  min="8"
                  max="40"
                  class="tm__input tm__input--num"
                />
              </div>
            </div>
          </section>

          <section class="tm__section">
            <div class="tm__section-title">
              Свойства
              <button class="tm__add-btn" @click="addProperty">
                + Свойство
              </button>
            </div>

            <div v-if="!form.propertySchema.length" class="tm__props-empty">
              Пока нет свойств. Нажмите «+ Свойство», чтобы добавить.
            </div>

            <div v-else class="tm__props">
              <div
                v-for="(p, idx) in form.propertySchema"
                :key="idx"
                class="tm__prop"
              >
                <div class="tm__prop-row">
                  <div class="tm__prop-order">
                    <button
                      class="tm__order-btn"
                      :disabled="idx === 0"
                      title="Вверх"
                      @click="moveProperty(idx, -1)"
                    >
                      ↑
                    </button>
                    <button
                      class="tm__order-btn"
                      :disabled="idx === form.propertySchema.length - 1"
                      title="Вниз"
                      @click="moveProperty(idx, 1)"
                    >
                      ↓
                    </button>
                  </div>

                  <div class="tm__field">
                    <label>Ключ</label>
                    <input
                      :value="p.key"
                      type="text"
                      class="tm__input tm__input--mono"
                      @input="
                        updateProperty(idx, {
                          key: ($event.target as HTMLInputElement).value,
                        })
                      "
                    />
                  </div>

                  <div class="tm__field tm__field--grow">
                    <label>Название</label>
                    <input
                      :value="p.label"
                      type="text"
                      class="tm__input"
                      @input="
                        updateProperty(idx, {
                          label: ($event.target as HTMLInputElement).value,
                        })
                      "
                    />
                  </div>

                  <div class="tm__field">
                    <label>Тип</label>
                    <select
                      :value="p.kind"
                      class="tm__input"
                      @change="
                        updateProperty(idx, {
                          kind: ($event.target as HTMLSelectElement).value as FieldKind,
                        })
                      "
                    >
                      <option v-for="k in KIND_LIST" :key="k" :value="k">
                        {{ KIND_LABELS[k] }}
                      </option>
                    </select>
                  </div>

                  <button
                    class="tm__prop-del"
                    title="Удалить"
                    @click="removeProperty(idx)"
                  >
                    ×
                  </button>
                </div>

                <div v-if="p.kind === 'select'" class="tm__prop-options">
                  <label>Варианты (через запятую)</label>
                  <input
                    :value="(p.options ?? []).join(', ')"
                    type="text"
                    class="tm__input"
                    placeholder="один, два, три"
                    @input="
                      setOptions(idx, ($event.target as HTMLInputElement).value)
                    "
                  />
                </div>
              </div>
            </div>
          </section>
        </template>
      </main>
    </div>

    <!-- Create modal -->
    <div v-if="showCreate" class="tm__backdrop" @click.self="showCreate = false">
      <div class="tm__dialog">
        <header class="tm__dialog-head">
          <div class="tm__dialog-title">Новый тип</div>
          <button class="tm__dialog-close" @click="showCreate = false">
            ×
          </button>
        </header>
        <div class="tm__dialog-body">
          <div class="tm__field">
            <label>Ключ (slug)</label>
            <input
              v-model="createForm.key"
              type="text"
              class="tm__input tm__input--mono"
              placeholder="например: spell"
              @keydown.enter="submitCreate"
            />
            <div class="tm__hint">
              Только латиница, цифры, дефисы. После создания не меняется.
            </div>
          </div>
          <div class="tm__field">
            <label>Название</label>
            <input
              v-model="createForm.label"
              type="text"
              class="tm__input"
              placeholder="например: Заклинания"
              @keydown.enter="submitCreate"
            />
          </div>
          <div class="tm__row">
            <div class="tm__field">
              <label>Цвет</label>
              <input
                v-model="createForm.color"
                type="color"
                class="tm__input tm__input--color"
              />
            </div>
            <div class="tm__field">
              <label>Иконка</label>
              <input
                v-model="createForm.icon"
                type="text"
                maxlength="4"
                class="tm__input tm__input--icon"
              />
            </div>
            <div class="tm__field">
              <label>Радиус</label>
              <input
                v-model.number="createForm.radius"
                type="number"
                min="8"
                max="40"
                class="tm__input tm__input--num"
              />
            </div>
          </div>
          <div v-if="createError" class="tm__err">{{ createError }}</div>
        </div>
        <footer class="tm__dialog-foot">
          <button class="tm__btn" @click="showCreate = false">Отмена</button>
          <button
            class="tm__btn tm__btn--primary"
            :disabled="createBusy"
            @click="submitCreate"
          >
            {{ createBusy ? 'Создание…' : 'Создать' }}
          </button>
        </footer>
      </div>
    </div>

    <ConfirmDialog
      v-if="pendingDelete"
      tone="danger"
      title="Удаление типа"
      :message="`Удалить тип «${pendingDelete.label}»?`"
      detail="Если у типа есть сущности, удаление будет отклонено сервером."
      confirm-label="Удалить"
      @confirm="confirmDelete"
      @cancel="cancelDelete"
    />
  </div>
</template>

<style scoped>
.tm {
  position: fixed;
  inset: 0;
  display: flex;
  flex-direction: column;
  background: var(--bg);
  overflow: hidden;
}

.tm__bar {
  display: flex;
  align-items: center;
  gap: 14px;
  padding: 12px 28px;
  border-bottom: 1px solid var(--border);
  flex-shrink: 0;
}

.tm__back {
  background: transparent;
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 6px 14px;
  font-family: var(--font-display);
  font-size: 13px;
  color: var(--muted);
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.tm__back:hover {
  color: var(--text);
  border-color: var(--border2);
}

.tm__title {
  font-family: var(--font-display);
  font-size: 18px;
  font-weight: 600;
  margin: 0;
}

.tm__bar-spacer {
  flex: 1;
}

.tm__body {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.tm__list {
  width: 280px;
  flex-shrink: 0;
  border-right: 1px solid var(--border);
  display: flex;
  flex-direction: column;
  background: var(--surface);
}

.tm__list-head {
  padding: 16px 20px 10px;
  font-family: var(--font-display);
  font-size: 12px;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--muted);
  display: flex;
  justify-content: space-between;
}

.tm__list-count {
  color: var(--dim);
}

.tm__list-state {
  padding: 18px 20px;
  color: var(--muted);
  font-style: italic;
  font-size: 13px;
}

.tm__list-items {
  list-style: none;
  margin: 0;
  padding: 0 8px;
  flex: 1;
  overflow-y: auto;
}

.tm__list-item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 9px 12px;
  border-radius: 8px;
  border: 1px solid transparent;
  margin-bottom: 4px;
  cursor: pointer;
  font-size: 14px;
  font-family: var(--font-display);
  letter-spacing: 0.02em;
  color: var(--text);
  transition: background 0.15s, border-color 0.15s;
}

.tm__list-item:hover:not(.tm__list-item--active) {
  background: rgba(101, 67, 33, 0.06);
}

.tm__list-icon {
  font-size: 15px;
  width: 18px;
  text-align: center;
}

.tm__list-label {
  flex: 1;
  display: flex;
  align-items: center;
  gap: 8px;
}

.tm__list-badge {
  font-size: 10px;
  padding: 1px 6px;
  border-radius: 99px;
  background: rgba(101, 67, 33, 0.1);
  color: var(--dim);
  font-family: monospace;
  letter-spacing: 0;
}

.tm__list-del {
  color: var(--dim);
  background: transparent;
  border: none;
  font-size: 16px;
  line-height: 1;
  padding: 0 6px;
  cursor: pointer;
  opacity: 0;
  transition: color 0.15s, opacity 0.15s;
}

.tm__list-item:hover .tm__list-del {
  opacity: 1;
}

.tm__list-del:hover {
  color: #a02929;
}

.tm__list-create {
  margin: 12px 16px;
  padding: 9px 14px;
  border: 1px dashed var(--border2);
  border-radius: 8px;
  background: transparent;
  font-family: var(--font-display);
  font-size: 13px;
  color: var(--muted);
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.tm__list-create:hover {
  color: var(--accent);
  border-color: var(--accent);
}

.tm__editor {
  flex: 1;
  overflow-y: auto;
  padding: 24px 32px;
  display: flex;
  flex-direction: column;
  gap: 20px;
  max-width: 1080px;
}

.tm__editor-empty {
  margin: auto;
  color: var(--muted);
  font-style: italic;
  font-size: 14px;
}

.tm__editor-head {
  display: flex;
  align-items: center;
  gap: 12px;
}

.tm__editor-title {
  margin: 0;
  font-family: var(--font-display);
  font-size: 22px;
  font-weight: 600;
  flex: 1;
  display: flex;
  align-items: center;
  gap: 10px;
}

.tm__editor-actions {
  display: flex;
  gap: 8px;
}

.tm__btn {
  padding: 7px 16px;
  border-radius: 7px;
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
  cursor: pointer;
  transition: filter 0.15s, opacity 0.15s, color 0.15s, border-color 0.15s;
}

.tm__btn:hover:not(:disabled) {
  color: var(--text);
  border-color: var(--border2);
}

.tm__btn--primary {
  font-weight: 600;
}

.tm__btn:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.tm__section {
  display: flex;
  flex-direction: column;
  gap: 12px;
  padding: 18px 20px;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 12px;
}

.tm__section-title {
  font-family: var(--font-display);
  font-size: 13px;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--muted);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.tm__add-btn {
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  color: var(--muted);
  background: transparent;
  border: 1px solid var(--border);
  border-radius: 5px;
  padding: 4px 10px;
  text-transform: none;
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.tm__add-btn:hover {
  color: var(--text);
  border-color: var(--border2);
}

.tm__row {
  display: flex;
  flex-wrap: wrap;
  gap: 14px;
  align-items: flex-end;
}

.tm__field {
  display: flex;
  flex-direction: column;
  gap: 4px;
  min-width: 0;
}

.tm__field--grow {
  flex: 1;
}

.tm__field > label {
  font-size: 11px;
  color: var(--muted);
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.tm__input {
  font-family: var(--font-serif);
  font-size: 14px;
  background: rgba(255, 255, 255, 0.6);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 7px 9px;
  width: 100%;
  min-width: 0;
}

.tm__input--mono {
  font-family: monospace;
  font-size: 13px;
}

.tm__input--icon {
  width: 60px;
  text-align: center;
}

.tm__input--num {
  width: 80px;
}

.tm__input--color {
  height: 36px;
  padding: 2px;
  width: 60px;
  cursor: pointer;
}

.tm__hint {
  font-size: 11px;
  color: var(--dim);
  font-style: italic;
}

.tm__props {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.tm__prop {
  background: rgba(101, 67, 33, 0.04);
  border: 1px solid var(--border);
  border-radius: 8px;
  padding: 10px 12px;
}

.tm__prop-row {
  display: flex;
  gap: 10px;
  align-items: flex-end;
}

.tm__prop-order {
  display: flex;
  flex-direction: column;
  gap: 2px;
  flex-shrink: 0;
}

.tm__order-btn {
  font-size: 11px;
  width: 22px;
  height: 18px;
  padding: 0;
  background: transparent;
  border: 1px solid var(--border);
  border-radius: 3px;
  color: var(--muted);
  cursor: pointer;
  transition: color 0.12s, border-color 0.12s;
}

.tm__order-btn:hover:not(:disabled) {
  color: var(--text);
  border-color: var(--border2);
}

.tm__order-btn:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.tm__prop-del {
  background: transparent;
  border: none;
  color: var(--dim);
  font-size: 18px;
  line-height: 1;
  padding: 0 8px;
  cursor: pointer;
  align-self: center;
  transition: color 0.15s;
}

.tm__prop-del:hover {
  color: #a02929;
}

.tm__prop-options {
  display: flex;
  flex-direction: column;
  gap: 4px;
  margin-top: 8px;
}

.tm__prop-options > label {
  font-size: 11px;
  color: var(--muted);
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.tm__props-empty {
  color: var(--muted);
  font-style: italic;
  font-size: 13px;
  padding: 6px 4px;
}

.tm__err {
  padding: 8px 12px;
  font-size: 12px;
  color: #a02929;
  background: rgba(160, 41, 41, 0.06);
  border: 1px solid rgba(160, 41, 41, 0.25);
  border-radius: 6px;
}

.tm__backdrop {
  position: fixed;
  inset: 0;
  background: rgba(58, 36, 20, 0.55);
  z-index: 200;
  display: flex;
  align-items: center;
  justify-content: center;
}

.tm__dialog {
  width: 480px;
  max-width: calc(100vw - 32px);
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 14px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

.tm__dialog-head {
  padding: 16px 22px;
  border-bottom: 1px solid var(--border);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.tm__dialog-title {
  font-family: var(--font-display);
  font-size: 17px;
  font-weight: 600;
}

.tm__dialog-close {
  background: transparent;
  border: none;
  color: var(--muted);
  font-size: 22px;
  line-height: 1;
  cursor: pointer;
  padding: 0 4px;
}

.tm__dialog-close:hover {
  color: var(--text);
}

.tm__dialog-body {
  padding: 18px 22px;
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.tm__dialog-foot {
  padding: 14px 22px;
  border-top: 1px solid var(--border);
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}
</style>
