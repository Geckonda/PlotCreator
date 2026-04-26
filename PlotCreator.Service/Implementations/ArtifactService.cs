using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public interface IArtifactService
        : IEntityCrudService<ArtifactDto, ArtifactCreateRequest, ArtifactUpdateRequest>
    { }

    public class ArtifactService
        : EntityCrudServiceBase<Artifact, ArtifactDto, ArtifactCreateRequest, ArtifactUpdateRequest>,
          IArtifactService
    {
        public ArtifactService(IEntityRepository<Artifact> repo, IRelationRepository relRepo)
            : base(repo, relRepo) { }

        protected override EntityType EntityType => EntityType.Artifact;

        protected override ArtifactDto ToDto(Artifact e) => new()
        {
            Id = e.Id,
            WorldId = e.WorldId,
            Name = e.Name,
            Tags = e.Tags,
            Status = e.Status,
            Description = e.Description,
            Material = e.Material,
            Origin = e.Origin,
            PictureUrl = e.PictureUrl
        };

        protected override void ApplyCreate(Artifact e, ArtifactCreateRequest r) => Apply(e, r);
        protected override void ApplyUpdate(Artifact e, ArtifactUpdateRequest r) => Apply(e, r);

        private static void Apply(Artifact e, ArtifactCreateRequest r)
        {
            e.Name = r.Name;
            e.Tags = r.Tags;
            e.Status = r.Status;
            e.Description = r.Description;
            e.Material = r.Material;
            e.Origin = r.Origin;
            e.PictureUrl = r.PictureUrl;
        }
    }
}
