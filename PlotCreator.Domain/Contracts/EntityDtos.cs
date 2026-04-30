using System;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Contracts
{
    public sealed class EntityDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public string TypeKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public string[] Aliases { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; }
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public JsonObject Properties { get; set; } = new();
        public JsonNode Content { get; set; } = JsonNode.Parse("{\"type\":\"doc\",\"content\":[]}")!;
    }

    public class EntityCreateRequest
    {
        public string TypeKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public string[] Aliases { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; } = EntityStatus.Draft;
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public JsonObject? Properties { get; set; }
        public JsonNode? Content { get; set; }
    }

    public sealed class EntityUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public string[] Aliases { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; } = EntityStatus.Draft;
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public JsonObject? Properties { get; set; }
        public JsonNode? Content { get; set; }
    }
}
