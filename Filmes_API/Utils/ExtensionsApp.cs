﻿using Filmes_API.Repositories.Concretes;
using Filmes_API.Repositories.Interfaces;
using Filmes_API.Services.Concretes;
using Filmes_API.Services.Interfaces;

namespace Filmes_API.Utils
{
    public static class ExtensionsApp
    {
        public static IServiceCollection ResolverDependencias(this IServiceCollection services)
        {
            services.AddScoped<IFilmeRepository, FilmeRepository>();
            services.AddScoped<IErroService, ErroService>();
            return services;
        }
    }
}
