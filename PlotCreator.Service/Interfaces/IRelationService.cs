using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Interfaces;

namespace PlotCreator.Service.Interfaces
{
    public interface IRelationService
    {
        Task<IBaseResponse<IReadOnlyList<RelationDto>>> GetByWorldAsync(int worldId);
        Task<IBaseResponse<RelationDto>> CreateAsync(int worldId, RelationCreateRequest request);
        Task<IBaseResponse<RelationDto>> UpdateAsync(int id, RelationUpdateRequest request);
        Task<IBaseResponse<bool>> DeleteAsync(int id);
    }
}
