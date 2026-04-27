<script setup lang="ts">
import { computed, reactive, ref, watch } from 'vue'
import type { Entity, EntityStatus, Relation } from '@/types/entity'
import { entityKey, relationFromKey, relationToKey } from '@/types/entity'
import type { FullEntityDto } from '@/types/api'
import { ENTITY_TYPES } from '@/config/entityTypes'
import { STATUS_LIST } from '@/config/statuses'
import { ENTITY_FIELD_SCHEMAS, type FieldDef } from '@/config/entityFields'
import { useEntitiesStore } from '@/stores/entities'
import { useWorldsStore } from '@/stores/worlds'
import TypeBadge from '@/components/ui/TypeBadge.vue'
import DynamicField from '@/components/entity/DynamicField.vue'
import RelationCreator from '@/components/entity/RelationCreator.vue'

const props = defineProps<{
  entity: Entity
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'delete', entity: Entity): void
}>()

const entitiesStore = useEntitiesStore()
const worldsStore = useWorldsStore()
const cfg = computed(() => ENTITY_TYPES[props.entity.type])

const loading = ref(false)
const saving = ref(false)
const error = ref<string | null>(null)
const showCreator = ref(false)

interface FormState {
  name: string
  description: string
  status: EntityStatus
  tagsText: string
  extras: Record<string, unknown>
}

const form = reactive<FormState>({
  name: '',
  description: '',
  status: 'draft',
  tagsText: '',
  extras: {},
})

const original = ref<string>('')

const schema = computed<FieldDef[]>(
  () => ENTITY_FIELD_SCHEMAS[props.entity.type] ?? [],
)

function snapshot(): string {
  return JSON.stringify(form)
}

function applyDto(dto: FullEntityDto) {
  form.name = dto.name
  form.description = dto.desc ?? ''
  form.status = dto.status
  form.tagsText = (dto.tags ?? []).join(', ')
  const extras: Record<string, unknown> = {}
  for (const def of schema.value) {
    extras[def.key] = (dto as unknown as Record<string, unknown>)[def.key] ?? null
  }
  form.extras = extras
  original.value = snapshot()
}

const dirty = computed(() => snapshot() !== original.value)

async function load() {
  loading.value = true
  error.value = null
  try {
    const dto = await entitiesStore.fetchDetail(props.entity.type, props.entity.id)
    applyDto(dto)
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось загрузить'
  } finally {
    loading.value = false
  }
}

watch(
  () => `${props.entity.type}:${props.entity.id}`,
  () => {
    showCreator.value = false
    load()
  },
  { immediate: true },
)

function buildPayload(): object {
  const tags = form.tagsText
    .split(',')
    .map((t) => t.trim())
    .filter(Boolean)
  return {
    name: form.name.trim(),
    desc: form.description.trim() || null,
    status: form.status,
    tags,
    ...form.extras,
  }
}

async function save() {
  if (!dirty.value || saving.value) return
  saving.value = true
  error.value = null
  try {
    await entitiesStore.update(props.entity.type, props.entity.id, buildPayload())
    original.value = snapshot()
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось сохранить'
  } finally {
    saving.value = false
  }
}

const selfKey = computed(() => entityKey(props.entity.type, props.entity.id))

const connRels = computed(() =>
  entitiesStore.relationsFor(props.entity.type, props.entity.id),
)

function getOther(rel: Relation) {
  const otherKey =
    relationFromKey(rel) === selfKey.value
      ? relationToKey(rel)
      : relationFromKey(rel)
  return entitiesStore.byKey.get(otherKey) ?? null
}

function getDirection(rel: Relation) {
  return relationFromKey(rel) === selfKey.value ? '→' : '←'
}

async function deleteRelation(id: number) {
  try {
    await entitiesStore.removeRelation(id)
  } catch (e) {
    console.error('Delete relation failed', e)
  }
}

const panelStyle = computed(() => ({
  borderLeft: `1px solid ${cfg.value.color}45`,
}))

const headerStyle = computed(() => ({
  background: `linear-gradient(135deg, ${cfg.value.color}14, transparent)`,
  borderBottom: `1px solid ${cfg.value.color}28`,
}))

const saveStyle = computed(() => ({
  background: `${cfg.value.color}1c`,
  color: cfg.value.color,
  border: `1px solid ${cfg.value.color}48`,
}))
</script>

