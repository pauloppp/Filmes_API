using Filmes_API.Context;
using Filmes_API.Enums;
using Filmes_API.Models;
using Filmes_API.Repositories.Interfaces;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.CodeAnalysis.Elfie.Model;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Filmes_API.Utils
{
    public class CommonServices
    {
        public static Erro AdicionarDadosIniciais(ApiContext context, string nomeArquivoFilme)
        {
            var erro2 = new Erro();
            var _errosLog = new ErrosLog();

            var assembly = Assembly.GetExecutingAssembly();
            var resourcesName = new List<string>();
            var resources = new List<string>();
            var resourceName = string.Empty;
            var nomeArquivo = string.Empty;
            var textoLinha = string.Empty;
            var prd = string.Empty;
            var linha = 1;
            var min = 0;
            var max = 0;

            try
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(nomeArquivoFilme))
                    {
                        resources.Add(nomeArquivoFilme);
                        foreach (var resource in resources)
                        {
                            resourcesName.Add(resource);
                        }
                        resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(nomeArquivoFilme));
                    }
                    else
                    {
                        resources = assembly.GetManifestResourceNames().Where(arq => arq.Contains("Principal")).ToList();
                        foreach (var resource in resources)
                        {
                            resourcesName.Add(resource.Split("Principal.").Last());
                        }
                    }
                }
                catch (Exception)
                {
                    linha = 0;
                    textoLinha = string.Empty;
                    return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, textoLinha, (int)EStatusErro.Arquivo_Nao_Encontrado).Result;
                }

                foreach (var fileName in resourcesName)
                {
                    nomeArquivo = fileName;
                    resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(nomeArquivo));
                    var stream = assembly.GetManifestResourceStream(resourceName);

                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            if (!string.IsNullOrWhiteSpace(line) && values.Length > 0)
                            {
                                foreach (var item in values)
                                {
                                    var campos = item.Split(';');

                                    if (campos.Count() == 3)
                                    {
                                        if (!string.IsNullOrWhiteSpace(campos[0]))
                                        {
                                            prd = (campos[0]);
                                        }
                                        else
                                        {
                                            return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.Producer_Obrigatorio).Result;
                                        }

                                        if (!string.IsNullOrWhiteSpace(campos[1]))
                                        {
                                            min = int.Parse(campos[1]);
                                        }
                                        else
                                        {
                                            return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.PreviousWin_Obrigatorio).Result;
                                        }

                                        if (!string.IsNullOrWhiteSpace(campos[2]))
                                        {
                                            max = int.Parse(campos[2]);
                                        }
                                        else
                                        {
                                            return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.FollowingWin_Obrigatorio).Result;
                                        }

                                        if (max > min)
                                        {
                                            var filme = new Filme
                                            {
                                                Producer = campos[0],
                                                PreviousWin = int.Parse(campos[1]),
                                                FollowingWin = int.Parse(campos[2]),
                                                Interval = int.Parse(campos[2]) - int.Parse(campos[1]),
                                            };
                                            context.Filmes.Add(filme);
                                            context.SaveChanges();

                                            erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.Arquivo_Processado_Com_Sucesso, filme: filme).Result;
                                        }
                                        else
                                        {
                                            if (max == min)
                                            {
                                                return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.FollowingWin_Igual_PreviousWin).Result;
                                            }
                                            else
                                            {
                                                return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.FollowingWin_Menor_PreviousWin).Result;
                                            }
                                        }
                                        linha++;
                                    }
                                    else
                                    {
                                        return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, 0, item, (int)EStatusErro.Arquivo_Incompleto).Result;
                                    }
                                }
                            }
                            else
                            {
                                return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, 0, "", (int)EStatusErro.Arquivo_Vazio_Invalido).Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var teste = ex.Message; //...
            }
            return erro2;
        }


        public static List<Erro> AdicionarDadosIniciais_2(ApiContext context, string nomeArquivoFilme)
        {
            var _errosLog = new ErrosLog();
            var arquivosComErro = new List<string>();
            var arquivosSemErro = new List<string>();
            var errosMontar = new List<Erro>();
            var filmes = new List<Filme>();
            var erros = new List<Erro>();
            var erro2 = new Erro();

            var assembly = Assembly.GetExecutingAssembly();
            string[] values = Array.Empty<string>();
            var resourcesName = new List<string>();
            var resources = new List<string>();
            var resourceName = string.Empty;

            var nomeArquivo = string.Empty;
            var textoLinha = string.Empty;
            var statusArquivo = "1";
            var prd = string.Empty;
            var temErro = false;
            var linha = 1;
            var min = 0;
            var max = 0;

            try
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(nomeArquivoFilme))
                    {
                        resources.Add(nomeArquivoFilme);
                        foreach (var resource in resources)
                        {
                            resourcesName.Add(resource);
                        }
                        resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(nomeArquivoFilme));
                    }
                    else
                    {
                        resources = assembly.GetManifestResourceNames().Where(arq => arq.Contains("Principal")).ToList();
                        foreach (var resource in resources)
                        {
                            resourcesName.Add(resource.Split("Principal.").Last());
                        }
                    }
                }
                catch (Exception)
                {
                    linha = 0;
                    textoLinha = string.Empty;
                    erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, textoLinha, (int)EStatusErro.Arquivo_Nao_Encontrado, errosMontar).Result;
                }

                foreach (var fileName in resourcesName)
                {
                    linha = 1;
                    nomeArquivo = fileName;
                    resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(nomeArquivo));
                    var stream = assembly.GetManifestResourceStream(resourceName);

                    using (var reader = new StreamReader(stream))
                    {
                        var lin = 1;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            values = line.Split(',');

                            // Processar somente arquivos com status = 1
                            if (lin == 1 && !line.Contains(((int)EStatusProcessamentoArquivo.NaoProcessado).ToString()))
                            {
                                statusArquivo = line;
                                statusArquivo = statusArquivo.Replace(";", "");
                                arquivosComErro.Add(nomeArquivo);
                                temErro = true;
                                break;
                            }

                            // Escapar da linha 1 (cabeçalho do arquivo) e processar iniciando na linha 2.
                            if (lin > 1)
                            {
                                if (!string.IsNullOrWhiteSpace(line) && values.Length > 0)
                                {
                                    foreach (var item in values)
                                    {
                                        var campos = item.Split(';');

                                        if (campos.Count() == 3) // Os três campos(colunas) dos registros.
                                        {
                                            if (!string.IsNullOrWhiteSpace(campos[0]))
                                            {
                                                prd = (campos[0]);
                                            }
                                            else
                                            {
                                                erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.Producer_Obrigatorio, errosMontar).Result;
                                                arquivosComErro.Add(nomeArquivo);
                                                temErro = true;
                                                break;
                                            }

                                            if (!string.IsNullOrWhiteSpace(campos[1]))
                                            {
                                                min = int.Parse(campos[1]);
                                            }
                                            else
                                            {
                                                erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.PreviousWin_Obrigatorio, errosMontar).Result;
                                                arquivosComErro.Add(nomeArquivo);
                                                temErro = true;
                                                break;
                                            }

                                            if (!string.IsNullOrWhiteSpace(campos[2]))
                                            {
                                                max = int.Parse(campos[2]);
                                            }
                                            else
                                            {
                                                erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.FollowingWin_Obrigatorio, errosMontar).Result;
                                                arquivosComErro.Add(nomeArquivo);
                                                temErro = true;
                                                break;
                                            }

                                            if (max > min)
                                            {
                                                // Caso não haja nenhum erro de processamento,
                                                // o controle sai do while e grava os filmes no BD
                                                // pela função GravarFilmesBD().
                                                //temErro = false;
                                                arquivosSemErro.Clear();
                                                arquivosSemErro.Add(nomeArquivo);
                                                var teste = "";
                                            }
                                            else
                                            {
                                                if (max == min)
                                                {
                                                    erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.FollowingWin_Igual_PreviousWin, errosMontar).Result;
                                                    arquivosComErro.Add(nomeArquivo);
                                                    temErro = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.FollowingWin_Menor_PreviousWin, errosMontar).Result;
                                                    arquivosComErro.Add(nomeArquivo);
                                                    temErro = true;
                                                    break;
                                                }
                                            }
                                            //linha++;
                                        }
                                        else
                                        {
                                            erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, item, (int)EStatusErro.Arquivo_Incompleto, errosMontar).Result;
                                            arquivosComErro.Add(nomeArquivo);
                                            temErro = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, 0, "", (int)EStatusErro.Arquivo_Vazio_Invalido, errosMontar).Result;
                                    arquivosComErro.Add(nomeArquivo);
                                    temErro = true;
                                    break;
                                }
                            }
                            linha++;
                            lin++;
                        }

                        if (temErro)
                        {
                            if (statusArquivo == "3")
                            {
                                erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, "", (int)EStatusErro.Arquivo_JaProcessado_Com_Erro, errosMontar, filme: null).Result;
                                temErro = false;
                            }
                            else
                            {
                                if (statusArquivo == "2")
                                {
                                    erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, "", (int)EStatusErro.Arquivo_JaProcessado, errosMontar, filme: null).Result;
                                    temErro = false;
                                }
                                
                                // Escreve o código de status do processamento [1,2,3] no arquivo processado.
                                AtualizarStatusArquivo(resources.Where(x => x.ToString().Contains(nomeArquivo)).FirstOrDefault(), temErro);
                            }
                        }
                        else
                        {
                            if (statusArquivo == "1")
                            {
                                GravarFilmesBD(resourcesName, assembly, nomeArquivo, context, arquivosSemErro);
                                erros = _errosLog.MontarObjetoErrosProcessamento(context, nomeArquivo, linha, "", (int)EStatusErro.Arquivo_Processado_Com_Sucesso, errosMontar, filme: null).Result;

                                // Escreve o código de status do processamento [1,2,3] no arquivo processado.
                                AtualizarStatusArquivo(resources.Where(x => x.ToString().Contains(nomeArquivo)).FirstOrDefault(), temErro);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var teste = ex.Message; //...
            }

            return erros;
        }

        public static void AtualizarStatusArquivo(string arquivo, bool contemErro)
        {
            var teste = !contemErro;
            var statusAtualizado = !contemErro ?
                ((int)EStatusProcessamentoArquivo.Processado).ToString() :
                    ((int)EStatusProcessamentoArquivo.ErroProcessamento).ToString();

            arquivo = arquivo.Replace(".", "\\");
            arquivo = arquivo.Replace("\\csv", ".csv");
            arquivo = "..\\" + arquivo;

            var linhas = File.ReadAllLines(arquivo);
            linhas[0] = $"{statusAtualizado};";

            File.WriteAllLines(arquivo, linhas);
        }

        public static void GravarFilmesBD(List<string> resourcesName, Assembly assembly, string nomeArquivo, ApiContext context, List<string> arquivosSemErro)
        {
            var arquivosOK = arquivosSemErro.DistinctBy(x => x.Contains(nomeArquivo));

            foreach (var fileName in arquivosOK)
            //foreach (var fileName in resourcesName)
            {
                nomeArquivo = fileName;
                var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(nomeArquivo));
                var stream = assembly.GetManifestResourceStream(resourceName);

                using (var reader = new StreamReader(stream))
                {
                    var lin = 1;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (lin > 1)
                        {
                            foreach (var item in values)
                            {
                                var campos = item.Split(';');
                                var filme = new Filme
                                {
                                    Producer = campos[0],
                                    PreviousWin = int.Parse(campos[1]),
                                    FollowingWin = int.Parse(campos[2]),
                                    Interval = int.Parse(campos[2]) - int.Parse(campos[1]),
                                    Arquivo = nomeArquivo,
                                };
                                context.Filmes.Add(filme);
                                context.SaveChanges();
                            }
                        }
                        lin++;
                    }
                }
            }
        }

    }
}
