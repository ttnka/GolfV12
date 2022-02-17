using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G249TiroEstadisticaController : ControllerBase 
    {
        private readonly IG249TiroEst _tiroEIFace;

        public G249TiroEstadisticaController(IG249TiroEst tiroEIFace)
        {
            this._tiroEIFace = tiroEIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G249TiroEstadistica>>> Buscar(int rol, string? player, 
            int hoyo, TiroTipo? tiroTipo)
        {
            try
            {
                var resultado = await _tiroEIFace.Buscar(rol, player, hoyo, tiroTipo);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando en las estaditicas de los tiros");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTiroEsts()
        {
            try
            {
                return Ok(await _tiroEIFace.GetTiroEsts());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de estadistica de tiros");
            }
        }

        [HttpGet("{tiroEstid:int}")]
        public async Task<ActionResult<G249TiroEstadistica>> GetTiroEst(int tiroEstId)
        {
            try
            {
                var resultado = await _tiroEIFace.GetTiroEst(tiroEstId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando una estadistica de tiro");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G249TiroEstadistica>> AddTiroEst(G249TiroEstadistica tiroEst)
        {
            try
            {
                if (tiroEst == null) return BadRequest();
                var newTiroEst = await _tiroEIFace.AddTiroEst(tiroEst);
                return CreatedAtAction(nameof(GetTiroEst), new { tiroEstId = tiroEst.Id }, newTiroEst);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear un nuevo registro de estadistica de tiro en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G249TiroEstadistica>> UpdateTiroEst(G249TiroEstadistica tiroEst)
        {
            try
            {
                return tiroEst != null ? await _tiroEIFace.UpdateTiroEst(tiroEst) :
                    NotFound($"La estadistica numero {tiroEst.Id} no fue encontrada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de la estadistica de tiro");
            }
        }
    }
}
