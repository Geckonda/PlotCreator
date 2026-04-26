using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Service.Interfaces;
using StatusCodeEnum = PlotCreator.Domain.Enum.StatusCode;

namespace PlotCreator_API.Controllers.Base
{
    [ApiController]
    public abstract class EntityControllerBase<TDto, TCreate, TUpdate> : ControllerBase
    {
        protected readonly IEntityCrudService<TDto, TCreate, TUpdate> Service;

        protected EntityControllerBase(IEntityCrudService<TDto, TCreate, TUpdate> service)
        {
            Service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id) => Map(await Service.GetByIdAsync(id));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TUpdate request) =>
            Map(await Service.UpdateAsync(id, request));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) => Map(await Service.DeleteAsync(id));

        protected IActionResult Map<T>(IBaseResponse<T> r) => r.StatusCode switch
        {
            StatusCodeEnum.Ok => Ok(r.Data),
            StatusCodeEnum.NotFound => NotFound(new { description = r.Description, errorForUser = r.ErrorForUser }),
            _ => StatusCode(500, new { description = r.Description, errorForUser = r.ErrorForUser })
        };
    }
}
