using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Interfaces;

namespace PlotCreator.Service.Interfaces
{
    public interface IEntityService
    {
        Task<IBaseResponse<IReadOnlyList<EntitySummaryDto>>> GetAllForWorldAsync(int worldId);
        Task<IBaseResponse<EntityDto>> GetByIdAsync(int id);
        Task<IBaseResponse<EntityDto>> CreateAsync(int worldId, EntityCreateRequest request);
        Task<IBaseResponse<EntityDto>> UpdateAsync(int id, EntityUpdateRequest request);
        Task<IBaseResponse<bool>> DeleteAsync(int id);
    }
}
