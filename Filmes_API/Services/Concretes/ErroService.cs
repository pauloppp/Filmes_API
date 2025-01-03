using Filmes_API.Context;
using Filmes_API.Enums;
using Filmes_API.Models;
using Filmes_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Filmes_API.Services.Concretes
{
    public class ErroService : IErroService
    {
        private readonly ApiContext _context;

        public ErroService(ApiContext context)
        {
            _context = context;
        }

        public async Task<Erro> Create(Erro erro)
        {
            _context.Erros.Add(erro);
            await _context.SaveChangesAsync();
            return erro;
        }

        public async Task<IEnumerable<Erro>> GetErros()
        {
            var erros = new List<Erro>();
            var listaErros = await _context.Erros.OrderBy(x => x.DataCriacao).Include(x => x.Filme).ToListAsync();

            var arquivoAtual = string.Empty;
            var arquivoProximo = string.Empty;

            foreach (var erro in listaErros)
            {
                arquivoAtual = erro.Arquivo;

                if (arquivoProximo != arquivoAtual)
                {
                    erros.Add(erro);
                    arquivoProximo = erro.Arquivo;
                }
            }
            return erros;
        }

        public async Task<IEnumerable<Erro>> GetErros_2()
        {
            var erros = new List<Erro>();

            //var listaErros = await _context.Erros.OrderBy(x => x.DataCriacao).Include(x => x.Filme).ToListAsync();
            //var listaErros = await _context.Erros.OrderBy(x => x.Arquivo).ToListAsync();

            var listaErros = await _context.Erros.OrderBy(x => x.Arquivo)
                     .Where(x => x.Mensagem.Substring(0, 30) != nameof(EStatusErro.Arquivo_Processado_Com_Sucesso)).ToListAsync();

            var erroAtual = string.Empty;
            var erroProximo = string.Empty;

            foreach (var erro in listaErros)
            {
                erroAtual = erro.Mensagem;

                if (erroProximo != erroAtual)
                {
                    erros.Add(erro);
                    erroProximo = erro.Mensagem;
                }
            }
            return erros;
        }
    }
}
