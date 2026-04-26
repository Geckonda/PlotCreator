using System;
using System.Text.Json.Serialization;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Contracts
{
    public sealed class CharacterDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; }
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public DateOnly? Birthday { get; set; }
        public DateOnly? Deathday { get; set; }
        public string? Gender { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string? Personality { get; set; }
        public string? Appearance { get; set; }
        public string? Conflict { get; set; }
        public string? Goals { get; set; }
        public string? Motivation { get; set; }
        public string? History { get; set; }
        public string? PictureUrl { get; set; }
    }

    public class CharacterCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; } = EntityStatus.Draft;
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public DateOnly? Birthday { get; set; }
        public DateOnly? Deathday { get; set; }
        public string? Gender { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string? Personality { get; set; }
        public string? Appearance { get; set; }
        public string? Conflict { get; set; }
        public string? Goals { get; set; }
        public string? Motivation { get; set; }
        public string? History { get; set; }
        public string? PictureUrl { get; set; }
    }

    public sealed class CharacterUpdateRequest : CharacterCreateRequest { }
}
