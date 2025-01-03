using Filmes_API.Models;

namespace Filmes_API.Services.Interfaces
{
    public interface IErroService
    {
        Task<IEnumerable<Erro>> GetErros();
        Task<Erro> Create(Erro erro);

        Task<IEnumerable<Erro>> GetErros_2();
    }
}
