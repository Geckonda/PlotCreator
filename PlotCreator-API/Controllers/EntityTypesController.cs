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
    [Route("api/users/me/entity-types")]
    public class EntityTypesController : ControllerBase
    {
        private readonly IEntityTypeService _types;

        public EntityTypesController(IEntityTypeService types)
        {
            _types = types;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Map(await _types.GetForCurrentUserAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EntityTypeCreateRequest request) =>
            Map(await _types.CreateAsync(request));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EntityTypeUpdateRequest request) =>
            Map(await _types.UpdateAsync(id, request));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            Map(await _types.DeleteAsync(id));

        private IActionResult Map<T>(IBaseResponse<T> r) => r.StatusCode switch
        {
            StatusCodeEnum.Ok => Ok(r.Data),
            StatusCodeEnum.NotFound => NotFound(new { description = r.Description, errorForUser = r.ErrorForUser }),
            _ => StatusCode(500, new { description = r.Description, errorForUser = r.ErrorForUser })
        };
    }
}
