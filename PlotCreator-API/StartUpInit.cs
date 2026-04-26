using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator_API
{
    public static class StartUpInit
    {
        public static void InitialiseRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddScoped<IRelationRepository, RelationRepository>();
            services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
        }

        public static void InitialiseServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, DefaultUserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IWorldService, WorldService>();
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<IRelationService, RelationService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IFactionService, FactionService>();
            services.AddScoped<IEpisodeService, EpisodeService>();
            services.AddScoped<IArtifactService, ArtifactService>();
            services.AddScoped<ILoreService, LoreService>();
        }
    }
}
