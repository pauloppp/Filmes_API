using Filmes_API.Context;
using Filmes_API.Models;
using Filmes_API.Test.Application;
using Filmes_API.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmes_API.Test.External
{
    public class Filmes_API_MockCSV
    {
        public static async Task<string> ReadFilmesCSV(Filmes_API_Application application, bool read, string arquivoFilme)
        {
            var retorno = new List<Erro>();
            //Erro retorno = null;
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var apiContext = provider.GetRequiredService<ApiContext>())
                {
                    if (read)
                    {
                        retorno = CommonServices.AdicionarDadosIniciais_2(apiContext, arquivoFilme);
                        var teste = "";
                        //retorno = CommonServices.AdicionarDadosIniciais(apiContext, arquivoFilme);
                    }
                    return await Task.FromResult(retorno.FirstOrDefault().Mensagem);
                }
            }
        }
    }
}
