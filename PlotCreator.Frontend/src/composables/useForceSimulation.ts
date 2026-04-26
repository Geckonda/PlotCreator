import { onUnmounted, ref, watch, type Ref } from 'vue'
import type { Entity, Relation } from '@/types/entity'

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
  const positions: Record<number, Pos> = {}
  const velocities: Record<number, Vel> = {}

  const tick = ref(0)
  const dragId = ref<number | null>(null)

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
      if (positions[e.id]) return
      const angle = (2 * Math.PI * i) / Math.max(list.length, 1)
      const r = Math.min(w, h) * 0.28 + (Math.random() - 0.5) * 60
      positions[e.id] = { x: cx + r * Math.cos(angle), y: cy + r * Math.sin(angle) }
      velocities[e.id] = { vx: 0, vy: 0 }
    })

    const ids = new Set<number>(list.map((e) => e.id))
    for (const key of Object.keys(positions)) {
      const id = Number(key)
      if (!ids.has(id)) {
        delete positions[id]
        delete velocities[id]
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
    const ids = entities.value.map((e) => e.id).filter((id) => positions[id])

    for (let i = 0; i < ids.length; i++) {
      for (let j = i + 1; j < ids.length; j++) {
        const a = ids[i]
        const b = ids[j]
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
      const fp = positions[rel.from]
      const tp = positions[rel.to]
      if (!fp || !tp) continue
      const dx = tp.x - fp.x
      const dy = tp.y - fp.y
      const d = Math.sqrt(dx * dx + dy * dy) || 1
      const f = (d - 170) * 0.045 * cool
      velocities[rel.from].vx += (dx / d) * f
      velocities[rel.from].vy += (dy / d) * f
      velocities[rel.to].vx -= (dx / d) * f
      velocities[rel.to].vy -= (dy / d) * f
    }

    for (const id of ids) {
      velocities[id].vx += (cx - positions[id].x) * 0.0025 * cool
      velocities[id].vy += (cy - positions[id].y) * 0.0025 * cool
    }

    for (const id of ids) {
      if (dragId.value === id) continue
      velocities[id].vx *= 0.8
      velocities[id].vy *= 0.8
      positions[id].x += velocities[id].vx
      positions[id].y += velocities[id].vy
      positions[id].x = Math.max(32, Math.min(w - 32, positions[id].x))
      positions[id].y = Math.max(32, Math.min(h - 32, positions[id].y))
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

  function setDragPosition(id: number, x: number, y: number) {
    if (!positions[id]) return
    positions[id].x = x
    positions[id].y = y
    velocities[id] = { vx: 0, vy: 0 }
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
