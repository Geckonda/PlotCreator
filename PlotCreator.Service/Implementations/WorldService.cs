using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WorldService : IWorldService
    {
        private readonly IWorldRepository _worlds;
        private readonly ICurrentUserService _currentUser;

        public WorldService(IWorldRepository worlds, ICurrentUserService currentUser)
        {
            _worlds = worlds;
            _currentUser = currentUser;
        }

        public async Task<IBaseResponse<IReadOnlyList<WorldDto>>> GetAllAsync()
        {
            var userId = _currentUser.GetUserId();
            var worlds = await _worlds.GetByOwnerAsync(userId);
            var dtos = new List<WorldDto>(worlds.Count);
            foreach (var w in worlds)
            {
                var count = await _worlds.GetEntityCountAsync(w.Id);
                dtos.Add(ToDto(w, count));
            }
            return Ok<IReadOnlyList<WorldDto>>(dtos);
        }

        public async Task<IBaseResponse<WorldDto>> GetByIdAsync(int id)
        {
            var world = await _worlds.GetAll().FirstOrDefaultAsync(w => w.Id == id);
            if (world is null) return NotFound<WorldDto>("World not found");
            var count = await _worlds.GetEntityCountAsync(id);
            return Ok(ToDto(world, count));
        }

        public async Task<IBaseResponse<WorldDto>> CreateAsync(WorldCreateRequest request)
        {
            var world = new World
            {
                OwnerUserId = _currentUser.GetUserId(),
                Name = request.Name,
                Genre = request.Genre,
                Description = request.Description,
                Color = request.Color
            };
            await _worlds.Add(world);
            return Ok(ToDto(world, 0));
        }

        public async Task<IBaseResponse<WorldDto>> UpdateAsync(int id, WorldUpdateRequest request)
        {
            var world = await _worlds.GetAll().FirstOrDefaultAsync(w => w.Id == id);
            if (world is null) return NotFound<WorldDto>("World not found");
            world.Name = request.Name;
            world.Genre = request.Genre;
            world.Description = request.Description;
            world.Color = request.Color;
            await _worlds.Update(world);
            var count = await _worlds.GetEntityCountAsync(id);
            return Ok(ToDto(world, count));
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(int id)
        {
            var world = await _worlds.GetAll().FirstOrDefaultAsync(w => w.Id == id);
            if (world is null) return NotFound<bool>("World not found");
            await _worlds.Delete(world);
            return Ok(true);
        }

        private static WorldDto ToDto(World w, int entitiesCount) => new()
        {
            Id = w.Id,
            Name = w.Name,
            Genre = w.Genre,
            Description = w.Description,
            Color = w.Color,
            EntitiesCount = entitiesCount,
            CreatedAt = w.CreatedAt,
            UpdatedAt = w.UpdatedAt
        };

        private static IBaseResponse<T> Ok<T>(T data) =>
            new BaseResponse<T> { Data = data, StatusCode = StatusCode.Ok };

        private static IBaseResponse<T> NotFound<T>(string description) =>
            new BaseResponse<T> { StatusCode = StatusCode.NotFound, Description = description, ErrorForUser = description };
    }
}
