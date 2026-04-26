using System;
using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Entity
{
    public class Relation
    {
        public int Id { get; set; }

        public int WorldId { get; set; }
        public World? World { get; set; }

        public int FromId { get; set; }
        public EntityType FromType { get; set; }

        public int ToId { get; set; }
        public EntityType ToType { get; set; }

        [Required]
        [MaxLength(200)]
        public string Label { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
