using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Filmes_API.Models
{
    public class Filme : Base
    {
        [Required]
        [SwaggerSchema(Description = "Produtor do filme")]
        public string? Producer { get; set; }

        [Required]
        [SwaggerSchema(Description = "Ano prêmio anterior")]
        public int PreviousWin { get; set; }

        [Required]
        [SwaggerSchema(Description = "Ano prêmio posterior")]
        public int FollowingWin { get; set; }

        [Required]
        [SwaggerSchema(Description = "Intervalo entre prêmios")]
        public int Interval { get; set; }
    }
}
