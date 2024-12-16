using Filmes_API.Context;
using Filmes_API.Enums;
using Filmes_API.Models;
using Filmes_API.Test.Application;
using Filmes_API.Test.External;
using Filmes_API.Utils;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Filmes_API.Test.Context
{
    public class Filmes_API_BD_IntegrationTests
    {
        [Test]
        public async Task GET_Retornar_IntervaloFilmes_Valido()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);
            var url = "api/v1/filmes/GetIndicados";
            var client = application.CreateClient();
            var result = await client.GetAsync(url);
            var intervalo = await client.GetFromJsonAsync<Intervalo>(url);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(intervalo.Min.Count > 0);
            Assert.IsTrue(intervalo.Max.Count > 0);
            Assert.IsNotNull(intervalo);
        }

        [Test]
        public async Task GET_Retornar_IntervaloFilmes_Invalido()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);
            var url = "api/v1/filmes/GetIndicadoss";
            var client = application.CreateClient();
            var result = await client.GetAsync(url);
            Intervalo? intervalo = null;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNull(intervalo);
        }

        [Test]
        public async Task GET_Retornar_Filmes_Todos_NaoNulo()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);
            var url = "api/v1/filmes";
            var client = application.CreateClient();
            var result = await client.GetAsync(url);
            var filmes = await client.GetFromJsonAsync<List<Filme>>(url);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(filmes.Count > 0);
            Assert.IsNotNull(filmes);
        }

        [Test]
        public async Task GET_Retornar_Filmes_Todos_Nulo()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);
            var url = "api/v1/filmess";
            var client = application.CreateClient();
            var result = await client.GetAsync(url);
            List<Filme> filmes = null;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsNull(filmes);
        }

        [Test]
        public async Task GET_Retornar_Filme_PorId_NaoNulo()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);
            var url = "api/v1/filmes/1";
            var client = application.CreateClient();
            var result = await client.GetAsync(url);
            var filme = await client.GetFromJsonAsync<Filme>(url);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(filme.PreviousWin != 0);
            Assert.IsNotNull(filme);
        }

        [Test]
        public async Task GET_Retornar_Filme_PorId_Nulo()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);
            var url = "api/v1/filmes/9999999";
            var client = application.CreateClient();
            var result = await client.GetAsync(url);
            Filme? filme = null;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsNull(filme);
        }

        [Test]
        public async Task PUT_Atualizar_Filme_PorId_Valido()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var id = 1;
            var url = $"api/v1/filmes/UpdateFilme/{id}";

            var filme = new Filme();
            filme.Producer = "Producer99";
            filme.PreviousWin = 2007;
            filme.FollowingWin = 2008;
            filme.Interval = 1;
            filme.Id = 1;

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PutAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public async Task PUT_Atualizar_Filme_PorId_Invalido()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var id = 9999;
            var url = $"api/v1/filmes/UpdateFilme/{id}";

            var filme = new Filme();
            filme.Producer = "Producer99";
            filme.PreviousWin = 2007;
            filme.FollowingWin = 2008;
            filme.Interval = 1;
            filme.Id = 1;

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PutAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();
            var msg = JsonConvert.DeserializeObject<string>(responseBody.ToString());
            msg = msg.Replace("\"", "");

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsTrue(responseBody.Contains("Id informado inválido"));
        }

        [Test]
        public async Task PUT_Atualizar_Filme_PorId_FilmeNull()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var id = 1;
            var url = $"api/v1/filmes/UpdateFilme/{id}";

            var filme = new Filme();
        
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PutAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task PUT_Atualizar_Filme_PorId_FollowingWin_Igual_PreviousWin()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var id = 1;
            var url = $"api/v1/filmes/UpdateFilme/{id}";

            var filme = new Filme();
            filme.Producer = "Producer99";
            filme.PreviousWin = 2007;
            filme.FollowingWin = 2007;
            filme.Interval = 1;
            filme.Id = 1;

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PutAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();
            var msg = JsonConvert.DeserializeObject<string>(responseBody.ToString());
            msg = msg.Replace("\"", "");

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsTrue(responseBody.Contains("FollowingWin deve ser maior que PreviousWin. Igual não permitido"));
        }

        [Test]
        public async Task PUT_Atualizar_Filme_PorId_FollowingWin_Menor_PreviousWin()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var id = 1;
            var url = $"api/v1/filmes/UpdateFilme/{id}";

            var filme = new Filme();
            filme.Producer = "Producer99";
            filme.PreviousWin = 2007;
            filme.FollowingWin = 2006;
            filme.Interval = 1;
            filme.Id = 1;

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PutAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();
            var msg = JsonConvert.DeserializeObject<string>(responseBody.ToString());
            msg = msg.Replace("\"", "");

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsTrue(responseBody.Contains("FollowingWin deve ser maior que PreviousWin"));
        }

        [Test]
        public async Task POST_Cadastrar_Filme_Valido()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);
           
            var url = $"api/v1/filmes/";

            var filme = new Filme();
            filme.Producer = "Producer99";
            filme.PreviousWin = 2007;
            filme.FollowingWin = 2008;
            filme.Interval = 1;
            
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PostAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();
            var filmeCadastrado = JsonConvert.DeserializeObject<Filme>(responseBody);

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.IsTrue(filmeCadastrado.Id > 0);
            Assert.IsNotNull(filmeCadastrado);
        }

        [Test]
        public async Task POST_Cadastrar_Filme_Invalido()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var url = $"api/v1/filmes/";

            var filme = new Filme();
            filme.Producer = "Producer99";
            filme.PreviousWin = 2007;
            filme.FollowingWin = 2008;
            filme.Interval = 1;

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PostAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();
            var filmeCadastrado = JsonConvert.DeserializeObject<Filme>(responseBody);

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.IsTrue(filmeCadastrado.Id > 0);
            Assert.IsNotNull(filmeCadastrado);
        }

        [Test]
        public async Task POST_Cadastrar_Filme_FollowingWin_Igual_PreviousWin()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var url = $"api/v1/filmes/";

            var filme = new Filme();
            filme.Producer = "Producer99";
            filme.PreviousWin = 2007;
            filme.FollowingWin = 2007;
            filme.Interval = 1;

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PostAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();

            var msg = JsonConvert.DeserializeObject<string>(responseBody.ToString());
            msg = msg.Replace("\"", "");

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsTrue(responseBody.Contains("FollowingWin deve ser maior que PreviousWin. Igual não permitido"));
        }

        [Test]
        public async Task POST_Cadastrar_Filme_FollowingWin_Menor_PreviousWin()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var url = $"api/v1/filmes/";

            var filme = new Filme();
            filme.Producer = "Producer99";
            filme.PreviousWin = 2007;
            filme.FollowingWin = 2006;
            filme.Interval = 1;

            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.PostAsync(url, content).Result);
            string responseBody = await result.Content.ReadAsStringAsync();
         
            var msg = JsonConvert.DeserializeObject<string>(responseBody.ToString());
            msg = msg.Replace("\"", "");

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsTrue(responseBody.Contains("FollowingWin deve ser maior que PreviousWin"));
        }

        [Test]
        public async Task DELETE_Excluir_Filme_PorId_Valido()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var id = 1;
            var url = $"api/v1/filmes/{id}";
            
            //var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(filme, Newtonsoft.Json.Formatting.Indented);
            //HttpContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var client = application.CreateClient();

            var result = await Task.FromResult<HttpResponseMessage>(client.DeleteAsync(url).Result);
            string responseBody = await result.Content.ReadAsStringAsync();
            //var msg = JsonConvert.DeserializeObject<string>(responseBody.ToString());
            //msg = msg.Replace("\"", "");

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            //Assert.IsTrue(responseBody.Contains("FollowingWin deve ser maior que PreviousWin"));
        }

        [Test]
        public async Task DELETE_Excluir_Filme_PorId_Invalido()
        {
            await using var application = new Filmes_API_Application();
            await Filmes_API_MockBD.CreateFilmes(application, true);

            var id = 99;
            var url = $"api/v1/filmes/{id}";
                        
            var client = application.CreateClient();
            var result = await Task.FromResult<HttpResponseMessage>(client.DeleteAsync(url).Result);
            string responseBody = await result.Content.ReadAsStringAsync();
            var msg = JsonConvert.DeserializeObject<string>(responseBody.ToString());
            msg = msg.Replace("\"", "");

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(responseBody.Contains("Id/Filme inexistente"));
        }
    }
}