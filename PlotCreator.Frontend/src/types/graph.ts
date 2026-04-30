export type EdgeDirection = 'none' | 'forward' | 'backward' | 'both'

export interface GraphSummary {
  id: number
  worldId: number
  ownerUserId: number
  name: string
  description: string | null
  isSystemDefault: boolean
  kind: string
  createdAt: string
  updatedAt: string
}

export interface GraphNode {
  id: number
  entityId: number
  entityName: string
  entityTypeKey: string | null
  x: number
  y: number
}

export interface GraphEdgeData {
  id: number
  fromNodeId: number
  toNodeId: number
  direction: EdgeDirection
  label: string | null
}

export interface GraphDetail {
  id: number
  worldId: number
  ownerUserId: number
  name: string
  description: string | null
  isSystemDefault: boolean
  kind: string
  nodes: GraphNode[]
  edges: GraphEdgeData[]
}

export interface GraphCreateRequest {
  name: string
  description?: string | null
}

export interface GraphUpdateRequest {
  name?: string
  description?: string | null
}

export interface GraphNodeCreateRequest {
  entityId: number
  x: number
  y: number
}

export interface GraphNodePositionRequest {
  x: number
  y: number
}

export interface GraphEdgeCreateRequest {
  fromNodeId: number
  toNodeId: number
  direction: EdgeDirection
  label?: string | null
}

export interface GraphEdgeUpdateRequest {
  direction?: EdgeDirection
  label?: string | null
}
