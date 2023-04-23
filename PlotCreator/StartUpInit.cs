﻿using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Service.Implementations;
using PlotCreator.Service.Interfaces;

namespace PlotCreator
{
    public static class StartUpInit
    {
        public static void InitialiseRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<Idea>, IdeaRepository>();
        }

        public static void InitialiseServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IIdeaService, IdeaService>();
        }
    }
}
