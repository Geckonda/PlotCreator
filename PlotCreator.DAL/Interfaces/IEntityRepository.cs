using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Interfaces
{
    public interface IEntityRepository : IBaseRepository<WorldEntity>
    {
        Task<IReadOnlyList<WorldEntity>> GetByWorldIdAsync(int worldId);
        Task<WorldEntity?> GetWithTypeAsync(int id);
    }
}
