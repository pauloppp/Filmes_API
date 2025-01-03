using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Filmes_API.Context;
using Filmes_API.Models;
using Swashbuckle.AspNetCore.Annotations;
using NuGet.Protocol.Core.Types;
using Filmes_API.Services.Interfaces;

namespace Filmes_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ErrosController : ControllerBase
    {
        private readonly IErroService _service;

        public ErrosController(IErroService service)
        {
            _service = service;
        }

        // GET: api/v1/Erros/GetErros
        //[HttpGet]
        //[Route("GetErros")]
        //[ProducesResponseType(typeof(Erro), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Erro), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(Erro), StatusCodes.Status500InternalServerError)]
        //[SwaggerOperation(Summary = "Obter lista de erros no processamento do arquivo [.csv]")]
        //public async Task<ActionResult<IEnumerable<Erro>>> GetErros()
        //{
        //    var errosProcessados = _service.GetErros();
        //    if (errosProcessados is null)
        //    {
        //        return NotFound("Lista de erros no processamento inexistente");
        //    }
        //    return Ok(errosProcessados.Result);
        //}


        // GET: api/v1/Erros/GetErros
        [HttpGet]
        [Route("GetErros")]
        [ProducesResponseType(typeof(Erro), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Erro), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Erro), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obter lista de erros no processamento do arquivo [.csv]")]
        public async Task<ActionResult<List<Erro>>> GetErros()
        {
            var errosProcessados = _service.GetErros_2();
            if (errosProcessados is null)
            {
                return NotFound("Lista de erros no processamento inexistente");
            }
            return Ok(errosProcessados.Result);
        }

    }
}
