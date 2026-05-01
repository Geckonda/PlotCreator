using System.ComponentModel.DataAnnotations;
using PlotCreator.Domain.Entity.Base;
using PlotCreator.Domain.Enum;

namespace PlotCreator.Domain.Entity
{
    public class GraphEdge : EntityBase
    {
        public int GraphId { get; set; }
        public Graph? Graph { get; set; }

        public int FromNodeId { get; set; }
        public GraphNode? FromNode { get; set; }

        public int ToNodeId { get; set; }
        public GraphNode? ToNode { get; set; }

        public EdgeDirection Direction { get; set; } = EdgeDirection.None;

        [MaxLength(200)]
        public string? Label { get; set; }
    }
}
