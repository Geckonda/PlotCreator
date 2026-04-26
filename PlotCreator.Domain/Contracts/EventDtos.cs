using System;
using System.Text.Json.Serialization;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Contracts
{
    public sealed class EventDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; }
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public DateOnly? Beginning { get; set; }
        public DateOnly? Ending { get; set; }
        public bool ChekhovsGun { get; set; }
    }

    public class EventCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EntityStatus Status { get; set; } = EntityStatus.Draft;
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        public DateOnly? Beginning { get; set; }
        public DateOnly? Ending { get; set; }
        public bool ChekhovsGun { get; set; }
    }

    public sealed class EventUpdateRequest : EventCreateRequest { }
}
