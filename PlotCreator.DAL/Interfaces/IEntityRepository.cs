using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity.Base;

namespace PlotCreator.DAL.Interfaces
{
    public interface IEntityRepository<T> : IBaseRepository<T> where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetByWorldIdAsync(int worldId);
    }
}
