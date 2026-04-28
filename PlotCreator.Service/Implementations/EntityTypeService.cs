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
    public class EntityTypeService : IEntityTypeService
    {
        private readonly IEntityTypeRepository _types;
        private readonly ICurrentUserService _currentUser;

        public EntityTypeService(IEntityTypeRepository types, ICurrentUserService currentUser)
        {
            _types = types;
            _currentUser = currentUser;
        }

        public async Task<IBaseResponse<IReadOnlyList<EntityTypeDto>>> GetForCurrentUserAsync()
        {
            await EnsureSeededAsync();
            var userId = _currentUser.GetUserId();
            var types = await _types.GetByOwnerAsync(userId);
            return Ok<IReadOnlyList<EntityTypeDto>>(types.Select(ToDto).ToList());
        }

        public async Task<IBaseResponse<EntityTypeDto>> CreateAsync(EntityTypeCreateRequest request)
        {
            await EnsureSeededAsync();
            var userId = _currentUser.GetUserId();

            var key = NormalizeKey(request.Key);
            if (string.IsNullOrEmpty(key))
                return BadRequest<EntityTypeDto>("Key is required");

            var existing = await _types.GetByOwnerAndKeyAsync(userId, key);
            if (existing is not null)
                return BadRequest<EntityTypeDto>($"Type with key '{key}' already exists");

            var type = new EntityType
            {
                OwnerUserId = userId,
                Key = key,
                Label = request.Label,
                Color = request.Color,
                Icon = request.Icon,
                Radius = request.Radius,
                PropertySchemaJson = request.PropertySchema?.ToJsonString() ?? "[]",
                IsSystemDefault = false
            };
            await _types.Add(type);
            return Ok(ToDto(type));
        }

        public async Task<IBaseResponse<EntityTypeDto>> UpdateAsync(int id, EntityTypeUpdateRequest request)
        {
            var userId = _currentUser.GetUserId();
            var type = await _types.GetAll().FirstOrDefaultAsync(t => t.Id == id);
            if (type is null) return NotFound<EntityTypeDto>("Entity type not found");
            if (type.OwnerUserId != userId) return NotFound<EntityTypeDto>("Entity type not found");

            type.Label = request.Label;
            type.Color = request.Color;
            type.Icon = request.Icon;
            type.Radius = request.Radius;
            if (request.PropertySchema is not null)
                type.PropertySchemaJson = request.PropertySchema.ToJsonString();

            await _types.Update(type);
            return Ok(ToDto(type));
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(int id)
        {
            var userId = _currentUser.GetUserId();
            var type = await _types.GetAll().FirstOrDefaultAsync(t => t.Id == id);
            if (type is null) return NotFound<bool>("Entity type not found");
            if (type.OwnerUserId != userId) return NotFound<bool>("Entity type not found");

            var inUse = await _types.CountEntitiesUsingTypeAsync(id);
            if (inUse > 0)
                return BadRequest<bool>($"Cannot delete: {inUse} entities use this type");

            await _types.Delete(type);
            return Ok(true);
        }

        public async Task EnsureSeededAsync()
        {
            var userId = _currentUser.GetUserId();
            var existing = await _types.GetByOwnerAsync(userId);
            if (existing.Count > 0) return;

            foreach (var seed in DefaultEntityTypes.All)
            {
                var type = new EntityType
                {
                    OwnerUserId = userId,
                    Key = seed.Key,
                    Label = seed.Label,
                    Color = seed.Color,
                    Icon = seed.Icon,
                    Radius = seed.Radius,
                    PropertySchemaJson = seed.PropertySchemaJson,
                    IsSystemDefault = true
                };
                await _types.Add(type);
            }
        }

        private static string NormalizeKey(string key) =>
            (key ?? string.Empty).Trim().ToLowerInvariant().Replace(' ', '-');

        private static EntityTypeDto ToDto(EntityType t) => new()
        {
            Id = t.Id,
            Key = t.Key,
            Label = t.Label,
            Color = t.Color,
            Icon = t.Icon,
            Radius = t.Radius,
            PropertySchema = JsonNode.Parse(t.PropertySchemaJson)?.AsArray() ?? new JsonArray(),
            IsSystemDefault = t.IsSystemDefault,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        };

        private static IBaseResponse<T> Ok<T>(T data) =>
            new BaseResponse<T> { Data = data, StatusCode = StatusCode.Ok };

        private static IBaseResponse<T> NotFound<T>(string desc) =>
            new BaseResponse<T> { StatusCode = StatusCode.NotFound, Description = desc, ErrorForUser = desc };

        private static IBaseResponse<T> BadRequest<T>(string desc) =>
            new BaseResponse<T> { StatusCode = StatusCode.InternalServerError, Description = desc, ErrorForUser = desc };
    }
}
