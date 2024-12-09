using Swashbuckle.AspNetCore.Annotations;

namespace Filmes_API.Models
{
    public class Intervalo
    {
        [SwaggerSchema(Description = "Filmes com menor intervalo entre prêmios")]
        public List<Filme>? Min { get; set; }

        [SwaggerSchema(Description = "Filmes com maior intervalo entre prêmios")]
        public List<Filme>? Max { get; set; }
    }
}
