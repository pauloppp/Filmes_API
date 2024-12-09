using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Filmes_API.Models
{
    public class Erro : Base
    {
        [Required]
        [SwaggerSchema(Description = "Data criação/verificação do erro")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Required]
        [SwaggerSchema(Description = "Descrição do erro")]
        public string? Mensagem { get; set; }

        [Required]
        [SwaggerSchema(Description = "Origem do erro")]
        public string? Origem { get; set; }
    }
}
