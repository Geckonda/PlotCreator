using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Interfaces
{
    public interface IWorldRepository : IBaseRepository<World>
    {
        Task<IReadOnlyList<World>> GetByOwnerAsync(int ownerUserId);
        Task<int> GetEntityCountAsync(int worldId);
    }
}
