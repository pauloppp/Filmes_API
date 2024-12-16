using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Filmes_API.Models
{
    public class Base
    {
        //[JsonIgnore]
        public int Id { get; set; }
    }
}
