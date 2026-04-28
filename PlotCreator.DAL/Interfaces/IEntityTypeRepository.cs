using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Interfaces
{
    public interface IEntityTypeRepository : IBaseRepository<EntityType>
    {
        Task<IReadOnlyList<EntityType>> GetByOwnerAsync(int ownerUserId);
        Task<EntityType?> GetByOwnerAndKeyAsync(int ownerUserId, string key);
        Task<int> CountEntitiesUsingTypeAsync(int typeId);
    }
}
