using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Service.Implementations;
using PlotCreator_API.Controllers.Base;

namespace PlotCreator_API.Controllers
{
    [Route("api/episodes")]
    public class EpisodesController
        : EntityControllerBase<EpisodeDto, EpisodeCreateRequest, EpisodeUpdateRequest>
    {
        public EpisodesController(IEpisodeService svc) : base(svc) { }

        [HttpPost("/api/worlds/{worldId:int}/episodes")]
        public async Task<IActionResult> Create(int worldId, [FromBody] EpisodeCreateRequest request) =>
            Map(await Service.CreateAsync(worldId, request));
    }
}
