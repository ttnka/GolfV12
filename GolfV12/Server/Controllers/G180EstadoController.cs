using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G180EstadoController : ControllerBase
    {
        private readonly IG180Estado _estadoIFace;

        public G180EstadoController(IG180Estado estadoIFace)
        {
            this._estadoIFace = estadoIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G180Estado>>> Buscar(string titulo, string grupo)
        {
            try
            {
                var resultado = await _estadoIFace.Buscar(titulo, grupo);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando estados de registros");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetEstados()
        {
            try
            {
                return Ok(await _estadoIFace.GetEstados());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de estado de registros");
            }
        }

        [HttpGet("{estadoid:int}")]
        public async Task<ActionResult<G180Estado>> GetEstado(int estadoId)
        {
            try
            {
                var resultado = await _estadoIFace.GetEstado(estadoId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un estado del registro");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G180Estado>> AddEstado(G180Estado estado)
        {
            try
            {
                if (estado == null) return BadRequest();
                var newEstado = await _estadoIFace.AddEstado(estado);
                return CreatedAtAction(nameof(GetEstado), new { estadoId = estado.Id }, newEstado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear una nuevo estado en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G180Estado>> UpdateOrganizacion(G180Estado estado)
        {
            try
            {
                return estado != null ? await _estadoIFace.UpdateEstado(estado) :
                    NotFound($"Estado {estado.Titulo} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de los estados de los registros");
            }
        }

    }
}
