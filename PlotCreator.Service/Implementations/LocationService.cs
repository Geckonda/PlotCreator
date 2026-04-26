using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public interface ILocationService
        : IEntityCrudService<LocationDto, LocationCreateRequest, LocationUpdateRequest>
    { }

    public class LocationService
        : EntityCrudServiceBase<Location, LocationDto, LocationCreateRequest, LocationUpdateRequest>,
          ILocationService
    {
        public LocationService(IEntityRepository<Location> repo, IRelationRepository relRepo)
            : base(repo, relRepo) { }

        protected override EntityType EntityType => EntityType.Location;

        protected override LocationDto ToDto(Location e) => new()
        {
            Id = e.Id,
            WorldId = e.WorldId,
            Name = e.Name,
            Tags = e.Tags,
            Status = e.Status,
            Description = e.Description,
            Region = e.Region,
            Climate = e.Climate,
            PictureUrl = e.PictureUrl
        };

        protected override void ApplyCreate(Location e, LocationCreateRequest r) => Apply(e, r);
        protected override void ApplyUpdate(Location e, LocationUpdateRequest r) => Apply(e, r);

        private static void Apply(Location e, LocationCreateRequest r)
        {
            e.Name = r.Name;
            e.Tags = r.Tags;
            e.Status = r.Status;
            e.Description = r.Description;
            e.Region = r.Region;
            e.Climate = r.Climate;
            e.PictureUrl = r.PictureUrl;
        }
    }
}
