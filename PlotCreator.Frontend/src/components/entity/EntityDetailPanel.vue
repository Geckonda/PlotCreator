<script setup lang="ts">
import { computed, reactive, ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import type { Entity, EntityStatus, Relation } from '@/types/entity'
import { entityKey, relationFromKey, relationToKey } from '@/types/entity'
import type {
  ContentSection,
  EntityDto,
  EntityUpdatePayload,
  PropertyDef,
  TipTapDoc,
} from '@/types/api'
import { emptyTipTapDoc } from '@/types/api'
import { useEntityTypesStore } from '@/stores/entityTypes'
import { STATUS_LIST } from '@/config/statuses'
import { useEntitiesStore } from '@/stores/entities'
import { useWorldsStore } from '@/stores/worlds'
import TypeBadge from '@/components/ui/TypeBadge.vue'
import DynamicField from '@/components/entity/DynamicField.vue'
import RelationCreator from '@/components/entity/RelationCreator.vue'
import BlockEditor from '@/components/blocks/BlockEditor.vue'
import ExtraContentBlocks from '@/components/blocks/ExtraContentBlocks.vue'
import TagsInput from '@/components/forms/TagsInput.vue'
import ConfirmDialog from '@/components/ui/ConfirmDialog.vue'
import EntityMentionPopover from '@/components/entity/EntityMentionPopover.vue'
import type { EntityRef } from '@/composables/tiptapEntityHighlight'

const props = defineProps<{
  entity: Entity
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'delete', entity: Entity): void
}>()

const entitiesStore = useEntitiesStore()
const worldsStore = useWorldsStore()
const types = useEntityTypesStore()
const router = useRouter()

const cfg = computed(() => types.display(props.entity.typeKey))
const schema = computed<PropertyDef[]>(() => types.schemaFor(props.entity.typeKey))

function openFullView() {
  if (worldsStore.currentId === null) return
  router.push({
    name: 'entity-detail',
    params: {
      id: worldsStore.currentId,
      entityId: props.entity.id,
    },
  })
}

const loading = ref(false)
const saving = ref(false)
const error = ref<string | null>(null)
const showCreator = ref(false)

interface FormState {
  name: string
  description: string
  status: EntityStatus
  tagsText: string[]
  aliases: string[]
  properties: Record<string, unknown>
  content: TipTapDoc
  extraContents: ContentSection[]
}

const form = reactive<FormState>({
  name: '',
  description: '',
  status: 'draft',
  tagsText: [],
  aliases: [],
  properties: {},
  content: emptyTipTapDoc(),
  extraContents: [],
})

const original = ref<string>('')

function snapshot(): string {
  return JSON.stringify(form)
}

function applyDto(dto: EntityDto) {
  form.name = dto.name
  form.description = dto.desc ?? ''
  form.status = dto.status
  form.tagsText = dto.tags ?? []
  form.aliases = dto.aliases ?? []
  const props_: Record<string, unknown> = {}
  for (const def of schema.value) {
    props_[def.key] = dto.properties[def.key] ?? null
  }
  form.properties = props_
  form.content = dto.content ?? emptyTipTapDoc()
  form.extraContents = dto.extraContents ?? []
  original.value = snapshot()
}

const dirty = computed(() => snapshot() !== original.value)

async function load() {
  loading.value = true
  error.value = null
  try {
    await types.ensureLoaded()
    const dto = await entitiesStore.fetchDetail(props.entity.id)
    applyDto(dto)
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось загрузить'
  } finally {
    loading.value = false
  }
}

watch(
  () => props.entity.id,
  () => {
    showCreator.value = false
    load()
  },
  { immediate: true },
)

function buildPayload(): EntityUpdatePayload {
  return {
    name: form.name.trim(),
    desc: form.description.trim() || null,
    status: form.status,
    tags: form.tagsText,
    aliases: form.aliases,
    properties: { ...form.properties },
    content: form.content,
    extraContents: form.extraContents,
  }
}

async function save() {
  if (!dirty.value || saving.value) return
  saving.value = true
  error.value = null
  try {
    await entitiesStore.update(props.entity.id, buildPayload())
    original.value = snapshot()
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось сохранить'
  } finally {
    saving.value = false
  }
}

const selfId = computed(() => props.entity.id)

const connRels = computed(() => entitiesStore.relationsFor(props.entity.id))

function getOther(rel: Relation) {
  const otherKey =
    relationFromKey(rel) === selfId.value
      ? relationToKey(rel)
      : relationFromKey(rel)
  return entitiesStore.byKey.get(otherKey) ?? null
}

// function getDirection(rel: Relation) {
//   return relationFromKey(rel) === selfId.value ? '→' : '←'
// }

type ConfirmKind =
  | { kind: 'entity' }
  | { kind: 'relation'; id: number; otherName: string; label: string | null }

const confirmState = ref<ConfirmKind | null>(null)
const confirmBusy = ref(false)

const confirmTitle = computed(() =>
  confirmState.value?.kind === 'relation' ? 'Удаление связи' : 'Удаление',
)

const confirmMessage = computed(() => {
  if (!confirmState.value) return ''
  if (confirmState.value.kind === 'entity') {
    return `Удалить «${props.entity.name}»?`
  }
  const lbl = confirmState.value.label
  return lbl
    ? `Удалить связь «${lbl}» с «${confirmState.value.otherName}»?`
    : `Удалить связь с «${confirmState.value.otherName}»?`
})

const confirmDetail = computed(() =>
  confirmState.value?.kind === 'entity'
    ? 'Все связи этой сущности также будут удалены.'
    : undefined,
)

