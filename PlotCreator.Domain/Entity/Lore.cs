using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class Lore : EntityBase
    {
        [MaxLength(100)]
        public string? Era { get; set; }

        public string? Content { get; set; }
    }
}
