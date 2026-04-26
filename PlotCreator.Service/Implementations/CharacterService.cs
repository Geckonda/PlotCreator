using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public interface ICharacterService
        : IEntityCrudService<CharacterDto, CharacterCreateRequest, CharacterUpdateRequest>
    { }

    public class CharacterService
        : EntityCrudServiceBase<Character, CharacterDto, CharacterCreateRequest, CharacterUpdateRequest>,
          ICharacterService
    {
        public CharacterService(IEntityRepository<Character> repo, IRelationRepository relRepo)
            : base(repo, relRepo) { }

        protected override EntityType EntityType => EntityType.Character;

        protected override CharacterDto ToDto(Character e) => new()
        {
            Id = e.Id,
            WorldId = e.WorldId,
            Name = e.Name,
            Tags = e.Tags,
            Status = e.Status,
            Description = e.Description,
            Birthday = e.Birthday,
            Deathday = e.Deathday,
            Gender = e.Gender,
            Height = e.Height,
            Weight = e.Weight,
            Personality = e.Personality,
            Appearance = e.Appearance,
            Conflict = e.Conflict,
            Goals = e.Goals,
            Motivation = e.Motivation,
            History = e.History,
            PictureUrl = e.PictureUrl
        };

        protected override void ApplyCreate(Character e, CharacterCreateRequest r) => Apply(e, r);
        protected override void ApplyUpdate(Character e, CharacterUpdateRequest r) => Apply(e, r);

        private static void Apply(Character e, CharacterCreateRequest r)
        {
            e.Name = r.Name;
            e.Tags = r.Tags;
            e.Status = r.Status;
            e.Description = r.Description;
            e.Birthday = r.Birthday;
            e.Deathday = r.Deathday;
            e.Gender = r.Gender;
            e.Height = r.Height;
            e.Weight = r.Weight;
            e.Personality = r.Personality;
            e.Appearance = r.Appearance;
            e.Conflict = r.Conflict;
            e.Goals = r.Goals;
            e.Motivation = r.Motivation;
            e.History = r.History;
            e.PictureUrl = r.PictureUrl;
        }
    }
}
