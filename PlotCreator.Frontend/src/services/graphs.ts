import api from './api'
import type {
  GraphSummary,
  GraphDetail,
  GraphCreateRequest,
  GraphUpdateRequest,
  GraphNode,
  GraphNodeCreateRequest,
  GraphNodePositionRequest,
  GraphEdgeData,
  GraphEdgeCreateRequest,
  GraphEdgeUpdateRequest,
} from '@/types/graph'

export const listGraphs = (worldId: number) =>
  api.get<GraphSummary[]>(`/worlds/${worldId}/graphs`).then((r) => r.data)

export const getGraph = (id: number) =>
  api.get<GraphDetail>(`/graphs/${id}`).then((r) => r.data)

export const createGraph = (worldId: number, body: GraphCreateRequest) =>
  api
    .post<GraphSummary>(`/worlds/${worldId}/graphs`, body)
    .then((r) => r.data)

export const updateGraph = (id: number, body: GraphUpdateRequest) =>
  api.patch<GraphSummary>(`/graphs/${id}`, body).then((r) => r.data)

export const deleteGraph = (id: number) =>
  api.delete<void>(`/graphs/${id}`).then(() => undefined)

export const addNode = (id: number, body: GraphNodeCreateRequest) =>
  api.post<GraphNode>(`/graphs/${id}/nodes`, body).then((r) => r.data)

export const updateNodePosition = (
  id: number,
  nodeId: number,
  body: GraphNodePositionRequest,
) =>
  api
    .patch<GraphNode>(`/graphs/${id}/nodes/${nodeId}`, body)
    .then((r) => r.data)

export const removeNode = (id: number, nodeId: number) =>
  api.delete<void>(`/graphs/${id}/nodes/${nodeId}`).then(() => undefined)

export const addEdge = (id: number, body: GraphEdgeCreateRequest) =>
  api.post<GraphEdgeData>(`/graphs/${id}/edges`, body).then((r) => r.data)

export const updateEdge = (
  id: number,
  edgeId: number,
  body: GraphEdgeUpdateRequest,
) =>
  api
    .patch<GraphEdgeData>(`/graphs/${id}/edges/${edgeId}`, body)
    .then((r) => r.data)

export const removeEdge = (id: number, edgeId: number) =>
  api.delete<void>(`/graphs/${id}/edges/${edgeId}`).then(() => undefined)
