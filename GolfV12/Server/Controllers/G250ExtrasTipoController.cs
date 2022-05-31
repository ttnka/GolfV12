using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G250ExtrasTipoController : ControllerBase 
    {
        private readonly IG250ExtrasTipo _extraTIFace;

        public G250ExtrasTipoController(IG250ExtrasTipo extraTIFace)
        {
            this._extraTIFace = extraTIFace;
        }
        /*
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G250ExtrasTipo>>> Buscar(string? titulo, 
            string? creador, string? grupo, bool publico)
        {
            try
            {
                var resultado = await _extraTIFace.Buscar(titulo, creador, grupo, publico);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando tipos de tiros extras");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetExtrasTipos()
        {
            try
            {
                return Ok(await _extraTIFace.GetExtrasTipos());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de ExtrasTipos");
            }
        }

        [HttpGet("{extrastipoid:int}")]
        public async Task<ActionResult<G250ExtrasTipo>> GetExtrasTipo(int extrasTipoId)
        {
            try
            {
                var resultado = await _extraTIFace.GetExtrasTipo(extrasTipoId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un tipo extra de tiro");
            }
        }
        */

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G250ExtrasTipo>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _extraTIFace.Filtro(clave);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando tipos de tiros extras");
            }
        }

        [HttpPost]
        public async Task<ActionResult<G250ExtrasTipo>> AddExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            try
            {
                if (extrasTipo == null) return BadRequest();
                return await _extraTIFace.AddExtrasTipo(extrasTipo);
                /*
                var newExtrasTipo = await _extraTIFace.AddExtrasTipo(extrasTipo);
                return CreatedAtAction(nameof(GetExtrasTipo), new { extrasTipoId = extrasTipo.Id }, newExtrasTipo);
                */
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear un nuevo registro en los tipos de extras de tiros en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G250ExtrasTipo>> UpdateExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            try
            {
                return extrasTipo != null ? await _extraTIFace.UpdateExtrasTipo(extrasTipo) :
                    NotFound($"El registro {extrasTipo.Id} titulado {extrasTipo.Titulo} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, la tipos de tiros extras");
            }
        }
    }
}