<template>
  <aside class="panel" :style="panelStyle">
    <header class="panel__head" :style="headerStyle">
      <div class="panel__head-row">
        <TypeBadge :type="entity.type" />
        <button class="panel__close" @click="emit('close')">×</button>
      </div>
      <input
        v-model="form.name"
        class="panel__name-input"
        :style="{ color: cfg.color }"
        placeholder="Название"
      />
    </header>

    <div v-if="loading" class="panel__state">Загрузка…</div>

    <template v-else>
      <section class="panel__section">
        <div class="panel__section-title">Основное</div>

        <div class="field">
          <label>Описание</label>
          <textarea v-model="form.description" rows="3" />
        </div>

        <div class="field-row">
          <div class="field">
            <label>Статус</label>
            <select v-model="form.status">
              <option v-for="[k, v] in STATUS_LIST" :key="k" :value="k">
                {{ v.label }}
              </option>
            </select>
          </div>
        </div>

        <div class="field">
          <label>Теги</label>
          <input
            v-model="form.tagsText"
            type="text"
            placeholder="тег1, тег2…"
          />
        </div>
      </section>

      <section v-if="schema.length" class="panel__section">
        <div class="panel__section-title">Свойства</div>
        <DynamicField
          v-for="def in schema"
          :key="def.key"
          :def="def"
          :model-value="form.extras[def.key]"
          @update:model-value="form.extras[def.key] = $event"
        />
      </section>

      <section class="panel__section">
        <div class="panel__section-title">
          Связи · {{ connRels.length }}
          <button
            class="panel__section-add"
            @click="showCreator = !showCreator"
          >
            {{ showCreator ? '×' : '+' }} Связь
          </button>
        </div>

        <RelationCreator
          v-if="showCreator && worldsStore.currentId !== null"
          :world-id="worldsStore.currentId"
          :from="entity"
          @created="showCreator = false"
          @cancel="showCreator = false"
        />

        <ul v-if="connRels.length" class="panel__conn-list">
          <li
            v-for="rel in connRels"
            :key="rel.id"
            class="panel__conn"
          >
            <template v-if="getOther(rel)">
              <span
                class="panel__conn-icon"
                :style="{ color: ENTITY_TYPES[getOther(rel)!.type].color }"
              >
                {{ ENTITY_TYPES[getOther(rel)!.type].icon }}
              </span>
              <div class="panel__conn-text">
                <div class="panel__conn-name">{{ getOther(rel)!.name }}</div>
                <div class="panel__conn-rel">
                  {{ getDirection(rel) }} {{ rel.label }}
                </div>
              </div>
              <button
                class="panel__conn-del"
                title="Удалить связь"
                @click="deleteRelation(rel.id)"
              >
                ×
              </button>
            </template>
          </li>
        </ul>
      </section>
    </template>

    <div v-if="error" class="panel__err">{{ error }}</div>

    <footer class="panel__actions">
      <button
        class="panel__save"
        :style="saveStyle"
        :disabled="!dirty || saving"
        @click="save"
      >
        {{ saving ? 'Сохранение…' : dirty ? 'Сохранить' : 'Сохранено' }}
      </button>
      <button class="panel__delete" @click="emit('delete', entity)">✕</button>
    </footer>
  </aside>
</template>

<style scoped>
.panel {
  width: 360px;
  flex-shrink: 0;
  background: linear-gradient(160deg, var(--card2), var(--surface));
  display: flex;
  flex-direction: column;
  overflow-y: auto;
  animation: slideR 0.28s cubic-bezier(0.2, 0.8, 0.2, 1);
}

.panel__head {
  padding: 22px 22px 18px;
}

.panel__head-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 12px;
}

.panel__close {
  color: var(--muted);
  font-size: 24px;
  line-height: 1;
  padding: 0 4px;
  transition: color 0.15s;
}

.panel__close:hover {
  color: var(--text);
}

.panel__name-input {
  width: 100%;
  background: transparent;
  border: none;
  border-bottom: 1px dashed transparent;
  font-family: var(--font-display);
  font-size: 21px;
  font-weight: 600;
  line-height: 1.3;
  padding: 2px 0;
  transition: border-color 0.15s;
}

.panel__name-input:focus {
  outline: none;
  border-bottom-color: rgba(101, 67, 33, 0.35);
}

.panel__state {
  padding: 26px 22px;
  color: var(--muted);
  font-style: italic;
  font-size: 13px;
}

.panel__section {
  padding: 16px 22px;
  border-top: 1px solid var(--border);
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.panel__section-title {
  font-family: var(--font-display);
  font-size: 12px;
  letter-spacing: 0.1em;
  color: var(--muted);
  text-transform: uppercase;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.panel__section-add {
  font-size: 11px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  color: var(--muted);
  background: transparent;
  border: 1px solid var(--border);
  border-radius: 5px;
  padding: 3px 9px;
  text-transform: none;
  transition: all 0.15s;
}

.panel__section-add:hover {
  color: var(--text);
  border-color: var(--border2);
}

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
.field textarea,
.field select {
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

.field-row {
  display: grid;
  grid-template-columns: 1fr;
  gap: 10px;
}

.panel__conn-list {
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: 6px;
  margin: 0;
  padding: 0;
}

.panel__conn {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 8px 10px;
  border-radius: 7px;
  background: rgba(101, 67, 33, 0.04);
  border: 1px solid var(--border);
  transition: background 0.15s;
}

.panel__conn:hover {
  background: rgba(101, 67, 33, 0.08);
}

.panel__conn-icon {
  font-size: 14px;
  flex-shrink: 0;
}

.panel__conn-text {
  flex: 1;
  min-width: 0;
}

.panel__conn-name {
  font-size: 13px;
  font-family: var(--font-display);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.panel__conn-rel {
  font-size: 12px;
  color: var(--muted);
  margin-top: 2px;
}

.panel__conn-del {
  color: var(--dim);
  font-size: 16px;
  line-height: 1;
  padding: 0 6px;
  transition: color 0.15s;
}

.panel__conn-del:hover {
  color: #a02929;
}

.panel__err {
  margin: 0 22px;
  padding: 8px 10px;
  font-size: 12px;
  color: #a02929;
  background: rgba(160, 41, 41, 0.06);
  border: 1px solid rgba(160, 41, 41, 0.25);
  border-radius: 6px;
}

.panel__actions {
  margin-top: auto;
  padding: 14px 22px 18px;
  border-top: 1px solid var(--border);
  display: flex;
  gap: 10px;
}

.panel__save {
  flex: 1;
  padding: 9px 0;
  border-radius: 8px;
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  transition: filter 0.15s, opacity 0.15s;
}

.panel__save:hover:not(:disabled) {
  filter: brightness(1.1);
}

.panel__save:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.panel__delete {
  padding: 9px 18px;
  border-radius: 8px;
  background: transparent;
  color: var(--muted);
  border: 1px solid var(--border);
  font-size: 14px;
  font-family: var(--font-display);
  transition: all 0.15s;
}

.panel__delete:hover {
  color: #a02929;
  border-color: rgba(160, 41, 41, 0.4);
}
</style>
