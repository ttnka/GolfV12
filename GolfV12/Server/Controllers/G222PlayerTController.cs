using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    public class G222PlayerTController : ControllerBase 
    {
        private readonly IG222PlayerT _playerIFace;

        public G222PlayerTController(IG222PlayerT playerIFace)
        {
            this._playerIFace = playerIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G222PlayerT>>> Buscar(int team, string? player)
        {
            try
            {
                var resultado = await _playerIFace.Buscar(team, player);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando Jugadores del Toreno");
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
                    "Error al leer la base de datos, buscando la lista de jugadores del toreno");
            }
        }

        [HttpGet("{playerTid:int}")]
        public async Task<ActionResult<G222PlayerT>> GetPlayer(string playerTId)
        {
            try
            {
                var resultado = await _playerIFace.GetPlayer(playerTId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un Jugador del Torneo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G222PlayerT>> AddPlayer(G222PlayerT playerT)
        {
            try
            {
                if (playerT == null) return BadRequest();
                var newPlayerT = await _playerIFace.AddPlayer(playerT);
                return CreatedAtAction(nameof(GetPlayer), new { playerTId = playerT.Id }, newPlayerT);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear un nuevo jugador para el torneo en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G222PlayerT>> UpdatePlayer(G222PlayerT playerT)
        {
            try
            {
                return playerT != null ? await _playerIFace.UpdatePlayer(playerT) :
                    NotFound($"El jugador {playerT.Player} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las playerTes");
            }
        }
    }
}
