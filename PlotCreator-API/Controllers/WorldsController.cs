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
    [Route("api/worlds")]
    public class WorldsController : ControllerBase
    {
        private readonly IWorldService _worlds;
        private readonly IEntityService _entities;

        public WorldsController(IWorldService worlds, IEntityService entities)
        {
            _worlds = worlds;
            _entities = entities;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Map(await _worlds.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id) => Map(await _worlds.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorldCreateRequest request)
        {
            var resp = await _worlds.CreateAsync(request);
            return resp.StatusCode == StatusCodeEnum.Ok
                ? CreatedAtAction(nameof(Get), new { id = resp.Data!.Id }, resp.Data)
                : Map(resp);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorldUpdateRequest request) =>
            Map(await _worlds.UpdateAsync(id, request));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) => Map(await _worlds.DeleteAsync(id));

        [HttpGet("{worldId:int}/entities")]
        public async Task<IActionResult> GetEntities(int worldId) =>
            Map(await _entities.GetAllForWorldAsync(worldId));

        private IActionResult Map<T>(IBaseResponse<T> r) => r.StatusCode switch
        {
            StatusCodeEnum.Ok => Ok(r.Data),
            StatusCodeEnum.NotFound => NotFound(new { description = r.Description, errorForUser = r.ErrorForUser }),
            StatusCodeEnum.Forbidden => Forbid(),
            _ => StatusCode(500, new { description = r.Description, errorForUser = r.ErrorForUser })
        };
    }
}
