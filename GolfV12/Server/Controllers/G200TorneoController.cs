using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G200TorneoController : ControllerBase
    {
        private readonly IG200Torneo _torneoIFace;

        public G200TorneoController(IG200Torneo torneoIFace)
        {
            this._torneoIFace = torneoIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G200Torneo>>> Buscar(int ejercicio, 
            string? titulo, string? creador)
        {
            try
            {
                var resultado = await _torneoIFace.Buscar(ejercicio, titulo, creador);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando torneos");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTorneos()
        {
            try
            {
                return Ok(await _torneoIFace.GetTorneos());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de torneos");
            }
        }

        [HttpGet("{torneoid:int}")]
        public async Task<ActionResult<G200Torneo>> GetTorneo(int torneoId)
        {
            try
            {
                var resultado = await _torneoIFace.GetTorneo(torneoId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un torneo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G200Torneo>> AddTorneo(G200Torneo torneo)
        {
            try
            {
                if (torneo == null) return BadRequest();
                var newTorneo = await _torneoIFace.AddTorneo(torneo);
                return CreatedAtAction(nameof(GetTorneo), new { torneoId = torneo.Id }, newTorneo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear un nuevo torneo en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G200Torneo>> UpdateTorneo(G200Torneo torneo)
        {
            try
            {
                return torneo != null ? await _torneoIFace.UpdateTorneo(torneo) :
                    NotFound($"torneo {torneo.Ejercicio} {torneo.Titulo} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de los torneos");
            }
        }
    }
}
