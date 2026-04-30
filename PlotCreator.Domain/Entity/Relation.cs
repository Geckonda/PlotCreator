using System;
using System.ComponentModel.DataAnnotations;

namespace PlotCreator.Domain.Entity
{
    public class Relation
    {
        public int Id { get; set; }

        public int WorldId { get; set; }
        public World? World { get; set; }

        public int FromId { get; set; }
        public WorldEntity? From { get; set; }

        public int ToId { get; set; }
        public WorldEntity? To { get; set; }

        [MaxLength(200)]
        public string? Label { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
