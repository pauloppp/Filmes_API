using Filmes_API.Models;
using Microsoft.Extensions.Hosting.Internal;
using System.Net;
using System.Net.Http.Json;

namespace Filmes_API.Test
{
    public class Filmes_API_IntegrationTests
    {
        [Test]
        public async Task GET_Retornar_Intervalo_Filmes()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockData.CreateFilmes(application, true);
            var url = "api/v1/filmes/GetIndicados";
            var client = application.CreateClient();
            var result = await client.GetAsync(url);
            var intervalo = await client.GetFromJsonAsync<Intervalo>(url);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(intervalo.Min.Count > 0);
            Assert.IsTrue(intervalo.Max.Count > 0);
            Assert.IsNotNull(intervalo);
        }
    }
}