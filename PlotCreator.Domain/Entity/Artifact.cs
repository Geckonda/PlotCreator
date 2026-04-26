using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class Artifact : EntityBase
    {
        [MaxLength(200)]
        public string? Material { get; set; }

        public string? Origin { get; set; }

        [MaxLength(500)]
        public string? PictureUrl { get; set; }
    }
}
