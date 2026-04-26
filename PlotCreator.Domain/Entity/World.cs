using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlotCreator.Domain.Entity
{
    public class World
    {
        public int Id { get; set; }

        public int OwnerUserId { get; set; }
        public User? Owner { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Genre { get; set; }

        public string? Description { get; set; }

        [MaxLength(9)]
        public string? Color { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Character> Characters { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<Event> Events { get; set; } = new();
        public List<Faction> Factions { get; set; } = new();
        public List<Episode> Episodes { get; set; } = new();
        public List<Artifact> Artifacts { get; set; } = new();
        public List<Lore> Lores { get; set; } = new();
        public List<Relation> Relations { get; set; } = new();
    }
}
