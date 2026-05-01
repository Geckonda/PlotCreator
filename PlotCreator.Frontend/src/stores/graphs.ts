import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import type {
  GraphSummary,
  GraphDetail,
  GraphCreateRequest,
  GraphUpdateRequest,
  GraphNodeCreateRequest,
  GraphEdgeCreateRequest,
  GraphEdgeUpdateRequest,
} from '@/types/graph'
import * as api from '@/services/graphs'

export const useGraphsStore = defineStore('graphs', () => {
  const summaries = ref<GraphSummary[]>([])
  const current = ref<GraphDetail | null>(null)
  const loadingList = ref(false)
  const loadingCurrent = ref(false)
  const error = ref<string | null>(null)

  const defaultGraph = computed(() =>
    summaries.value.find((g) => g.isSystemDefault) ?? null,
  )

  async function fetchByWorld(worldId: number) {
    loadingList.value = true
    error.value = null
    try {
      summaries.value = await api.listGraphs(worldId)
    } catch (e: unknown) {
      const err = e as any
      error.value = err?.response?.data?.errorForUser || (e instanceof Error ? e.message : 'Failed to load graphs')
      // Re-throw authorization errors so the view can handle them
      if (err?.response?.status === 403) {
        throw e
      }
    } finally {
      loadingList.value = false
    }
  }

  async function fetchOne(graphId: number) {
    loadingCurrent.value = true
    error.value = null
    try {
      current.value = await api.getGraph(graphId)
    } catch (e: unknown) {
      const err = e as any
      error.value = err?.response?.data?.errorForUser || (e instanceof Error ? e.message : 'Failed to load graph')
      current.value = null
      // Re-throw authorization errors so the view can handle them
      if (err?.response?.status === 403) {
        throw e
      }
    } finally {
      loadingCurrent.value = false
    }
  }

  function clear() {
    summaries.value = []
    current.value = null
    error.value = null
  }

  async function createGraph(worldId: number, body: GraphCreateRequest) {
    const dto = await api.createGraph(worldId, body)
    summaries.value.push(dto)
    return dto
  }

  async function updateGraphMeta(graphId: number, body: GraphUpdateRequest) {
    const dto = await api.updateGraph(graphId, body)
    const i = summaries.value.findIndex((g) => g.id === graphId)
    if (i >= 0) summaries.value[i] = dto
    if (current.value?.id === graphId) {
      current.value = { ...current.value, name: dto.name, description: dto.description }
    }
    return dto
  }

  async function deleteGraph(graphId: number) {
    await api.deleteGraph(graphId)
    summaries.value = summaries.value.filter((g) => g.id !== graphId)
    if (current.value?.id === graphId) current.value = null
  }

  async function addNode(graphId: number, body: GraphNodeCreateRequest) {
    const node = await api.addNode(graphId, body)
    if (current.value?.id === graphId) {
      current.value.nodes.push(node)
    }
    return node
  }

  async function updateNodePosition(graphId: number, nodeId: number, x: number, y: number) {
    const node = await api.updateNodePosition(graphId, nodeId, { x, y })
    if (current.value?.id === graphId) {
      const i = current.value.nodes.findIndex((n) => n.id === nodeId)
      if (i >= 0) current.value.nodes[i] = node
    }
  }

  async function removeNode(graphId: number, nodeId: number) {
    await api.removeNode(graphId, nodeId)
    if (current.value?.id === graphId) {
      current.value.nodes = current.value.nodes.filter((n) => n.id !== nodeId)
      current.value.edges = current.value.edges.filter(
        (e) => e.fromNodeId !== nodeId && e.toNodeId !== nodeId,
      )
    }
  }

  async function addEdge(graphId: number, body: GraphEdgeCreateRequest) {
    const edge = await api.addEdge(graphId, body)
    if (current.value?.id === graphId) current.value.edges.push(edge)
    return edge
  }

  async function updateEdge(graphId: number, edgeId: number, body: GraphEdgeUpdateRequest) {
    const edge = await api.updateEdge(graphId, edgeId, body)
    if (current.value?.id === graphId) {
      const i = current.value.edges.findIndex((e) => e.id === edgeId)
      if (i >= 0) current.value.edges[i] = edge
    }
    return edge
  }

  async function removeEdge(graphId: number, edgeId: number) {
    await api.removeEdge(graphId, edgeId)
    if (current.value?.id === graphId) {
      current.value.edges = current.value.edges.filter((e) => e.id !== edgeId)
    }
  }

  return {
    summaries,
    current,
    defaultGraph,
    loadingList,
    loadingCurrent,
    error,
    fetchByWorld,
    fetchOne,
    clear,
    createGraph,
    updateGraphMeta,
    deleteGraph,
    addNode,
    updateNodePosition,
    removeNode,
    addEdge,
    updateEdge,
    removeEdge,
  }
})
