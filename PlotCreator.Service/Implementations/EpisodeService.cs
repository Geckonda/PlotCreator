using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public interface IEpisodeService
        : IEntityCrudService<EpisodeDto, EpisodeCreateRequest, EpisodeUpdateRequest>
    { }

    public class EpisodeService
        : EntityCrudServiceBase<Episode, EpisodeDto, EpisodeCreateRequest, EpisodeUpdateRequest>,
          IEpisodeService
    {
        public EpisodeService(IEntityRepository<Episode> repo, IRelationRepository relRepo)
            : base(repo, relRepo) { }

        protected override EntityType EntityType => EntityType.Episode;

        protected override EpisodeDto ToDto(Episode e) => new()
        {
            Id = e.Id,
            WorldId = e.WorldId,
            Name = e.Name,
            Tags = e.Tags,
            Status = e.Status,
            Description = e.Description,
            Position = e.Position,
            Content = e.Content
        };

        protected override void ApplyCreate(Episode e, EpisodeCreateRequest r) => Apply(e, r);
        protected override void ApplyUpdate(Episode e, EpisodeUpdateRequest r) => Apply(e, r);

        private static void Apply(Episode e, EpisodeCreateRequest r)
        {
            e.Name = r.Name;
            e.Tags = r.Tags;
            e.Status = r.Status;
            e.Description = r.Description;
            e.Position = r.Position;
            e.Content = r.Content;
        }
    }
}
