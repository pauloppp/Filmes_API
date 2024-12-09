using Filmes_API.Context;
using Filmes_API.Models;
using Filmes_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Filmes_API.Repositories.Concretes
{
    public class ErroService : IErroService
    {
        private readonly ApiContext _context;

        public ErroService(ApiContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Erro>> GetErros()
        {
            return await _context.Erros.OrderBy(x => x.Origem).ToListAsync();         
        }
    }
}
