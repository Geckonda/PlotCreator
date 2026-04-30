import { Extension } from '@tiptap/core'
import { Plugin, PluginKey } from '@tiptap/pm/state'
import { Decoration, DecorationSet } from '@tiptap/pm/view'
import type { Node as PMNode } from '@tiptap/pm/model'

export interface EntityRef {
  id: number
  name: string
  aliases: string[]
}

export interface EntityHighlightOptions {
  getEntities: () => readonly EntityRef[]
  getRelatedIds: () => ReadonlySet<number>
  getExcludeId: () => number | null
}

export const entityHighlightPluginKey = new PluginKey('entityHighlight')

function escapeRegExp(s: string): string {
  return s.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')
}

interface MatchTerm {
  id: number
  text: string
}

function buildIndex(entities: readonly EntityRef[], excludeId: number | null) {
  const terms: MatchTerm[] = []
  for (const e of entities) {
    if (excludeId !== null && e.id === excludeId) continue
    const candidates = [e.name, ...(e.aliases ?? [])]
    for (const raw of candidates) {
      const t = (raw ?? '').trim()
      if (!t) continue
      terms.push({ id: e.id, text: t })
    }
  }
  // Longest first so "Aragorn the Brave" wins over "Aragorn"
  terms.sort((a, b) => b.text.length - a.text.length)

  const textToId = new Map<string, number>()
  for (const t of terms) {
    const k = t.text.toLowerCase()
    if (!textToId.has(k)) textToId.set(k, t.id)
  }
  return { terms, textToId }
}

function buildDecorations(
  doc: PMNode,
  opts: EntityHighlightOptions,
): DecorationSet {
  const entities = opts.getEntities()
  const excludeId = opts.getExcludeId()
  const related = opts.getRelatedIds()

  if (!entities || entities.length === 0) return DecorationSet.empty

  const { terms, textToId } = buildIndex(entities, excludeId)
  if (terms.length === 0) return DecorationSet.empty

  const grouped = terms.map((t) => escapeRegExp(t.text)).join('|')
  // Word boundary using Unicode letter/number lookbehind/lookahead so it works
  // for Cyrillic / accented text, not just ASCII (\b would fail there).
  let re: RegExp
  try {
    re = new RegExp(`(?<![\\p{L}\\p{N}_])(?:${grouped})(?![\\p{L}\\p{N}_])`, 'giu')
  } catch {
    return DecorationSet.empty
  }

  const decorations: Decoration[] = []

  doc.descendants((node, pos) => {
    if (!node.isText || !node.text) return
    const text = node.text
    re.lastIndex = 0
    let m: RegExpExecArray | null
    while ((m = re.exec(text)) !== null) {
      const matched = m[0]
      const id = textToId.get(matched.toLowerCase())
      if (id === undefined) continue
      const from = pos + m.index
      const to = from + matched.length
      const isRelated = related.has(id)
      decorations.push(
        Decoration.inline(from, to, {
          class: `entity-mention ${isRelated ? 'entity-mention--related' : 'entity-mention--unrelated'}`,
          'data-entity-id': String(id),
        }),
      )
    }
  })

  return DecorationSet.create(doc, decorations)
}

export const EntityHighlight = Extension.create<EntityHighlightOptions>({
  name: 'entityHighlight',

  addOptions() {
    return {
      getEntities: () => [],
      getRelatedIds: () => new Set<number>(),
      getExcludeId: () => null,
    }
  },

  addProseMirrorPlugins() {
    const opts = this.options
    return [
      new Plugin({
        key: entityHighlightPluginKey,
        state: {
          init: (_config, { doc }) => buildDecorations(doc, opts),
          apply: (tr, old) => {
            if (tr.getMeta(entityHighlightPluginKey)) {
              return buildDecorations(tr.doc, opts)
            }
            if (tr.docChanged) {
              return buildDecorations(tr.doc, opts)
            }
            return old.map(tr.mapping, tr.doc)
          },
        },
        props: {
          decorations(state) {
            return entityHighlightPluginKey.getState(state)
          },
        },
      }),
    ]
  },
})
