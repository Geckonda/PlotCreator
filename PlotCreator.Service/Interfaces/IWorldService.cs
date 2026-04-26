using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Interfaces;

namespace PlotCreator.Service.Interfaces
{
    public interface IWorldService
    {
        Task<IBaseResponse<IReadOnlyList<WorldDto>>> GetAllAsync();
        Task<IBaseResponse<WorldDto>> GetByIdAsync(int id);
        Task<IBaseResponse<WorldDto>> CreateAsync(WorldCreateRequest request);
        Task<IBaseResponse<WorldDto>> UpdateAsync(int id, WorldUpdateRequest request);
        Task<IBaseResponse<bool>> DeleteAsync(int id);
    }
}
