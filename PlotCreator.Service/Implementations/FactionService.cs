using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public interface IFactionService
        : IEntityCrudService<FactionDto, FactionCreateRequest, FactionUpdateRequest>
    { }

    public class FactionService
        : EntityCrudServiceBase<Faction, FactionDto, FactionCreateRequest, FactionUpdateRequest>,
          IFactionService
    {
        public FactionService(IEntityRepository<Faction> repo, IRelationRepository relRepo)
            : base(repo, relRepo) { }

        protected override EntityType EntityType => EntityType.Faction;

        protected override FactionDto ToDto(Faction e) => new()
        {
            Id = e.Id,
            WorldId = e.WorldId,
            Name = e.Name,
            Tags = e.Tags,
            Status = e.Status,
            Description = e.Description,
            Ideology = e.Ideology,
            Headquarters = e.Headquarters
        };

        protected override void ApplyCreate(Faction e, FactionCreateRequest r) => Apply(e, r);
        protected override void ApplyUpdate(Faction e, FactionUpdateRequest r) => Apply(e, r);

        private static void Apply(Faction e, FactionCreateRequest r)
        {
            e.Name = r.Name;
            e.Tags = r.Tags;
            e.Status = r.Status;
            e.Description = r.Description;
            e.Ideology = r.Ideology;
            e.Headquarters = r.Headquarters;
        }
    }
}
