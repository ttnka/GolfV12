using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G128HcpController : ControllerBase 
    {
        private readonly IG128Hcp _hcpIFace;
        public G128HcpController(IG128Hcp hcpIFace)
        {
            this._hcpIFace = hcpIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G128Hcp>>> Buscar(string playerId)
        {
            try
            {
                var resultado = await _hcpIFace.Buscar(playerId);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando Hcp");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetHcps()
        {
            try
            {
                return Ok(await _hcpIFace.GetHcps());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando Hcp de los jugadores");
            }
        }

        [HttpGet("{playerid:int}")]
        public async Task<ActionResult<G128Hcp>> GetHcp(int playerId)
        {
            try
            {
                var resultado = await _hcpIFace.GetHcp(playerId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando Hcp de los jugadores");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G128Hcp>> AddHcp(G128Hcp hcp)
        {
            try
            {
                if (hcp == null) return BadRequest();
                var newHcp = await _hcpIFace.AddHcp(hcp);
                return CreatedAtAction(nameof(GetHcp), new { playerId = newHcp.Id }, newHcp);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar agregar la base de datos, buscando Hcp de los jugadores");
            }
        }

        [HttpPut]
        public async Task<ActionResult<G128Hcp>> UpdateHcp(G128Hcp hcp)
        {
            try
            {
                return hcp != null ? await _hcpIFace.UpdateHcp(hcp) : 
                    NotFound($"No se encontro el registro hcp para actualizarlo");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar actualizar la base de datos, del jugador");
            }
        }

    }
}
