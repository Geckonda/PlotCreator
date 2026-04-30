using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.Domain.Entity
{
    public class GraphNode : EntityBase
    {
        public int GraphId { get; set; }
        public Graph? Graph { get; set; }

        public int EntityId { get; set; }
        public WorldEntity? Entity { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
