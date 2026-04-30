using System;

namespace PlotCreator.Domain.Contracts
{
    public sealed class RelationDto
    {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string? Label { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public sealed class RelationCreateRequest
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string? Label { get; set; }
    }

    public sealed class RelationUpdateRequest
    {
        public string? Label { get; set; }
    }
}
