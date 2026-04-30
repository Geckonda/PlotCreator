import { computed, type ComputedRef, type Ref } from 'vue'
import type { Entity } from '@/types/entity'

export interface EntitySearchOptions {
  /** Hide these entity ids from the result (e.g. already-picked ones). */
  excludeIds?: Ref<ReadonlySet<number>> | ComputedRef<ReadonlySet<number>>
  /** When true (default), an empty query returns the full source list. */
  showAllOnEmptyQuery?: boolean
  /** Cap the result length. 0 / undefined means no cap. */
  limit?: number
}

export function useEntitySearch(
  source: Ref<Entity[]> | ComputedRef<Entity[]>,
  query: Ref<string>,
  opts: EntitySearchOptions = {},
) {
  const { excludeIds, showAllOnEmptyQuery = true, limit = 0 } = opts

  return computed<Entity[]>(() => {
    const q = query.value.trim().toLowerCase()
    const skip = excludeIds?.value
    const list = source.value

    const matches: Entity[] = []
    for (const e of list) {
      if (skip?.has(e.id)) continue
      if (q) {
        const inName = e.name.toLowerCase().includes(q)
        const inTags = e.tags.some((t) => t.toLowerCase().includes(q))
        if (!inName && !inTags) continue
      } else if (!showAllOnEmptyQuery) {
        continue
      }
      matches.push(e)
      if (limit > 0 && matches.length >= limit) break
    }
    return matches
  })
}
