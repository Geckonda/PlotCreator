using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public interface IEventService
        : IEntityCrudService<EventDto, EventCreateRequest, EventUpdateRequest>
    { }

    public class EventService
        : EntityCrudServiceBase<Event, EventDto, EventCreateRequest, EventUpdateRequest>,
          IEventService
    {
        public EventService(IEntityRepository<Event> repo, IRelationRepository relRepo)
            : base(repo, relRepo) { }

        protected override EntityType EntityType => EntityType.Event;

        protected override EventDto ToDto(Event e) => new()
        {
            Id = e.Id,
            WorldId = e.WorldId,
            Name = e.Name,
            Tags = e.Tags,
            Status = e.Status,
            Description = e.Description,
            Beginning = e.Beginning,
            Ending = e.Ending,
            ChekhovsGun = e.ChekhovsGun
        };

        protected override void ApplyCreate(Event e, EventCreateRequest r) => Apply(e, r);
        protected override void ApplyUpdate(Event e, EventUpdateRequest r) => Apply(e, r);

        private static void Apply(Event e, EventCreateRequest r)
        {
            e.Name = r.Name;
            e.Tags = r.Tags;
            e.Status = r.Status;
            e.Description = r.Description;
            e.Beginning = r.Beginning;
            e.Ending = r.Ending;
            e.ChekhovsGun = r.ChekhovsGun;
        }
    }
}
