using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G310ParticipantesController : ControllerBase 
    {
        private readonly IG310Participante _participanteIFace;

        public G310ParticipantesController(IG310Participante participanteIFace)
        {
            this._participanteIFace = participanteIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G310Participantes>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _participanteIFace.Filtro(clave);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando tarjetas de score!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<G310Participantes>> AddAzar(G310Participantes participante)
        {
            try
            {
                if (participante == null) return BadRequest();
                return await _participanteIFace.AddParticipante(participante);
                /*
                 var newTarjeta = await _tarjIFace.AddTarjeta(tarjeta);
                 return CreatedAtAction(nameof(Filtro), new { clave = $"tar1_-_id_-_{tarjeta.Id}" });               
                */
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear una nueva organizacion en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G310Participantes>> UpdateAzar(G310Participantes participante)
        {
            try
            {
                return participante != null ? await _participanteIFace.UpdateParticipante(participante) :
                    NotFound($"Azar {participante.Id} no fue encontrada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las tarjetas de juego");
            }
        }
    }
}
