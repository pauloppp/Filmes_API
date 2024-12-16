using Filmes_API.Context;
using Filmes_API.Enums;
using Filmes_API.Models;
using Filmes_API.Repositories.Interfaces;
using Humanizer.Localisation;
using System.ComponentModel;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

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
                    return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, textoLinha, (int)StatusErro.Arquivo_Nao_Encontrado).Result;
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
                                            return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)StatusErro.Producer_Obrigatorio).Result;
                                        }

                                        if (!string.IsNullOrWhiteSpace(campos[1]))
                                        {
                                            min = int.Parse(campos[1]);
                                        }
                                        else
                                        {
                                            return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)StatusErro.PreviousWin_Obrigatorio).Result;
                                        }

                                        if (!string.IsNullOrWhiteSpace(campos[2]))
                                        {
                                            max = int.Parse(campos[2]);
                                        }
                                        else
                                        {
                                            return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)StatusErro.FollowingWin_Obrigatorio).Result;
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

                                            erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)StatusErro.Arquivo_Processado_Com_Sucesso, filme: filme).Result;
                                        }
                                        else
                                        {
                                            if (max == min)
                                            {
                                                return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)StatusErro.FollowingWin_Igual_PreviousWin).Result;
                                            }
                                            else
                                            {
                                                return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, linha, item, (int)StatusErro.FollowingWin_Menor_PreviousWin).Result;
                                            }
                                        }
                                        linha++;
                                    }
                                    else
                                    {
                                        return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, 0, item, (int)StatusErro.Arquivo_Incompleto).Result;
                                    }
                                }
                            }
                            else
                            {
                                return erro2 = _errosLog.GravarErrosProcessamento(context, nomeArquivo, 0, "", (int)StatusErro.Arquivo_Vazio_Invalido).Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var teste = ex.Message;//...
            }
            return erro2;
        }
    }
}
