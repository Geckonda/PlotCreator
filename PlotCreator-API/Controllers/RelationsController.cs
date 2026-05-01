using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Service.Interfaces;
using StatusCodeEnum = PlotCreator.Domain.Enum.StatusCode;

namespace PlotCreator_API.Controllers
{
    [ApiController]
    [Authorize]
    public class RelationsController : ControllerBase
    {
        private readonly IRelationService _relations;

        public RelationsController(IRelationService relations)
        {
            _relations = relations;
        }

        [HttpGet("api/worlds/{worldId:int}/relations")]
        public async Task<IActionResult> GetByWorld(int worldId) =>
            Map(await _relations.GetByWorldAsync(worldId));

        [HttpPost("api/worlds/{worldId:int}/relations")]
        public async Task<IActionResult> Create(int worldId, [FromBody] RelationCreateRequest request) =>
            Map(await _relations.CreateAsync(worldId, request));

        [HttpPut("api/relations/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RelationUpdateRequest request) =>
            Map(await _relations.UpdateAsync(id, request));

        [HttpDelete("api/relations/{id:int}")]
        public async Task<IActionResult> Delete(int id) => Map(await _relations.DeleteAsync(id));

        private IActionResult Map<T>(IBaseResponse<T> r) => r.StatusCode switch
        {
            StatusCodeEnum.Ok => Ok(r.Data),
            StatusCodeEnum.NotFound => NotFound(new { description = r.Description, errorForUser = r.ErrorForUser }),
            _ => StatusCode(500, new { description = r.Description, errorForUser = r.ErrorForUser })
        };
    }
}
