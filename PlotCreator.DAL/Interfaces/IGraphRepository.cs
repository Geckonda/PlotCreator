using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Interfaces
{
    public interface IGraphRepository : IBaseRepository<Graph>
    {
        Task<IReadOnlyList<Graph>> GetByWorldAndUserAsync(int worldId, int userId);
        Task<Graph?> GetSummaryAsync(int graphId);
        Task<Graph?> GetWithContentsAsync(int graphId);
        Task<Graph> EnsureSystemDefaultAsync(int worldId, int userId);
    }
}
