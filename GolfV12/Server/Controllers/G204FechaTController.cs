using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G204FechaTController : ControllerBase 
    {
        private readonly IG204FechaT _fechaIFace;

        public G204FechaTController(IG204FechaT fechaIFace)
        {
            this._fechaIFace = fechaIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G204FechaT>>> Buscar(int torneo, DateTime fecha)
        {
            try
            {
                var resultado = await _fechaIFace.Buscar(torneo, fecha);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando Fechas de torneo");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetFechasT()
        {
            try
            {
                return Ok(await _fechaIFace.GetFechasT());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de Fechas de Torneo");
            }
        }

        [HttpGet("{fechatid:int}")]
        public async Task<ActionResult<G204FechaT>> GetFechaT(int fechaTId)
        {
            try
            {
                var resultado = await _fechaIFace.GetFechaT(fechaTId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando una fecha del torneo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G204FechaT>> AddFechaT(G204FechaT fechaT)
        {
            try
            {
                if (fechaT == null) return BadRequest();
                var newfechaT = await _fechaIFace.AddFechaT(fechaT);
                var new2fec = CreatedAtAction(nameof(GetFechaT), new { fechaTId = fechaT.Id }, newfechaT);
                return new2fec;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear una nueva fecha de Torneo en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G204FechaT>> UpdateFechaT(G204FechaT fechaT)
        {
            try
            {
                return fechaT != null ? await _fechaIFace.UpdateFechaT(fechaT) :
                    NotFound($"El Registro {fechaT.Id} con fecha {fechaT.Fecha} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las fechas de Torneo");
            }
        }
    }
}
