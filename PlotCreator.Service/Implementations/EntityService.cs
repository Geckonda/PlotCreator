using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Entity.Base;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public class EntityService : IEntityService
    {
        private readonly IEntityRepository<Character> _characters;
        private readonly IEntityRepository<Location> _locations;
        private readonly IEntityRepository<Event> _events;
        private readonly IEntityRepository<Faction> _factions;
        private readonly IEntityRepository<Episode> _episodes;
        private readonly IEntityRepository<Artifact> _artifacts;
        private readonly IEntityRepository<Lore> _lores;

        public EntityService(
            IEntityRepository<Character> characters,
            IEntityRepository<Location> locations,
            IEntityRepository<Event> events,
            IEntityRepository<Faction> factions,
            IEntityRepository<Episode> episodes,
            IEntityRepository<Artifact> artifacts,
            IEntityRepository<Lore> lores)
        {
            _characters = characters;
            _locations = locations;
            _events = events;
            _factions = factions;
            _episodes = episodes;
            _artifacts = artifacts;
            _lores = lores;
        }

        public async Task<IBaseResponse<IReadOnlyList<EntitySummaryDto>>> GetAllForWorldAsync(int worldId)
        {
            var result = new List<EntitySummaryDto>();
            result.AddRange(Project(await _characters.GetByWorldIdAsync(worldId), EntityType.Character));
            result.AddRange(Project(await _locations.GetByWorldIdAsync(worldId), EntityType.Location));
            result.AddRange(Project(await _events.GetByWorldIdAsync(worldId), EntityType.Event));
            result.AddRange(Project(await _factions.GetByWorldIdAsync(worldId), EntityType.Faction));
            result.AddRange(Project(await _episodes.GetByWorldIdAsync(worldId), EntityType.Episode));
            result.AddRange(Project(await _artifacts.GetByWorldIdAsync(worldId), EntityType.Artifact));
            result.AddRange(Project(await _lores.GetByWorldIdAsync(worldId), EntityType.Lore));

            return new BaseResponse<IReadOnlyList<EntitySummaryDto>>
            {
                Data = result,
                StatusCode = StatusCode.Ok
            };
        }

        private static IEnumerable<EntitySummaryDto> Project<T>(IReadOnlyList<T> items, EntityType type)
            where T : EntityBase =>
            items.Select(e => new EntitySummaryDto
            {
                Id = e.Id,
                Type = type,
                Name = e.Name,
                Tags = e.Tags,
                Status = e.Status,
                Description = e.Description
            });
    }
}
