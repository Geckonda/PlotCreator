using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Service.Interfaces;
using StatusCodeEnum = PlotCreator.Domain.Enum.StatusCode;

namespace PlotCreator_API.Controllers
{
    [ApiController]
    public class GraphsController : ControllerBase
    {
        private readonly IGraphService _graphs;

        public GraphsController(IGraphService graphs)
        {
            _graphs = graphs;
        }

        [HttpGet("api/worlds/{worldId:int}/graphs")]
        public async Task<IActionResult> GetByWorld(int worldId) =>
            Map(await _graphs.GetByWorldAsync(worldId));

        [HttpGet("api/graphs/{id:int}")]
        public async Task<IActionResult> Get(int id) =>
            Map(await _graphs.GetAsync(id));

        [HttpPost("api/worlds/{worldId:int}/graphs")]
        public async Task<IActionResult> Create(int worldId, [FromBody] GraphCreateRequest request) =>
            Map(await _graphs.CreateAsync(worldId, request));

        [HttpPatch("api/graphs/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GraphUpdateRequest request) =>
            Map(await _graphs.UpdateAsync(id, request));

        [HttpDelete("api/graphs/{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            Map(await _graphs.DeleteAsync(id));

        [HttpPost("api/graphs/{id:int}/nodes")]
        public async Task<IActionResult> AddNode(int id, [FromBody] GraphNodeCreateRequest request) =>
            Map(await _graphs.AddNodeAsync(id, request));

        [HttpPatch("api/graphs/{id:int}/nodes/{nodeId:int}")]
        public async Task<IActionResult> UpdateNode(int id, int nodeId, [FromBody] GraphNodePositionRequest request) =>
            Map(await _graphs.UpdateNodePositionAsync(id, nodeId, request));

        [HttpDelete("api/graphs/{id:int}/nodes/{nodeId:int}")]
        public async Task<IActionResult> RemoveNode(int id, int nodeId) =>
            Map(await _graphs.RemoveNodeAsync(id, nodeId));

        [HttpPost("api/graphs/{id:int}/edges")]
        public async Task<IActionResult> AddEdge(int id, [FromBody] GraphEdgeCreateRequest request) =>
            Map(await _graphs.AddEdgeAsync(id, request));

        [HttpPatch("api/graphs/{id:int}/edges/{edgeId:int}")]
        public async Task<IActionResult> UpdateEdge(int id, int edgeId, [FromBody] GraphEdgeUpdateRequest request) =>
            Map(await _graphs.UpdateEdgeAsync(id, edgeId, request));

        [HttpDelete("api/graphs/{id:int}/edges/{edgeId:int}")]
        public async Task<IActionResult> RemoveEdge(int id, int edgeId) =>
            Map(await _graphs.RemoveEdgeAsync(id, edgeId));

        private IActionResult Map<T>(IBaseResponse<T> r) => r.StatusCode switch
        {
            StatusCodeEnum.Ok => Ok(r.Data),
            StatusCodeEnum.Forbidden => StatusCode(403, new { description = r.Description, errorForUser = r.ErrorForUser }),
            StatusCodeEnum.NotFound => NotFound(new { description = r.Description, errorForUser = r.ErrorForUser }),
            StatusCodeEnum.Conflict => Conflict(new { description = r.Description, errorForUser = r.ErrorForUser }),
            _ => StatusCode(500, new { description = r.Description, errorForUser = r.ErrorForUser })
        };
    }
}
