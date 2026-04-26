using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public interface ILoreService
        : IEntityCrudService<LoreDto, LoreCreateRequest, LoreUpdateRequest>
    { }

    public class LoreService
        : EntityCrudServiceBase<Lore, LoreDto, LoreCreateRequest, LoreUpdateRequest>,
          ILoreService
    {
        public LoreService(IEntityRepository<Lore> repo, IRelationRepository relRepo)
            : base(repo, relRepo) { }

        protected override EntityType EntityType => EntityType.Lore;

        protected override LoreDto ToDto(Lore e) => new()
        {
            Id = e.Id,
            WorldId = e.WorldId,
            Name = e.Name,
            Tags = e.Tags,
            Status = e.Status,
            Description = e.Description,
            Era = e.Era,
            Content = e.Content
        };

        protected override void ApplyCreate(Lore e, LoreCreateRequest r) => Apply(e, r);
        protected override void ApplyUpdate(Lore e, LoreUpdateRequest r) => Apply(e, r);

        private static void Apply(Lore e, LoreCreateRequest r)
        {
            e.Name = r.Name;
            e.Tags = r.Tags;
            e.Status = r.Status;
            e.Description = r.Description;
            e.Era = r.Era;
            e.Content = r.Content;
        }
    }
}
