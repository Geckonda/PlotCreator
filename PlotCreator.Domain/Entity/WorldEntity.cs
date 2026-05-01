using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Entity
{
    public class WorldEntity : EntityBase
    {
        public int WorldId { get; set; }
        public World? World { get; set; }

        public int TypeId { get; set; }
        public EntityType? Type { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public EntityStatus Status { get; set; } = EntityStatus.Draft;

        public string[] Tags { get; set; } = System.Array.Empty<string>();

        public string[] Aliases { get; set; } = System.Array.Empty<string>();

        public string PropertiesJson { get; set; } = "{}";

        public string ContentJson { get; set; } = "{\"type\":\"doc\",\"content\":[]}";

        public string ExtraContentsJson { get; set; } = "[]";
    }
}
