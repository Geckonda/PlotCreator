using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Service.Implementations;
using PlotCreator_API.Controllers.Base;

namespace PlotCreator_API.Controllers
{
    [Route("api/factions")]
    public class FactionsController
        : EntityControllerBase<FactionDto, FactionCreateRequest, FactionUpdateRequest>
    {
        public FactionsController(IFactionService svc) : base(svc) { }

        [HttpPost("/api/worlds/{worldId:int}/factions")]
        public async Task<IActionResult> Create(int worldId, [FromBody] FactionCreateRequest request) =>
            Map(await Service.CreateAsync(worldId, request));
    }
}
