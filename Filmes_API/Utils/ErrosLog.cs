using Filmes_API.Context;
using Filmes_API.Enums;
using Filmes_API.Models;
using Filmes_API.Repositories.Interfaces;
using Microsoft.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Filmes_API.Utils
{
    public class ErrosLog
    {
        public Task<Erro> GravarErrosProcessamento(ApiContext context, string nomeArquivo, int numeroLinhaArquivo,
            string textoLinhaArquivo, int statusErro, Filme? filme = null)
        {
            //var erro = new Erro();

            //switch (statusErro)
            //{
            //    case 0:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.Arquivo_Nao_Encontrado)}";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        break;

            //    case 1:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.Arquivo_Vazio_Invalido)} : Dados recebidos => [{textoLinhaArquivo}]";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        break;

            //    case 2:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.Producer_Obrigatorio)} : Dados recebidos => [{textoLinhaArquivo}]";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        break;

            //    case 3:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.PreviousWin_Obrigatorio)} : Dados recebidos => [{textoLinhaArquivo}]";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        break;

            //    case 4:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.FollowingWin_Obrigatorio)} : Dados recebidos => [{textoLinhaArquivo}]";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        break;

            //    case 5:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.FollowingWin_Igual_PreviousWin)} : Dados recebidos => [{textoLinhaArquivo}]";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        break;

            //    case 6:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.FollowingWin_Menor_PreviousWin)} : Dados recebidos => [{textoLinhaArquivo}]";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        break;

            //    case 7:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.Arquivo_Processado_Com_Sucesso)}";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        erro.Filme = filme;
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        var erros = context.Erros.ToList();
            //        break;

            //    case 8:
            //        erro.Arquivo = nomeArquivo;
            //        erro.Mensagem = $"{nameof(EStatusErro.Arquivo_Incompleto)} : Dados recebidos => [{textoLinhaArquivo}]";
            //        erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            //        context.Erros.Add(erro);
            //        context.SaveChanges();
            //        break;

            //    default:
            //        break;
            //}
            //return Task.FromResult(erro);

            //---------
            var erro = new Erro();
            erro.Arquivo = nomeArquivo;
            erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            erro.Filme = filme ?? null;

            var finalMensagem = $" : Dados recebidos => [{textoLinhaArquivo}]";

            switch (statusErro)
            {
                case 0:
                    erro.Mensagem = $"{nameof(EStatusErro.Arquivo_Nao_Encontrado)}";
                    break;

                case 1:
                    erro.Mensagem = $"{nameof(EStatusErro.Arquivo_Vazio_Invalido)}{finalMensagem}";
                    break;

                case 2:
                    erro.Mensagem = $"{nameof(EStatusErro.Producer_Obrigatorio)}{finalMensagem}";
                    break;

                case 3:
                    erro.Mensagem = $"{nameof(EStatusErro.PreviousWin_Obrigatorio)}{finalMensagem}";
                    break;

                case 4:
                    erro.Mensagem = $"{nameof(EStatusErro.FollowingWin_Obrigatorio)}{finalMensagem}";
                    break;

                case 5:
                    erro.Mensagem = $"{nameof(EStatusErro.FollowingWin_Igual_PreviousWin)}{finalMensagem}";
                    break;

                case 6:
                    erro.Mensagem = $"{nameof(EStatusErro.FollowingWin_Menor_PreviousWin)}{finalMensagem}";
                    break;

                case 7:
                    erro.Mensagem = $"{nameof(EStatusErro.Arquivo_Processado_Com_Sucesso)}";
                    break;

                case 8:
                    erro.Mensagem = $"{nameof(EStatusErro.Arquivo_Incompleto)}{finalMensagem}";
                    break;

                default:
                    break;
            }

            GravarErrosBD(context, erro);
            return Task.FromResult(erro);
            //---------
        }

        public Task<Erro> GravarErrosBD(ApiContext context, Erro erro)
        {
            context.Erros.Add(erro);
            context.SaveChanges();

            return Task.FromResult(erro);
        }


        public Task<List<Erro>> MontarObjetoErrosProcessamento(ApiContext context, string nomeArquivo, int numeroLinhaArquivo,
            string textoLinhaArquivo, int statusErro, List<Erro> listaErros, Filme? filme = null)
        {

            var finalMensagem = $" : Dados recebidos => [{textoLinhaArquivo}]";

            var erro = new Erro();
            erro.Arquivo = nomeArquivo;
            erro.Origem = $"Linha arquivo: [{numeroLinhaArquivo}]";
            erro.Filme = filme ?? null;
            erro.Mensagem = $"{GetDescricaoStatusErro(statusErro)}{finalMensagem}";

            GravarErrosBD(context, erro);

            // Verificação teste
            var errosTodos = context.Erros.ToList();

            listaErros.Add(erro);
            return Task.FromResult(listaErros);
        }

        public string GetDescricaoStatusErro(int statusErro)
        {
            var descricao = string.Empty;

            switch (statusErro)
            {
                case 0:
                    descricao = $"{nameof(EStatusErro.Arquivo_Nao_Encontrado)}";
                    break;

                case 1:
                    descricao = $"{nameof(EStatusErro.Arquivo_Vazio_Invalido)}";
                    break;

                case 2:
                    descricao = $"{nameof(EStatusErro.Producer_Obrigatorio)}";
                    break;

                case 3:
                    descricao = $"{nameof(EStatusErro.PreviousWin_Obrigatorio)}";
                    break;

                case 4:
                    descricao = $"{nameof(EStatusErro.FollowingWin_Obrigatorio)}";
                    break;

                case 5:
                    descricao = $"{nameof(EStatusErro.FollowingWin_Igual_PreviousWin)}";
                    break;

                case 6:
                    descricao = $"{nameof(EStatusErro.FollowingWin_Menor_PreviousWin)}";
                    break;

                case 7:
                    descricao = $"{nameof(EStatusErro.Arquivo_Processado_Com_Sucesso)}";
                    break;

                case 8:
                    descricao = $"{nameof(EStatusErro.Arquivo_Incompleto)}";
                    break;

                case 9:
                    descricao = $"{nameof(EStatusErro.Arquivo_JaProcessado)}";
                    break;

                case 10:
                    descricao = $"{nameof(EStatusErro.Arquivo_JaProcessado_Com_Erro)}";
                    break;

                default:
                    break;
            }
            return descricao;
        }


    }
}
