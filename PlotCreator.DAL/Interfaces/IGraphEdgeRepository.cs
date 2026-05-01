using System.Threading.Tasks;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Interfaces
{
    public interface IGraphEdgeRepository : IBaseRepository<GraphEdge>
    {
        Task<GraphEdge?> FindAsync(int edgeId);
    }
}
