using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Response.Interfaces;

namespace PlotCreator.Service.Interfaces
{
    public interface IEntityCrudService<TDto, TCreate, TUpdate>
    {
        Task<IBaseResponse<IReadOnlyList<TDto>>> GetByWorldAsync(int worldId);
        Task<IBaseResponse<TDto>> GetByIdAsync(int id);
        Task<IBaseResponse<TDto>> CreateAsync(int worldId, TCreate request);
        Task<IBaseResponse<TDto>> UpdateAsync(int id, TUpdate request);
        Task<IBaseResponse<bool>> DeleteAsync(int id);
    }
}
