using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public class EntityService : IEntityService
    {
        private const string EmptyContent = "{\"type\":\"doc\",\"content\":[]}";

        private readonly IEntityRepository _entities;
        private readonly IEntityTypeRepository _types;
        private readonly IRelationRepository _relations;
        private readonly ICurrentUserService _currentUser;
        private readonly IEntityTypeService _typeService;

        public EntityService(
            IEntityRepository entities,
            IEntityTypeRepository types,
            IRelationRepository relations,
            ICurrentUserService currentUser,
            IEntityTypeService typeService)
        {
            _entities = entities;
            _types = types;
            _relations = relations;
            _currentUser = currentUser;
            _typeService = typeService;
        }

        public async Task<IBaseResponse<IReadOnlyList<EntitySummaryDto>>> GetAllForWorldAsync(int worldId)
        {
            var entities = await _entities.GetByWorldIdAsync(worldId);
            var summaries = entities.Select(e => new EntitySummaryDto
            {
                Id = e.Id,
                TypeKey = e.Type?.Key ?? string.Empty,
                Name = e.Name,
                Tags = e.Tags,
                Status = e.Status,
                Description = e.Description
            }).ToList();
            return Ok<IReadOnlyList<EntitySummaryDto>>(summaries);
        }

        public async Task<IBaseResponse<EntityDto>> GetByIdAsync(int id)
        {
            var entity = await _entities.GetWithTypeAsync(id);
            if (entity is null) return NotFound<EntityDto>("Entity not found");
            return Ok(ToDto(entity));
        }

        public async Task<IBaseResponse<EntityDto>> CreateAsync(int worldId, EntityCreateRequest request)
        {
            await _typeService.EnsureSeededAsync();

            var userId = _currentUser.GetUserId();
            var type = await _types.GetByOwnerAndKeyAsync(userId, request.TypeKey);
            if (type is null) return NotFound<EntityDto>($"Entity type '{request.TypeKey}' not found");

            var entity = new WorldEntity
            {
                WorldId = worldId,
                TypeId = type.Id,
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                Tags = request.Tags,
                PropertiesJson = request.Properties?.ToJsonString() ?? "{}",
                ContentJson = request.Content?.ToJsonString() ?? EmptyContent
            };
            await _entities.Add(entity);

            entity.Type = type;
            return Ok(ToDto(entity));
        }

        public async Task<IBaseResponse<EntityDto>> UpdateAsync(int id, EntityUpdateRequest request)
        {
            var entity = await _entities.GetAll().FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null) return NotFound<EntityDto>("Entity not found");

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Status = request.Status;
            entity.Tags = request.Tags;
            if (request.Properties is not null)
                entity.PropertiesJson = request.Properties.ToJsonString();
            if (request.Content is not null)
                entity.ContentJson = request.Content.ToJsonString();

            await _entities.Update(entity);

            var withType = await _entities.GetWithTypeAsync(entity.Id);
            return Ok(ToDto(withType ?? entity));
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(int id)
        {
            var entity = await _entities.GetAll().FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null) return NotFound<bool>("Entity not found");

            await _relations.DeleteForEntityAsync(id);
            await _entities.Delete(entity);
            return Ok(true);
        }

        private static EntityDto ToDto(WorldEntity e) => new()
        {
            Id = e.Id,
            WorldId = e.WorldId,
            TypeKey = e.Type?.Key ?? string.Empty,
            Name = e.Name,
            Tags = e.Tags,
            Status = e.Status,
            Description = e.Description,
            Properties = JsonNode.Parse(e.PropertiesJson)?.AsObject() ?? new JsonObject(),
            Content = JsonNode.Parse(e.ContentJson) ?? JsonNode.Parse(EmptyContent)!
        };

        private static IBaseResponse<T> Ok<T>(T data) =>
            new BaseResponse<T> { Data = data, StatusCode = StatusCode.Ok };

        private static IBaseResponse<T> NotFound<T>(string desc) =>
            new BaseResponse<T> { StatusCode = StatusCode.NotFound, Description = desc, ErrorForUser = desc };
    }
}
