using Filmes_API.Context;
using Filmes_API.Models;
using Filmes_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Filmes_API.Repositories.Concretes
{
    public class FilmeRepository : IFilmeRepository
    {
        private readonly ApiContext _context;

        public FilmeRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task<Filme> Add(Filme filme)
        {
            filme.Interval = filme.FollowingWin - filme.PreviousWin;
            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();
            return filme;
        }

        public async Task<Filme> Delete(Filme filme)
        {
            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();
            return filme;
        }

        public async Task<Filme> GetFilme(int id)
        {
            return await _context.Filmes.FindAsync(id);
        }

        public async Task<IEnumerable<Filme>> GetFilmes()
        {
            return await _context.Filmes.ToListAsync();
        }

        public async Task<Intervalo> GetIntervalos()
        {
            var intervals = new Intervalo();

            var min = _context.Filmes.Min(x => x.Interval);
            var max = _context.Filmes.Max(x => x.Interval);

            var exists = (min > 0 && max > 0);
            if (exists)
            {
                var mins = await _context.Filmes.Where(x => x.Interval == min).OrderBy(x => x.Id).ToListAsync();
                var maxs = await _context.Filmes.Where(x => x.Interval == max).OrderBy(x => x.Id).ToListAsync();
                intervals.Min = mins;
                intervals.Max = maxs;
            }
            return intervals;
        }
        
        public async Task<Filme> Update(Filme filme)
        {
            filme.Interval = filme.FollowingWin - filme.PreviousWin;
            _context.Entry(filme).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return filme;
        }
    }
}
