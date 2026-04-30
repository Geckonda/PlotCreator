using System.Threading.Tasks;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Interfaces
{
    public interface IGraphNodeRepository : IBaseRepository<GraphNode>
    {
        Task<GraphNode?> FindAsync(int nodeId);
        Task<bool> ExistsAsync(int graphId, int entityId);
        Task RemoveWithAttachedEdgesAsync(int nodeId);
    }
}
