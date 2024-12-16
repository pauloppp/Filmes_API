using Filmes_API.Context;
using Filmes_API.Enums;
using Filmes_API.Models;
using Filmes_API.Repositories.Interfaces;
using Microsoft.CodeAnalysis;

namespace Filmes_API.Utils
{
    public class ErrosLog
    {
        public Task<Erro> GravarErrosProcessamento(ApiContext context, string nomeArquivo, int numeroLinhaArquivo,
            string textoLinhaArquivo, int statusErro, Filme? filme = null)
        {
            var erro = new Erro();

            switch (statusErro)
            {
                case 0:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.Arquivo_Nao_Encontrado)}";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    break;

                case 1:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.Arquivo_Vazio_Invalido)} : Dados recebidos => [{textoLinhaArquivo}]";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    break;

                case 2:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.Producer_Obrigatorio)} : Dados recebidos => [{textoLinhaArquivo}]";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    break;

                case 3:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.PreviousWin_Obrigatorio)} : Dados recebidos => [{textoLinhaArquivo}]";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    break;

                case 4:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.FollowingWin_Obrigatorio)} : Dados recebidos => [{textoLinhaArquivo}]";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    break;

                case 5:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.FollowingWin_Igual_PreviousWin)} : Dados recebidos => [{textoLinhaArquivo}]";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    break;

                case 6:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.FollowingWin_Menor_PreviousWin)} : Dados recebidos => [{textoLinhaArquivo}]";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    break;

                case 7:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.Arquivo_Processado_Com_Sucesso)}";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    erro.Filme = filme;
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    var erros = context.Erros.ToList();
                    break;

                case 8:
                    erro.Arquivo = nomeArquivo;
                    erro.Mensagem = $"{nameof(StatusErro.Arquivo_Incompleto)} : Dados recebidos => [{textoLinhaArquivo}]";
                    erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
                    context.Erros.Add(erro);
                    context.SaveChanges();
                    break;

                default:
                    break;
            }
            return Task.FromResult(erro);
        }
    }
}
