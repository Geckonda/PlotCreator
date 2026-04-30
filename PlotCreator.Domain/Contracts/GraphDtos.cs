using System;
using System.Collections.Generic;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Contracts
{
    public sealed class GraphSummaryDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public int OwnerUserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSystemDefault { get; set; }
        public string Kind { get; set; } = "Custom";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public sealed class GraphDetailDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public int OwnerUserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSystemDefault { get; set; }
        public string Kind { get; set; } = "Custom";
        public List<GraphNodeDto> Nodes { get; set; } = new();
        public List<GraphEdgeDto> Edges { get; set; } = new();
    }

    public sealed class GraphNodeDto
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string? EntityTypeKey { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public sealed class GraphEdgeDto
    {
        public int Id { get; set; }
        public int FromNodeId { get; set; }
        public int ToNodeId { get; set; }
        public EdgeDirection Direction { get; set; }
        public string? Label { get; set; }
    }

    public sealed class GraphCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public sealed class GraphUpdateRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public sealed class GraphNodeCreateRequest
    {
        public int EntityId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public sealed class GraphNodePositionRequest
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public sealed class GraphEdgeCreateRequest
    {
        public int FromNodeId { get; set; }
        public int ToNodeId { get; set; }
        public EdgeDirection Direction { get; set; } = EdgeDirection.None;
        public string? Label { get; set; }
    }

    public sealed class GraphEdgeUpdateRequest
    {
        public EdgeDirection? Direction { get; set; }
        public string? Label { get; set; }
    }
}
