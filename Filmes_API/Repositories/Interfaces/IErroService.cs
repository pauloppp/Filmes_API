using Filmes_API.Models;

namespace Filmes_API.Repositories.Interfaces
{
    public interface IErroService
    {
        Task<IEnumerable<Erro>> GetErros();
    }
}
