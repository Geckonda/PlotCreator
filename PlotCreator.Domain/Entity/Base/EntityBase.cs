using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Entity.Base
{
    public abstract class EntityBase
    {
        public int Id { get; set; }

        public int WorldId { get; set; }
        public World? World { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public EntityStatus Status { get; set; } = EntityStatus.Draft;

        public string[] Tags { get; set; } = System.Array.Empty<string>();

        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
