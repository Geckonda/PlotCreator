using System;
using System.Text.Json.Nodes;

namespace PlotCreator.Domain.Contracts
{
    public sealed class EntityTypeDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public int Radius { get; set; }
        public JsonArray PropertySchema { get; set; } = new();
        public bool IsSystemDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public sealed class EntityTypeCreateRequest
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public int Radius { get; set; } = 18;
        public JsonArray? PropertySchema { get; set; }
    }

    public sealed class EntityTypeUpdateRequest
    {
        public string Label { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public int Radius { get; set; } = 18;
        public JsonArray? PropertySchema { get; set; }
    }
}
