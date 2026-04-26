using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Service.Implementations;
using PlotCreator_API.Controllers.Base;

namespace PlotCreator_API.Controllers
{
    [Route("api/locations")]
    public class LocationsController
        : EntityControllerBase<LocationDto, LocationCreateRequest, LocationUpdateRequest>
    {
        public LocationsController(ILocationService svc) : base(svc) { }

        [HttpPost("/api/worlds/{worldId:int}/locations")]
        public async Task<IActionResult> Create(int worldId, [FromBody] LocationCreateRequest request) =>
            Map(await Service.CreateAsync(worldId, request));
    }
}
