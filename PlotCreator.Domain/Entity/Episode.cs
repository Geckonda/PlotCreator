using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class Episode : EntityBase
    {
        public int Position { get; set; }
        public string? Content { get; set; }
    }
}