function askDeleteEntity() {
  confirmState.value = { kind: 'entity' }
}

function askDeleteRelation(rel: Relation) {
  const other = getOther(rel)
  confirmState.value = {
    kind: 'relation',
    id: rel.id,
    otherName: other?.name ?? '—',
    label: rel.label,
  }
}

async function onConfirm() {
  if (!confirmState.value || confirmBusy.value) return
  const state = confirmState.value
  if (state.kind === 'entity') {
    confirmState.value = null
    emit('delete', props.entity)
    return
  }
  confirmBusy.value = true
  try {
    await entitiesStore.removeRelation(state.id)
    confirmState.value = null
  } catch (e) {
    console.error('Delete relation failed', e)
  } finally {
    confirmBusy.value = false
  }
}

function onCancel() {
  if (confirmBusy.value) return
  confirmState.value = null
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

const mentionEntities = computed<EntityRef[]>(() =>
  entitiesStore.entities.map((e) => ({
    id: e.id,
    name: e.name,
    aliases: e.aliases ?? [],
  })),
)

const relatedIds = computed<Set<number>>(() => {
  const s = new Set<number>()
  for (const r of connRels.value) {
    s.add(relationFromKey(r) === selfId.value ? relationToKey(r) : relationFromKey(r))
  }
  return s
})

const mention = ref<{ entity: Entity; x: number; y: number } | null>(null)
const mentionRelationTarget = ref<Entity | null>(null)

function onMentionClick(payload: { entityId: number; x: number; y: number }) {
  const e = entitiesStore.byKey.get(payload.entityId)
  if (!e) return
  mention.value = { entity: e, x: payload.x, y: payload.y }
}

function onMentionCreate() {
  if (!mention.value) return
  mentionRelationTarget.value = mention.value.entity
  mention.value = null
  showCreator.value = true
}

function onMentionOpen() {
  if (!mention.value) return
  if (worldsStore.currentId === null) return
  router.push({
    name: 'entity-detail',
    params: { id: worldsStore.currentId, entityId: mention.value.entity.id },
  })
  mention.value = null
}

function onMentionClose() {
  mention.value = null
}

function onCreatorClose() {
  showCreator.value = false
  mentionRelationTarget.value = null
}
</script>

<template>
  <aside class="panel" :style="panelStyle">
    <header class="panel__head" :style="headerStyle">
      <div class="panel__head-row">
        <TypeBadge :type-key="entity.typeKey" />
        <div class="panel__head-actions">
          <button
            class="panel__expand"
            title="Открыть подробнее"
            @click="openFullView"
          >
            ↗ Подробнее
          </button>
          <button class="panel__close" @click="emit('close')">×</button>
        </div>
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
          <TagsInput
            v-model="form.tagsText"
            placeholder="Введите и нажмите Enter..."
          />
        </div>

        <div class="field">
          <label>Псевдонимы</label>
          <TagsInput
            v-model="form.aliases"
            placeholder="Альт. имена для подсветки..."
          />
        </div>
      </section>

      <section v-if="schema.length" class="panel__section">
        <div class="panel__section-title">Свойства</div>
        <DynamicField
          v-for="def in schema"
          :key="def.key"
          :def="def"
          :model-value="form.properties[def.key]"
          @update:model-value="form.properties[def.key] = $event"
        />
      </section>

      <section class="panel__section">
        <div class="panel__section-title">Содержание</div>
        <BlockEditor
          v-model="form.content"
          :entities="mentionEntities"
          :related-ids="relatedIds"
          :exclude-id="entity.id"
          @mention-click="onMentionClick"
        />

        <ExtraContentBlocks
          v-model="form.extraContents"
          :entities="mentionEntities"
          :related-ids="relatedIds"
          :exclude-id="entity.id"
          @mention-click="onMentionClick"
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
          :initial-target="mentionRelationTarget"
          @created="onCreatorClose"
          @cancel="onCreatorClose"
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
                :style="{ color: types.display(getOther(rel)!.typeKey).color }"
              >
                {{ types.display(getOther(rel)!.typeKey).icon }}
              </span>
              <div class="panel__conn-text">
                <div class="panel__conn-name">{{ getOther(rel)!.name }}</div>
                <div class="panel__conn-rel">
                  {{ rel.label ? '→ ' + rel.label : 'без подписи' }}
                </div>
              </div>
              <button
                class="panel__conn-del"
                title="Удалить связь"
                @click="askDeleteRelation(rel)"
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
      <button class="panel__delete" @click="askDeleteEntity">✕</button>
    </footer>

    <EntityMentionPopover
      v-if="mention"
      :entity="mention.entity"
      :x="mention.x"
      :y="mention.y"
      :already-related="relatedIds.has(mention.entity.id)"
      @create-relation="onMentionCreate"
      @open-entity="onMentionOpen"
      @close="onMentionClose"
    />

    <ConfirmDialog
      v-if="confirmState"
      tone="danger"
      :title="confirmTitle"
      :message="confirmMessage"
      :detail="confirmDetail"
      confirm-label="Удалить"
      :busy="confirmBusy"
      @confirm="onConfirm"
      @cancel="onCancel"
    />
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

.panel__head-actions {
  display: flex;
  align-items: center;
  gap: 8px;
}

.panel__expand {
  background: transparent;
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 4px 10px;
  font-family: var(--font-display);
  font-size: 11px;
  letter-spacing: 0.04em;
  color: var(--muted);
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.panel__expand:hover {
  color: var(--text);
  border-color: var(--border2);
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
