using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Service.Implementations;
using PlotCreator_API.Controllers.Base;

namespace PlotCreator_API.Controllers
{
    [Route("api/lores")]
    public class LoresController
        : EntityControllerBase<LoreDto, LoreCreateRequest, LoreUpdateRequest>
    {
        public LoresController(ILoreService svc) : base(svc) { }

        [HttpPost("/api/worlds/{worldId:int}/lores")]
        public async Task<IActionResult> Create(int worldId, [FromBody] LoreCreateRequest request) =>
            Map(await Service.CreateAsync(worldId, request));
    }
}
