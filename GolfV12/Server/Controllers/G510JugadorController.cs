using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G510JugadorController : ControllerBase
    {
        private readonly IG510Jugador _jugadorIFace;

        public G510JugadorController(IG510Jugador jugadorIFace)
        {
            this._jugadorIFace = jugadorIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G510Jugador>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _jugadorIFace.Filtro(clave);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando jugadores de una tarjeta!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<G510Jugador>> AddTarjeta(G510Jugador jugador)
        {
            try
            {
                if (jugador == null) return BadRequest();
                return await _jugadorIFace.AddJugador(jugador);
                /*
                var newJugador = await _jugadorIFace.AddJugador(jugador);
                //return CreatedAtAction(nameof(GetOrganizacion), new { organizacionId = organizacion.Id }, newOrganizacion);
                return CreatedAtAction(nameof(Filtro), new { clave = $"jug1_-_id_-_{jugador.Id}" }, newJugador);
                */
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar agregar un jugador a la tarjeta.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G510Jugador>> UpdateJugador(G510Jugador jugador)
        {
            try
            {
                return jugador != null ? await _jugadorIFace.UpdateJugador(jugador) :
                    NotFound($"Tarjeta {jugador.Player} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la tarjeta de juego");
            }
        }
    }
}
