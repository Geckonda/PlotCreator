<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, reactive, ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import type { Entity, EntityStatus, Relation } from '@/types/entity'
import { relationFromKey, relationToKey } from '@/types/entity'
import type { EntityDto, EntityUpdatePayload, PropertyDef } from '@/types/api'
import { useEntityTypesStore } from '@/stores/entityTypes'
import { STATUS_LIST, STATUSES } from '@/config/statuses'
import { useEntitiesStore } from '@/stores/entities'
import { useWorldsStore } from '@/stores/worlds'
import TypeBadge from '@/components/ui/TypeBadge.vue'
import DynamicField from '@/components/entity/DynamicField.vue'
import RelationCreator from '@/components/entity/RelationCreator.vue'
import EntityCard from '@/components/entity/EntityCard.vue'
import TagsInput from '@/components/forms/TagsInput.vue'
import ConfirmDialog from '@/components/ui/ConfirmDialog.vue'

const props = defineProps<{
  worldId: number
  entityId: number
}>()

const router = useRouter()
const entitiesStore = useEntitiesStore()
const worldsStore = useWorldsStore()
const types = useEntityTypesStore()

const loading = ref(false)
const saving = ref(false)
const error = ref<string | null>(null)
const showCreator = ref(false)
const notFound = ref(false)
const typeKey = ref<string>('')

interface FormState {
  name: string
  description: string
  status: EntityStatus
  tagsText: string[]
  properties: Record<string, unknown>
}

const form = reactive<FormState>({
  name: '',
  description: '',
  status: 'draft',
  tagsText: [],
  properties: {},
})

const original = ref('')

const entity = computed<Entity | null>(
  () => entitiesStore.byKey.get(props.entityId) ?? null,
)

const cfg = computed(() =>
  typeKey.value ? types.display(typeKey.value) : null,
)

const statusCfg = computed(() => STATUSES[form.status] ?? STATUSES.draft)

const schema = computed<PropertyDef[]>(() =>
  typeKey.value ? types.schemaFor(typeKey.value) : [],
)

const supportsPicture = computed(
  () =>
    typeKey.value === 'character' ||
    typeKey.value === 'location' ||
    typeKey.value === 'artifact',
)

const pictureUrl = computed(() => {
  const v = form.properties.pictureUrl
  return typeof v === 'string' && v.trim() ? v : null
})

function snapshot() {
  return JSON.stringify({ ...form, typeKey: typeKey.value })
}

function applyDto(dto: EntityDto) {
  typeKey.value = dto.typeKey
  form.name = dto.name
  form.description = dto.desc ?? ''
  form.status = dto.status
  form.tagsText = dto.tags ?? []
  const props_: Record<string, unknown> = {}
  for (const def of schema.value) {
    props_[def.key] = dto.properties[def.key] ?? null
  }
  form.properties = props_
  original.value = snapshot()
}

const dirty = computed(() => snapshot() !== original.value)

async function load() {
  loading.value = true
  error.value = null
  notFound.value = false
  try {
    worldsStore.setCurrent(props.worldId)
    await types.ensureLoaded()
    if (!entitiesStore.byKey.get(props.entityId)) {
      await entitiesStore.fetchForWorld(props.worldId)
    }
    if (!entitiesStore.byKey.get(props.entityId)) {
      notFound.value = true
      return
    }
    const dto = await entitiesStore.fetchDetail(props.entityId)
    applyDto(dto)
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось загрузить'
  } finally {
    loading.value = false
  }
}

watch(
  () => `${props.entityId}:${props.worldId}`,
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
    properties: { ...form.properties },
  }
}

async function save() {
  if (!dirty.value || saving.value) return
  saving.value = true
  error.value = null
  try {
    await entitiesStore.update(props.entityId, buildPayload())
    original.value = snapshot()
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Не удалось сохранить'
  } finally {
    saving.value = false
  }
}

