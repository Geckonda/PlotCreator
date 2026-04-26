using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Interfaces;

namespace PlotCreator.Service.Interfaces
{
    public interface IEntityService
    {
        Task<IBaseResponse<IReadOnlyList<EntitySummaryDto>>> GetAllForWorldAsync(int worldId);
    }
}
