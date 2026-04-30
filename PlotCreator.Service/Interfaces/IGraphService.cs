using System.Collections.Generic;
using System.Threading.Tasks;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Interfaces;

namespace PlotCreator.Service.Interfaces
{
    public interface IGraphService
    {
        Task<IBaseResponse<IReadOnlyList<GraphSummaryDto>>> GetByWorldAsync(int worldId);
        Task<IBaseResponse<GraphDetailDto>> GetAsync(int graphId);
        Task<IBaseResponse<GraphSummaryDto>> CreateAsync(int worldId, GraphCreateRequest request);
        Task<IBaseResponse<GraphSummaryDto>> UpdateAsync(int graphId, GraphUpdateRequest request);
        Task<IBaseResponse<bool>> DeleteAsync(int graphId);

        Task<IBaseResponse<GraphNodeDto>> AddNodeAsync(int graphId, GraphNodeCreateRequest request);
        Task<IBaseResponse<GraphNodeDto>> UpdateNodePositionAsync(int graphId, int nodeId, GraphNodePositionRequest request);
        Task<IBaseResponse<bool>> RemoveNodeAsync(int graphId, int nodeId);

        Task<IBaseResponse<GraphEdgeDto>> AddEdgeAsync(int graphId, GraphEdgeCreateRequest request);
        Task<IBaseResponse<GraphEdgeDto>> UpdateEdgeAsync(int graphId, int edgeId, GraphEdgeUpdateRequest request);
        Task<IBaseResponse<bool>> RemoveEdgeAsync(int graphId, int edgeId);
    }
}
