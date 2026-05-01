using System;

namespace PlotCreator.Domain.Contracts
{
    public sealed class WorldDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public int EntitiesCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public sealed class WorldCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }

    public sealed class WorldUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
