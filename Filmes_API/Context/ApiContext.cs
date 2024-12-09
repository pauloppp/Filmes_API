using Filmes_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Filmes_API.Context
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Erro> Erros { get; set; }
    }
}
