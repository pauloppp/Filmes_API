using Filmes_API.Models;

namespace Filmes_API.Repositories.Interfaces
{
    public interface IFilmeRepository
    {
        Task<Intervalo> GetIntervalos();
        Task<IEnumerable<Filme>> GetFilmes();
        Task<Filme> GetFilme(int id);
        Task<Filme> Add(Filme filme);
        Task<Filme> Update(Filme filme);
        Task<Filme> Delete(Filme filme);
    
    }
}
