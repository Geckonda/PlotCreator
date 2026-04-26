using System;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Contracts
{
    public sealed class RelationDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public int FromId { get; set; }
        public EntityType FromType { get; set; }
        public int ToId { get; set; }
        public EntityType ToType { get; set; }
        public string Label { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public sealed class RelationCreateRequest
    {
        public int FromId { get; set; }
        public EntityType FromType { get; set; }
        public int ToId { get; set; }
        public EntityType ToType { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    public sealed class RelationUpdateRequest
    {
        public string Label { get; set; } = string.Empty;
    }
}
