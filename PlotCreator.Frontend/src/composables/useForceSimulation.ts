import { onUnmounted, ref, watch, type Ref } from 'vue'
import type { Entity, EntityKey, Relation } from '@/types/entity'
import { entityKey, relationFromKey, relationToKey } from '@/types/entity'

interface Pos { x: number; y: number }
interface Vel { vx: number; vy: number }

export interface ForceSimulationOptions {
  width: Ref<number>
  height: Ref<number>
}

export function useForceSimulation(
  entities: Ref<Entity[]>,
  relations: Ref<Relation[]>,
  opts: ForceSimulationOptions,
) {
  const positions: Record<EntityKey, Pos> = {}
  const velocities: Record<EntityKey, Vel> = {}

  const tick = ref(0)
  const dragId = ref<EntityKey | null>(null)

  let ticks = 0
  let frame: number | null = null
  let running = false

  function ensurePositions() {
    const w = opts.width.value || 900
    const h = opts.height.value || 600
    const cx = w / 2
    const cy = h / 2
    const list = entities.value

    list.forEach((e, i) => {
      const k = entityKey(e.type, e.id)
      if (positions[k]) return
      const angle = (2 * Math.PI * i) / Math.max(list.length, 1)
      const r = Math.min(w, h) * 0.28 + (Math.random() - 0.5) * 60
      positions[k] = { x: cx + r * Math.cos(angle), y: cy + r * Math.sin(angle) }
      velocities[k] = { vx: 0, vy: 0 }
    })

    const keys = new Set<EntityKey>(list.map((e) => entityKey(e.type, e.id)))
    for (const key of Object.keys(positions)) {
      if (!keys.has(key)) {
        delete positions[key]
        delete velocities[key]
      }
    }
  }

  function step() {
    if (!running) return
    ticks++
    const w = opts.width.value
    const h = opts.height.value
    if (w === 0 || h === 0) {
      frame = requestAnimationFrame(step)
      return
    }
    const cx = w / 2
    const cy = h / 2
    const cool = Math.max(0.02, 1 - ticks / 350)
    const keys = entities.value
      .map((e) => entityKey(e.type, e.id))
      .filter((k) => positions[k])

    for (let i = 0; i < keys.length; i++) {
      for (let j = i + 1; j < keys.length; j++) {
        const a = keys[i]
        const b = keys[j]
        const dx = positions[b].x - positions[a].x
        const dy = positions[b].y - positions[a].y
        const d2 = dx * dx + dy * dy + 0.01
        const inv = Math.sqrt(d2)
        const f = (14000 * cool) / d2
        velocities[a].vx -= (dx / inv) * f
        velocities[a].vy -= (dy / inv) * f
        velocities[b].vx += (dx / inv) * f
        velocities[b].vy += (dy / inv) * f
      }
    }

    for (const rel of relations.value) {
      const fk = relationFromKey(rel)
      const tk = relationToKey(rel)
      const fp = positions[fk]
      const tp = positions[tk]
      if (!fp || !tp) continue
      const dx = tp.x - fp.x
      const dy = tp.y - fp.y
      const d = Math.sqrt(dx * dx + dy * dy) || 1
      const f = (d - 170) * 0.045 * cool
      velocities[fk].vx += (dx / d) * f
      velocities[fk].vy += (dy / d) * f
      velocities[tk].vx -= (dx / d) * f
      velocities[tk].vy -= (dy / d) * f
    }

    for (const k of keys) {
      velocities[k].vx += (cx - positions[k].x) * 0.0025 * cool
      velocities[k].vy += (cy - positions[k].y) * 0.0025 * cool
    }

    for (const k of keys) {
      if (dragId.value === k) continue
      velocities[k].vx *= 0.8
      velocities[k].vy *= 0.8
      positions[k].x += velocities[k].vx
      positions[k].y += velocities[k].vy
      positions[k].x = Math.max(32, Math.min(w - 32, positions[k].x))
      positions[k].y = Math.max(32, Math.min(h - 32, positions[k].y))
    }

    if (ticks % 2 === 0) tick.value++
    frame = requestAnimationFrame(step)
  }

  function start() {
    if (running) return
    running = true
    ticks = 0
    ensurePositions()
    frame = requestAnimationFrame(step)
  }

  function stop() {
    running = false
    if (frame !== null) cancelAnimationFrame(frame)
    frame = null
  }

  function setDragPosition(key: EntityKey, x: number, y: number) {
    if (!positions[key]) return
    positions[key].x = x
    positions[key].y = y
    velocities[key] = { vx: 0, vy: 0 }
    tick.value++
  }

  watch(
    [entities, relations],
    () => {
      ensurePositions()
      ticks = 0
    },
    { flush: 'post' },
  )

  onUnmounted(() => stop())

  return {
    positions,
    tick,
    dragId,
    start,
    stop,
    setDragPosition,
  }
}
