using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class Graph : EntityBase
    {
        public int OwnerUserId { get; set; }
        public User? Owner { get; set; }

        public int WorldId { get; set; }
        public World? World { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsSystemDefault { get; set; }

        [Required]
        [MaxLength(40)]
        public string Kind { get; set; } = "Custom";

        public List<GraphNode> Nodes { get; set; } = new();
        public List<GraphEdge> Edges { get; set; } = new();
    }
}
