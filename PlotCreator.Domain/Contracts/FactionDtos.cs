using System;
using System.Text.Json.Serialization;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Contracts
{
    public sealed class FactionDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; }
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public string? Ideology { get; set; }
        public string? Headquarters { get; set; }
    }

    public class FactionCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; } = EntityStatus.Draft;
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public string? Ideology { get; set; }
        public string? Headquarters { get; set; }
    }

    public sealed class FactionUpdateRequest : FactionCreateRequest { }
}
