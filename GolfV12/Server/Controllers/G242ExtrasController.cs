using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G242ExtrasController : ControllerBase 
    {
        private readonly IG242Extras _extraIFace;

        public G242ExtrasController(IG242Extras extraIFace)
        {
            this._extraIFace = extraIFace;
        }
        

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G242Extras>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _extraIFace.Filtro(clave);
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
        public async Task<ActionResult<G242Extras>> AddExtra(G242Extras extras)
        {
            try
            {
                if (extras == null) return BadRequest();
                return await _extraIFace.AddExtra(extras);
                /*
                var newExtrasTipo = await _extraTIFace.AddExtrasTipo(extrasTipo);
                return CreatedAtAction(nameof(GetExtrasTipo), new { extrasTipoId = extrasTipo.Id }, newExtrasTipo);
                */
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear un nuevo registro en los extras de tiros en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G242Extras>> UpdateExtra(G242Extras extras)
        {
            try
            {
                return extras != null ? await _extraIFace.UpdateExtra(extras) :
                    NotFound($"El registro {extras.Id} titulado {extras.TipoExtra} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, la tipos de extras");
            }
        }

    }
}
