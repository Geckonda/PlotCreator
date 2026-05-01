using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Interfaces;

namespace PlotCreator.Service.Interfaces
{
    public interface IEntityTypeService
    {
        Task<IBaseResponse<IReadOnlyList<EntityTypeDto>>> GetForCurrentUserAsync();
        Task<IBaseResponse<EntityTypeDto>> CreateAsync(EntityTypeCreateRequest request);
        Task<IBaseResponse<EntityTypeDto>> UpdateAsync(int id, EntityTypeUpdateRequest request);
        Task<IBaseResponse<bool>> DeleteAsync(int id);
        Task EnsureSeededAsync();
    }
}
