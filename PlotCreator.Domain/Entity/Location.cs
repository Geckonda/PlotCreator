using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class Location : EntityBase
    {
        [MaxLength(200)]
        public string? Region { get; set; }

        [MaxLength(100)]
        public string? Climate { get; set; }

        [MaxLength(500)]
        public string? PictureUrl { get; set; }
    }
}
