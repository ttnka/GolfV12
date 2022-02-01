using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G190BitacoraController : ControllerBase
    {
        private readonly IG190Bitacora _bitacoraIFace;

        public G190BitacoraController(IG190Bitacora bitacoraIFace)
        {
            this._bitacoraIFace = bitacoraIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G190Bitacora>>> Buscar(int playerId, bool sitema,
            BitaAcciones? accion, string texto, DateTime fini, DateTime ffin)
        {
            try
            {
                var resultado = await _bitacoraIFace.Buscar(playerId, sitema, accion, texto, fini, ffin);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando registros de bitacora");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetBitacoras()
        {
            try
            {
                return Ok(await _bitacoraIFace.GetBitacoraAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, listado de registros de bitacora");
            }
        }

        [HttpGet("{bitacorasid:int}")]
        public async Task<ActionResult<G190Bitacora>> GetBitacora(int bitacoraId)
        {
            try
            {
                var resultado = await _bitacoraIFace.GetBitacora(bitacoraId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando un registro de bitacora");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G190Bitacora>> AddBitacora(G190Bitacora bitacora)
        {
            try
            {
                if (bitacora == null) return BadRequest();
                var newBitacora = await _bitacoraIFace.AddBitacora(bitacora);
                return CreatedAtAction(nameof(GetBitacora), new { bitacoraId = bitacora.Id }, newBitacora);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar crear un nuevo registro de bitacora en la base de datos2.");
            }
        }
        
    }
}
