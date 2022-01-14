using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G120PlayerController : ControllerBase
    {
        private readonly IG120Player _playerIFace;

        public G120PlayerController(IG120Player playerIFace)
        {
            this._playerIFace = playerIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G120Player>>> Buscar(
            string org, string apodo, string nombre, string paterno, DateTime bday)
        {
            try
            {
                var resultado = await _playerIFace.Buscar(org, apodo, nombre, paterno, bday);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando jugadores");
            }
        }
        
        [HttpGet]
        public async Task<ActionResult> GetPlayers()
        {
            try
            {
                return Ok(await _playerIFace.GetPlayers());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando jugadores");
            }
        }

        [HttpGet("{playerid:int}")]
        public async Task<ActionResult<G120Player>> GetPlayer(int playerId)
        {
            try
            {
                var resultado = await _playerIFace.GetPlayer(playerId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, buscando un jugador");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G120Player>> NewPlayer(G120Player player)
        {
            try
            {
                if (player == null) return BadRequest();
                var newPlayer = await _playerIFace.AddPlayer(player);
                return CreatedAtAction(nameof(GetPlayer), new { player = player.Id }, newPlayer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al intentar crear un nuevo jugador en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G120Player>> UpdatePlayer(G120Player player)
        {
            try
            {
                return player != null ? await _playerIFace.UpdatePlayer(player) : 
                    NotFound($"Jugador {player.Nombre} {player.Paterno} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al intentar actualizar la base de datos, del jugador");
            }
        }

    }
}
