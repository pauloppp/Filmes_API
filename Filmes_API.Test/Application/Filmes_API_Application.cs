using Filmes_API.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmes_API.Test.Application
{
    public class Filmes_API_Application : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApiContext>));
                services.AddDbContext<ApiContext>(options =>
                options.UseInMemoryDatabase("Oscar", root));
            });
            return base.CreateHost(builder);
        }
    }
}