const connRels = computed<Relation[]>(() =>
  entitiesStore.relationsFor(props.entityId),
)

function getOther(rel: Relation): Entity | null {
  const otherKey =
    relationFromKey(rel) === props.entityId
      ? relationToKey(rel)
      : relationFromKey(rel)
  return entitiesStore.byKey.get(otherKey) ?? null
}

function getDirection(rel: Relation) {
  return relationFromKey(rel) === props.entityId ? '→' : '←'
}

function goBack() {
  router.push({ name: 'world', params: { id: props.worldId } })
}

function goToEntity(other: Entity) {
  router.push({
    name: 'entity-detail',
    params: { id: props.worldId, entityId: other.id },
  })
}

type ConfirmKind =
  | { kind: 'entity' }
  | { kind: 'relation'; id: number; otherName: string; label: string }

const confirmState = ref<ConfirmKind | null>(null)
const confirmBusy = ref(false)

const confirmTitle = computed(() =>
  confirmState.value?.kind === 'relation' ? 'Удаление связи' : 'Удаление',
)

const confirmMessage = computed(() => {
  if (!confirmState.value) return ''
  if (confirmState.value.kind === 'entity') {
    return `Удалить «${form.name}»?`
  }
  return `Удалить связь «${confirmState.value.label}» с «${confirmState.value.otherName}»?`
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
    if (!entity.value) {
      confirmState.value = null
      return
    }
    confirmBusy.value = true
    try {
      await entitiesStore.remove(entity.value)
      confirmState.value = null
      goBack()
    } catch (e) {
      console.error('Delete entity failed', e)
    } finally {
      confirmBusy.value = false
    }
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

function onKey(ev: KeyboardEvent) {
  if (confirmState.value) return
  const target = ev.target as HTMLElement | null
  const tag = target?.tagName
  const editable = tag === 'INPUT' || tag === 'TEXTAREA' || tag === 'SELECT'

  if ((ev.ctrlKey || ev.metaKey) && ev.key.toLowerCase() === 's') {
    ev.preventDefault()
    save()
    return
  }

  if (ev.key === 'Escape' && !editable) {
    if (showCreator.value) {
      showCreator.value = false
      return
    }
    goBack()
  }
}

onMounted(() => {
  window.addEventListener('keydown', onKey)
})

onBeforeUnmount(() => {
  window.removeEventListener('keydown', onKey)
})

const barStyle = computed(() => {
  if (!cfg.value) return {}
  return {
    background: `linear-gradient(180deg, ${cfg.value.color}10, transparent)`,
    borderBottom: `1px solid ${cfg.value.color}28`,
  }
})

const saveStyle = computed(() => {
  if (!cfg.value) return {}
  return {
    background: `${cfg.value.color}1c`,
    color: cfg.value.color,
    border: `1px solid ${cfg.value.color}48`,
  }
})

const profileStyle = computed(() => {
  if (!cfg.value) return {}
  return {
    background: `linear-gradient(135deg, ${cfg.value.color}14, transparent 70%)`,
    border: `1px solid ${cfg.value.color}38`,
  }
})

const picFrameStyle = computed(() => {
  if (!cfg.value) return {}
  return {
    border: `2px solid ${cfg.value.color}55`,
    boxShadow: `0 6px 20px ${cfg.value.color}22`,
  }
})

const picPlaceholderStyle = computed(() => {
  if (!cfg.value) return {}
  return {
    background: `repeating-linear-gradient(48deg, ${cfg.value.color}14, ${cfg.value.color}14 2px, transparent 2px, transparent 14px)`,
  }
})

const statusPillStyle = computed(() => ({
  background: `${statusCfg.value.color}1c`,
  color: statusCfg.value.color,
  border: `1px solid ${statusCfg.value.color}55`,
}))

const sectionTitleStyle = computed(() => {
  if (!cfg.value) return {}
  return { color: cfg.value.color }
})

function onPictureUrlInput(ev: Event) {
  const value = (ev.target as HTMLInputElement).value
  form.properties.pictureUrl = value === '' ? null : value
}

const visibleSchema = computed(() =>
  schema.value.filter((d) => d.key !== 'pictureUrl'),
)
</script>

<template>
  <div class="ed">
    <header class="ed__bar" :style="barStyle">
      <button class="ed__back" @click="goBack">← Назад</button>
      <div class="ed__bar-spacer"></div>
      <span v-if="dirty" class="ed__dirty">несохранено</span>
      <button
        class="ed__save"
        :style="saveStyle"
        :disabled="!dirty || saving"
        @click="save"
      >
        {{ saving ? 'Сохранение…' : dirty ? 'Сохранить' : 'Сохранено' }}
      </button>
      <button
        class="ed__delete"
        title="Удалить сущность"
        @click="askDeleteEntity"
      >
        ✕
      </button>
    </header>

    <main class="ed__page">
      <div v-if="loading" class="ed__state">Загрузка…</div>

      <div v-else-if="notFound" class="ed__state">
        <div>Сущность не найдена.</div>
        <button class="ed__back-inline" @click="goBack">
          Вернуться к миру
        </button>
      </div>

      <template v-else>
        <section class="profile" :style="profileStyle">
          <div class="profile__pic-col">
            <div class="profile__pic" :style="picFrameStyle">
              <img
                v-if="pictureUrl"
                :src="pictureUrl"
                :alt="form.name"
                class="profile__pic-img"
              />
              <div
                v-else
                class="profile__pic-placeholder"
                :style="picPlaceholderStyle"
              >
                <span
                  class="profile__pic-icon"
                  :style="{ color: cfg?.color }"
                >
                  {{ cfg?.icon }}
                </span>
              </div>
            </div>

            <div v-if="supportsPicture" class="profile__pic-url">
              <label>URL изображения</label>
              <input
                type="text"
                :value="(form.properties.pictureUrl as string | null) ?? ''"
                placeholder="https://…"
                @input="onPictureUrlInput"
              />
            </div>
          </div>

          <div class="profile__info">
            <div class="profile__heading">
              <input
                v-model="form.name"
                class="profile__name"
                :style="{ color: cfg?.color }"
                placeholder="Название"
              />
              <div class="profile__meta">
                <TypeBadge v-if="typeKey" :type-key="typeKey" />
                <span class="profile__status-pill" :style="statusPillStyle">
                  <span
                    class="profile__status-dot"
                    :style="{ background: statusCfg.color }"
                  ></span>
                  {{ statusCfg.label }}
                </span>
              </div>
            </div>

            <div class="profile__field">
              <label>Описание</label>
              <textarea
                v-model="form.description"
                rows="6"
                placeholder="Краткое описание сущности…"
              />
            </div>

            <div class="profile__row">
              <div class="profile__field">
                <label>Статус</label>
                <select v-model="form.status">
                  <option v-for="[k, v] in STATUS_LIST" :key="k" :value="k">
                    {{ v.label }}
                  </option>
                </select>
              </div>

              <div class="profile__field profile__field--grow">
                <label>Теги</label>
                <TagsInput
                  v-model="form.tagsText"
                  placeholder="Введите и нажмите Enter…"
                />
              </div>
            </div>
          </div>
        </section>

        <section class="block">
          <div class="block__head">
            <h2 class="block__title" :style="sectionTitleStyle">
              Связи
              <span class="block__count">{{ connRels.length }}</span>
            </h2>
            <button
              class="block__action"
              @click="showCreator = !showCreator"
            >
              {{ showCreator ? '× Отмена' : '+ Добавить связь' }}
            </button>
          </div>

          <RelationCreator
            v-if="showCreator && entity"
            :world-id="props.worldId"
            :from="entity"
            @created="showCreator = false"
            @cancel="showCreator = false"
          />

          <div v-if="connRels.length" class="rel-grid">
            <div v-for="rel in connRels" :key="rel.id" class="rel">
              <template v-if="getOther(rel)">
                <div class="rel__meta">
                  <span class="rel__arrow" :style="{ color: cfg?.color }">
                    {{ getDirection(rel) }}
                  </span>
                  <span class="rel__label">{{ rel.label }}</span>
                  <button
                    class="rel__del"
                    title="Удалить связь"
                    @click="askDeleteRelation(rel)"
                  >
                    ×
                  </button>
                </div>
                <EntityCard
                  :entity="getOther(rel)!"
                  :relations="entitiesStore.relations"
                  compact
                  @select="goToEntity(getOther(rel)!)"
                />
              </template>
            </div>
          </div>
          <div v-else-if="!showCreator" class="block__empty">
            Нет связей. Нажмите «+ Добавить связь», чтобы создать первую.
          </div>
        </section>

        <section v-if="visibleSchema.length" class="block">
          <div class="block__head">
            <h2 class="block__title" :style="sectionTitleStyle">
              Дополнительные поля
            </h2>
          </div>
          <div class="props-grid">
            <DynamicField
              v-for="def in visibleSchema"
              :key="def.key"
              :def="def"
              :model-value="form.properties[def.key]"
              @update:model-value="form.properties[def.key] = $event"
            />
          </div>
        </section>
      </template>
    </main>

    <div v-if="error" class="ed__err">{{ error }}</div>

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
  </div>
</template>

<style scoped>
.ed {
  position: fixed;
  inset: 0;
  display: flex;
  flex-direction: column;
  background: var(--bg);
  overflow: hidden;
}

.ed__bar {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 12px 28px;
  flex-shrink: 0;
}

.ed__bar-spacer {
  flex: 1;
}

.ed__back {
  background: transparent;
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 6px 14px;
  font-family: var(--font-display);
  font-size: 13px;
  letter-spacing: 0.04em;
  color: var(--muted);
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.ed__back:hover {
  color: var(--text);
  border-color: var(--border2);
}

.ed__dirty {
  font-size: 12px;
  color: var(--muted);
  font-style: italic;
  margin-right: 4px;
}

.ed__save {
  padding: 7px 18px;
  border-radius: 8px;
  font-size: 13px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  cursor: pointer;
  transition: filter 0.15s, opacity 0.15s;
}

.ed__save:hover:not(:disabled) {
  filter: brightness(1.1);
}

.ed__save:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.ed__delete {
  padding: 7px 13px;
  border-radius: 8px;
  background: transparent;
  color: var(--muted);
  border: 1px solid var(--border);
  font-size: 14px;
  font-family: var(--font-display);
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.ed__delete:hover {
  color: #a02929;
  border-color: rgba(160, 41, 41, 0.4);
}

.ed__page {
  flex: 1;
  overflow-y: auto;
  padding: 24px 28px 80px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 24px;
}

.ed__page > * {
  width: 100%;
  max-width: 1080px;
}

.ed__state {
  width: 100%;
  padding: 60px 24px;
  color: var(--muted);
  font-style: italic;
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
}

.ed__back-inline {
  background: transparent;
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 6px 14px;
  font-family: var(--font-display);
  font-size: 12px;
  color: var(--muted);
  cursor: pointer;
}

.ed__back-inline:hover {
  color: var(--text);
}

.profile {
  display: grid;
  grid-template-columns: 240px 1fr;
  gap: 28px;
  padding: 24px;
  border-radius: 14px;
}

.profile__pic-col {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.profile__pic {
  width: 240px;
  height: 240px;
  border-radius: 12px;
  overflow: hidden;
  background: var(--card);
}

.profile__pic-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

.profile__pic-placeholder {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.profile__pic-icon {
  font-size: 72px;
  opacity: 0.35;
}

.profile__pic-url {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.profile__pic-url > label {
  font-size: 10px;
  color: var(--muted);
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.profile__pic-url input {
  width: 100%;
  font-family: var(--font-serif);
  font-size: 12px;
  background: rgba(255, 255, 255, 0.6);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 6px 8px;
}

.profile__info {
  display: flex;
  flex-direction: column;
  gap: 16px;
  min-width: 0;
}

.profile__heading {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.profile__name {
  background: transparent;
  border: none;
  border-bottom: 2px dashed transparent;
  font-family: var(--font-display);
  font-size: 32px;
  font-weight: 700;
  line-height: 1.2;
  padding: 4px 0;
  width: 100%;
  min-width: 0;
  transition: border-color 0.15s;
}

.profile__name:focus {
  outline: none;
  border-bottom-color: rgba(101, 67, 33, 0.35);
}

.profile__meta {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
}

.profile__status-pill {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  font-size: 13px;
  padding: 3px 12px;
  border-radius: 99px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  white-space: nowrap;
  line-height: 1.4;
}

.profile__status-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
}

.profile__field {
  display: flex;
  flex-direction: column;
  gap: 5px;
  min-width: 0;
}

.profile__field > label {
  font-size: 11px;
  color: var(--muted);
  font-family: var(--font-display);
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.profile__field input[type='text'],
.profile__field textarea,
.profile__field select {
  width: 100%;
  font-family: var(--font-serif);
  font-size: 14px;
  background: rgba(255, 255, 255, 0.7);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 8px 10px;
}

.profile__field textarea {
  resize: vertical;
  min-height: 90px;
}

.profile__row {
  display: grid;
  grid-template-columns: 200px 1fr;
  gap: 14px;
  align-items: start;
}

.profile__field--grow {
  min-width: 0;
}

.block {
  display: flex;
  flex-direction: column;
  gap: 14px;
  padding: 18px 22px 22px;
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 12px;
}

.block__head {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 12px;
}

.block__title {
  margin: 0;
  font-family: var(--font-display);
  font-size: 16px;
  font-weight: 600;
  letter-spacing: 0.04em;
  display: flex;
  align-items: center;
  gap: 10px;
}

.block__count {
  font-size: 12px;
  color: var(--muted);
  font-weight: 400;
  background: rgba(101, 67, 33, 0.08);
  padding: 1px 10px;
  border-radius: 99px;
}

.block__action {
  font-size: 12px;
  font-family: var(--font-display);
  letter-spacing: 0.04em;
  color: var(--muted);
  background: transparent;
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 5px 12px;
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.block__action:hover {
  color: var(--text);
  border-color: var(--border2);
}

.block__empty {
  color: var(--muted);
  font-style: italic;
  font-size: 13px;
  padding: 8px 0 4px;
}

.rel-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 14px;
}

.rel {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.rel__meta {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 0 4px;
}

.rel__arrow {
  font-size: 16px;
  flex-shrink: 0;
}

.rel__label {
  flex: 1;
  font-size: 13px;
  color: var(--text);
  font-style: italic;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.rel__del {
  color: var(--dim);
  font-size: 16px;
  line-height: 1;
  padding: 0 6px;
  background: transparent;
  border: none;
  cursor: pointer;
  transition: color 0.15s;
}

.rel__del:hover {
  color: #a02929;
}

.props-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
  gap: 14px;
}

.ed__err {
  margin: 0 28px 12px;
  padding: 10px 14px;
  font-size: 12px;
  color: #a02929;
  background: rgba(160, 41, 41, 0.06);
  border: 1px solid rgba(160, 41, 41, 0.25);
  border-radius: 6px;
  flex-shrink: 0;
}

@media (max-width: 760px) {
  .profile {
    grid-template-columns: 1fr;
  }

  .profile__pic-col {
    align-items: center;
  }

  .profile__row {
    grid-template-columns: 1fr;
  }
}
</style>
