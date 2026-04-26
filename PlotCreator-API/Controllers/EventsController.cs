using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Service.Implementations;
using PlotCreator_API.Controllers.Base;

namespace PlotCreator_API.Controllers
{
    [Route("api/events")]
    public class EventsController
        : EntityControllerBase<EventDto, EventCreateRequest, EventUpdateRequest>
    {
        public EventsController(IEventService svc) : base(svc) { }

        [HttpPost("/api/worlds/{worldId:int}/events")]
        public async Task<IActionResult> Create(int worldId, [FromBody] EventCreateRequest request) =>
            Map(await Service.CreateAsync(worldId, request));
    }
}
