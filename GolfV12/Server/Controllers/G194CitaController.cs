using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G194CitaController : ControllerBase
    {
        private readonly IG194Cita _citaIFace;

        public G194CitaController(IG194Cita citaIFace)
        {
            this._citaIFace = citaIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G194Cita>>> Buscar(int creador, string desc, 
                                       int masterId, DateTime fini, DateTime ffin)
        {
            try
            {
                var resultado = await _citaIFace.Buscar(creador, desc, masterId, fini, ffin);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando citas del calendario");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCitas()
        {
            try
            {
                return Ok(await _citaIFace.GetCitas());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, listado de citas");
            }
        }

        [HttpGet("{citaid:int}")]
        public async Task<ActionResult<G194Cita>> GetCita(int citaId)
        {
            try
            {
                var resultado = await _citaIFace.GetCita(citaId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando una cita");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G194Cita>> NewCita(G194Cita cita)
        {
            try
            {
                if (cita == null) return BadRequest();
                var newCita = await _citaIFace.AddCita(cita);
                return CreatedAtAction(nameof(GetCita), new { citaId = cita.Id }, newCita);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar crear una nueva cita en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G194Cita>> UpdateCita(G194Cita cita)
        {
            try
            {
                return cita != null ? await _citaIFace.UpdateCita(cita) : 
                    NotFound($"Cita {cita.Id} {cita.Desc} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar actualizar la base de datos, de la cita");
            }
        }
    }
}
