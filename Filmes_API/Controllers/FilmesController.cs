using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Filmes_API.Context;
using Filmes_API.Models;
using Filmes_API.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;

namespace Filmes_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class FilmesController : ControllerBase
    {
        private readonly IFilmeRepository _repository;
        public FilmesController(IFilmeRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Filmes/GetIndicados
        [HttpGet]
        [Route("GetIndicados")]
        [ProducesResponseType(typeof(Intervalo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Intervalo), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Intervalo), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obter lista de indicados e vencedores da categoria \"Pior Filme\"")]
        public async Task<ActionResult<Intervalo>> GetIntervalo()
        {
            var filmesIntervalo = _repository.GetIntervalos();
            if (filmesIntervalo is null)
            {
                return NotFound("Lista vencedores da categoria 'Pior Filme' inexistente");
            }
            return Ok(filmesIntervalo.Result);
        }

        // GET: api/Filmes
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Filme>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Filme>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Intervalo), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obter lista de filmes cadastrados")]
        public async Task<ActionResult<IEnumerable<Filme>>> GetFilmes()
        {
            var filmes = await _repository.GetFilmes();
            if (filmes is null)
            {
                return NotFound("Filmes inexistentes");
            }

            return Ok(await _repository.GetFilmes());
        }

        // GET: api/Filmes/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Intervalo), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obter um filme pelo \"id\" cadastrado")]
        public async Task<ActionResult<Filme>> GetFilme(int id)
        {
            var filme = await _repository.GetFilme(id);
            if (filme == null)
            {
                return NotFound("Filme inexistente");
            }

            return filme;
        }

        // PUT: api/Filmes/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Intervalo), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Atualizar um filme pelo \"id\" cadastrado")]
        public async Task<IActionResult> UpdateFilme(int id, Filme filme)
        {
            if (id != filme.Id)
            {
                return BadRequest("Id informado inválido");
            }

            if (filme is null)
            {
                return BadRequest("Filme inválido");
            }

            try
            {
                await _repository.Update(filme);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmeExists(id))
                {
                    return NotFound("Filme Inexistente");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Filmes
        [HttpPost]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Intervalo), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Cadastrar e/ou incluir um filme")]
        public async Task<ActionResult<Filme>> CreateFilme(Filme filme)
        {
            if (filme is null)
            {
                return BadRequest("Filme inválido");
            }

            await _repository.Add(filme);

            return CreatedAtAction("GetFilme", new { id = filme.Id }, filme);
        }

        // DELETE: api/Filmes/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Filme), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Intervalo), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Excluir um filme pelo \"id\" cadastrado")]
        public async Task<IActionResult> DeleteFilme(int id)
        {
            var filme = await _repository.GetFilme(id);
            if (filme == null)
            {
                return NotFound("Id/Filme inexistente");
            }

            await _repository.Delete(filme);

            return NoContent();
        }

        private bool FilmeExists(int id)
        {
            return _repository.GetFilme(id) != null;
        }

        // Anotações gerais para ResponsesType...
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
    }
}
