using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;

namespace PlotCreator.DAL.Interfaces
{
    public interface IRelationRepository : IBaseRepository<Relation>
    {
        Task<IReadOnlyList<Relation>> GetByWorldIdAsync(int worldId);
        Task DeleteForEntityAsync(int entityId, EntityType type);
    }
}
