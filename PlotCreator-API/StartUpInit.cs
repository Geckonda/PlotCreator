using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;
using PlotCreator_API.Auth;

namespace PlotCreator_API
{
    public static class StartUpInit
    {
        public static void InitialiseRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddScoped<IRelationRepository, RelationRepository>();
            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped<IEntityTypeRepository, EntityTypeRepository>();
            services.AddScoped<IGraphRepository, GraphRepository>();
            services.AddScoped<IGraphNodeRepository, GraphNodeRepository>();
            services.AddScoped<IGraphEdgeRepository, GraphEdgeRepository>();
        }

        public static void InitialiseServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IWorldService, WorldService>();
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<IEntityTypeService, EntityTypeService>();
            services.AddScoped<IRelationService, RelationService>();
            services.AddScoped<IGraphService, GraphService>();
        }
    }
}
