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
    public class EntitiesController : ControllerBase
    {
        private readonly IEntityService _entities;

        public EntitiesController(IEntityService entities)
        {
            _entities = entities;
        }

        [HttpGet("api/entities/{id:int}")]
        public async Task<IActionResult> Get(int id) =>
            Map(await _entities.GetByIdAsync(id));

        [HttpGet("api/worlds/{worldId:int}/entities/{id:int}")]
        public async Task<IActionResult> GetFromWorld(int worldId, int id) =>
            Map(await _entities.GetByIdAsync(worldId, id));

        [HttpPost("api/worlds/{worldId:int}/entities")]
        public async Task<IActionResult> Create(int worldId, [FromBody] EntityCreateRequest request) =>
            Map(await _entities.CreateAsync(worldId, request));

        [HttpPut("api/entities/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EntityUpdateRequest request) =>
            Map(await _entities.UpdateAsync(id, request));

        [HttpPut("api/worlds/{worldId:int}/entities/{id:int}")]
        public async Task<IActionResult> UpdateInWorld(int worldId, int id, [FromBody] EntityUpdateRequest request) =>
            Map(await _entities.UpdateAsync(worldId, id, request));

        [HttpDelete("api/entities/{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            Map(await _entities.DeleteAsync(id));

        [HttpDelete("api/worlds/{worldId:int}/entities/{id:int}")]
        public async Task<IActionResult> DeleteFromWorld(int worldId, int id) =>
            Map(await _entities.DeleteAsync(worldId, id));

        private IActionResult Map<T>(IBaseResponse<T> r) => r.StatusCode switch
        {
            StatusCodeEnum.Ok => Ok(r.Data),
            StatusCodeEnum.NotFound => NotFound(new { description = r.Description, errorForUser = r.ErrorForUser }),
            StatusCodeEnum.Forbidden => Forbid(),
            _ => StatusCode(500, new { description = r.Description, errorForUser = r.ErrorForUser })
        };
    }
}
