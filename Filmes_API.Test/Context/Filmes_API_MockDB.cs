﻿using Filmes_API.Context;
using Filmes_API.Test.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmes_API.Test.Context
{
    public class Filmes_API_MockDB
    {
        public static async Task CreateFilmes(Filmes_API_Application application, bool create)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var apiContext = provider.GetRequiredService<ApiContext>())
                {
                    await apiContext.Database.EnsureCreatedAsync();

                    if (create)
                    {
                        //await apiContext.Filmes.AddAsync(new Models.Filme
                        //{ Producer = "Producer1_t", PreviousWin = 2008, FollowingWin = 2009, Interval = 1 });
                        //await apiContext.Filmes.AddAsync(new Models.Filme
                        //{ Producer = "Producer2_t", PreviousWin = 2018, FollowingWin = 2019, Interval = 1 });
                        //await apiContext.Filmes.AddAsync(new Models.Filme
                        //{ Producer = "Producer1_t", PreviousWin = 1900, FollowingWin = 1999, Interval = 1 });
                        //await apiContext.Filmes.AddAsync(new Models.Filme
                        //{ Producer = "Producer2_t", PreviousWin = 2000, FollowingWin = 2099, Interval = 1 });
                    }
                    await apiContext.SaveChangesAsync();
                }
            }
        }
    }
}
