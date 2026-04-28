using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class EntityType : EntityBase
    {
        public int OwnerUserId { get; set; }
        public User? Owner { get; set; }

        [Required]
        [MaxLength(50)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Label { get; set; } = string.Empty;

        [MaxLength(9)]
        public string? Color { get; set; }

        [MaxLength(50)]
        public string? Icon { get; set; }

        public int Radius { get; set; } = 18;

        public string PropertySchemaJson { get; set; } = "[]";

        public bool IsSystemDefault { get; set; }

        public List<WorldEntity> Entities { get; set; } = new();
    }
}
