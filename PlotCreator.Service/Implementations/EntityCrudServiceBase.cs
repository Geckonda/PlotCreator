using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity.Base;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public abstract class EntityCrudServiceBase<T, TDto, TCreate, TUpdate>
        : IEntityCrudService<TDto, TCreate, TUpdate>
        where T : EntityBase, new()
    {
        protected readonly IEntityRepository<T> Repository;
        protected readonly IRelationRepository RelationRepository;

        protected abstract EntityType EntityType { get; }

        protected EntityCrudServiceBase(IEntityRepository<T> repository, IRelationRepository relationRepository)
        {
            Repository = repository;
            RelationRepository = relationRepository;
        }

        public async Task<IBaseResponse<IReadOnlyList<TDto>>> GetByWorldAsync(int worldId)
        {
            var entities = await Repository.GetByWorldIdAsync(worldId);
            return Ok<IReadOnlyList<TDto>>(entities.Select(ToDto).ToList());
        }

        public async Task<IBaseResponse<TDto>> GetByIdAsync(int id)
        {
            var entity = await Repository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null) return NotFound<TDto>($"{typeof(T).Name} not found");
            return Ok(ToDto(entity));
        }

        public async Task<IBaseResponse<TDto>> CreateAsync(int worldId, TCreate request)
        {
            var entity = new T { WorldId = worldId };
            ApplyCreate(entity, request);
            await Repository.Add(entity);
            return Ok(ToDto(entity));
        }

        public async Task<IBaseResponse<TDto>> UpdateAsync(int id, TUpdate request)
        {
            var entity = await Repository.GetAll().FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null) return NotFound<TDto>($"{typeof(T).Name} not found");
            ApplyUpdate(entity, request);
            await Repository.Update(entity);
            return Ok(ToDto(entity));
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(int id)
        {
            var entity = await Repository.GetAll().FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null) return NotFound<bool>($"{typeof(T).Name} not found");
            await Repository.Delete(entity);
            await RelationRepository.DeleteForEntityAsync(id, EntityType);
            return Ok(true);
        }

        protected abstract TDto ToDto(T entity);
        protected abstract void ApplyCreate(T entity, TCreate request);
        protected abstract void ApplyUpdate(T entity, TUpdate request);

        protected static IBaseResponse<TR> Ok<TR>(TR data) =>
            new BaseResponse<TR> { Data = data, StatusCode = StatusCode.Ok };

        protected static IBaseResponse<TR> NotFound<TR>(string desc) =>
            new BaseResponse<TR> { StatusCode = StatusCode.NotFound, Description = desc, ErrorForUser = desc };
    }
}
