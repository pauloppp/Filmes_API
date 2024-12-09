using Filmes_API.Context;
using Filmes_API.Models;
using Humanizer.Localisation;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Filmes_API.Utils
{
    public static class CommonServices
    {
        public static async void AdicionarDadosIniciais(ApiContext context)
        {
            //var arquivo = "Filme.csv";
            var arquivo = "Filmes.csv";
            var resourceName = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();

            try
            {
                try
                {
                    resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(arquivo));
                }
                catch (Exception)
                {
                    var erro = new Erro
                    {
                        Mensagem = $"Arquivo: [{arquivo}] não encontrado",
                        Origem = $"Linha arquivo: [{0}]"
                    };
                    context.Erros.Add(erro);
                    context.SaveChanges();
                }
                
                var stream = assembly.GetManifestResourceStream(resourceName);

                var prd = string.Empty;
                var min = 0;
                var max = 0;
                var i = 1;

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

                                if (!string.IsNullOrWhiteSpace(campos[0]))
                                {
                                    prd = (campos[0]);
                                }
                                else
                                {
                                    var erro = new Erro
                                    {
                                        Mensagem = $"Producer é obrigatório",
                                        Origem = $"Linha arquivo: [{i}]"
                                    };
                                    context.Erros.Add(erro);
                                    context.SaveChanges();
                                }

                                if (!string.IsNullOrWhiteSpace(campos[1]))
                                {
                                    min = int.Parse(campos[1]);
                                }
                                else
                                {
                                    var erro = new Erro
                                    {
                                        Mensagem = $"PreviousWin é obrigatório",
                                        Origem = $"Linha arquivo: [{i}]"
                                    };
                                    context.Erros.Add(erro);
                                    context.SaveChanges();
                                }

                                if (!string.IsNullOrWhiteSpace(campos[2]))
                                {
                                    max = int.Parse(campos[2]);
                                }
                                else
                                {
                                    var erro = new Erro
                                    {
                                        Mensagem = $"FollowingWin é obrigatório",
                                        Origem = $"Linha arquivo: [{i}]"
                                    };
                                    context.Erros.Add(erro);
                                    context.SaveChanges();
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
                                }
                                else
                                {
                                    if (max == min)
                                    {
                                        var erro = new Erro
                                        {
                                            Mensagem = $"FollowingWin:[{max}] deve ser maior que PreviousWin:[{min}]. Igual não permitido",
                                            Origem = $"Linha arquivo: [{i}]"
                                        };
                                        context.Erros.Add(erro);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        var erro = new Erro
                                        {
                                            Mensagem = $"FollowingWin:[{max}] menor que PreviousWin:[{min}]",
                                            Origem = $"Linha arquivo: [{i}]"
                                        };
                                        context.Erros.Add(erro);
                                        context.SaveChanges();
                                    }
                                }
                                i++;
                            }
                        }
                        else
                        {
                            var erro = new Erro
                            {
                                Mensagem = "Arquivo vazio e/ou inválido",
                                Origem = $"Linha arquivo: [{0}]"
                            };
                            context.Erros.Add(erro);
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
    }
}
