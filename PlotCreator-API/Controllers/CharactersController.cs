using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Service.Implementations;
using PlotCreator_API.Controllers.Base;

namespace PlotCreator_API.Controllers
{
    [Route("api/characters")]
    public class CharactersController
        : EntityControllerBase<CharacterDto, CharacterCreateRequest, CharacterUpdateRequest>
    {
        public CharactersController(ICharacterService svc) : base(svc) { }

        [HttpPost("/api/worlds/{worldId:int}/characters")]
        public async Task<IActionResult> Create(int worldId, [FromBody] CharacterCreateRequest request) =>
            Map(await Service.CreateAsync(worldId, request));
    }
}
