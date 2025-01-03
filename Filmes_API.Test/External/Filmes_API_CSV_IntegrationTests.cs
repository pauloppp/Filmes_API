using Filmes_API.Enums;
using Filmes_API.Test.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmes_API.Test.External
{
    public class Filmes_API_CSV_IntegrationTests
    {
        [Test]
        public async Task Retornar_ArquivoCSV_NaoEncontrado()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filme.csv";
            var msgResultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            msgResultado = msgResultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.Arquivo_Nao_Encontrado);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ArquivoCSV_ProcessadoComSucesso()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_Arquivo_Processado_Com_Successo.csv";
            var msgResultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            msgResultado = msgResultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.Arquivo_Processado_Com_Sucesso);


            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ErroProcessamento_ProducerObrigatorio()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_SemProducer_Linha1.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.Producer_Obrigatorio);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ErroProcessamento_PreviousWinObrigatorio()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_SemPreviousWin_Linha1.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.PreviousWin_Obrigatorio);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ErroProcessamento_FollowingWinWinObrigatorio()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_SemFollowingWin_Linha1.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.FollowingWin_Obrigatorio);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ErroProcessamento_FollowingWinIgualPreviousWin()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_FollowingWin_Igual_PreviousWin_Linha1.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.FollowingWin_Igual_PreviousWin);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ErroProcessamento_FollowingWinMenorPreviousWin()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_FollowingWin_Menor_PreviousWin_Linha1.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.FollowingWin_Menor_PreviousWin);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ArquivoCSV_VazioInvalido()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_Arquivo_Vazio_Invalido.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.Arquivo_Vazio_Invalido);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ArquivoCSV_Incompleto()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_Arquivo_Incompleto.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.Arquivo_Incompleto);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ArquivoCSV_JaProcessado()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_Arquivo_Processado_Com_Status_Igual_DOIS.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.Arquivo_JaProcessado);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }

        [Test]
        public async Task Retornar_ArquivoCSV_JaProcessado_Com_Erro()
        {
            await using var application = new Filmes_API_Application();
            var arqFilmes = "Filmes_Arquivo_Processado_Com_Status_Igual_TRES.csv";
            var resultado = await Filmes_API_MockCSV.ReadFilmesCSV(application, true, arqFilmes);
            var msgResultado = resultado.Split(":").First().Trim();
            var msgEsperada = nameof(EStatusErro.Arquivo_JaProcessado_Com_Erro);

            Assert.IsTrue(msgEsperada is not null);
            Assert.AreEqual(msgEsperada, msgResultado);
        }
    }
}
