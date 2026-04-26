using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class Faction : EntityBase
    {
        public string? Ideology { get; set; }

        [MaxLength(200)]
        public string? Headquarters { get; set; }
    }
}
