using System;
using System.Text.Json.Serialization;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Contracts
{
    public sealed class ArtifactDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; }
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public string? Material { get; set; }
        public string? Origin { get; set; }
        public string? PictureUrl { get; set; }
    }

    public class ArtifactCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; } = EntityStatus.Draft;
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public string? Material { get; set; }
        public string? Origin { get; set; }
        public string? PictureUrl { get; set; }
    }

    public sealed class ArtifactUpdateRequest : ArtifactCreateRequest { }
}
