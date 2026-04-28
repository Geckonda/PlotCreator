using System.Text.Json.Serialization;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Contracts
{
    public sealed class EntitySummaryDto
    {
        public int Id { get; set; }
        public string TypeKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string[] Tags { get; set; } = System.Array.Empty<string>();
        public EntityStatus Status { get; set; }
        [JsonPropertyName("desc")]
        public string? Description { get; set; }
    }
}
