using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Service.Implementations;
using PlotCreator_API.Controllers.Base;

namespace PlotCreator_API.Controllers
{
    [Route("api/artifacts")]
    public class ArtifactsController
        : EntityControllerBase<ArtifactDto, ArtifactCreateRequest, ArtifactUpdateRequest>
    {
        public ArtifactsController(IArtifactService svc) : base(svc) { }

        [HttpPost("/api/worlds/{worldId:int}/artifacts")]
        public async Task<IActionResult> Create(int worldId, [FromBody] ArtifactCreateRequest request) =>
            Map(await Service.CreateAsync(worldId, request));
    }
}
