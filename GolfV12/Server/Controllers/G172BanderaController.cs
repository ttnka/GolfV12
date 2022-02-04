using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    public class G172BanderaController : ControllerBase
    {
        private readonly IG172Bandera _banderaIFace;

        public G172BanderaController(IG172Bandera banderaIFace)
        {
            this._banderaIFace = banderaIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G172Bandera>>> Buscar(int campo, string? color)
        {
            try
            {
                var resultado = await _banderaIFace.Buscar(campo, color);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando banderas");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetBanderas()
        {
            try
            {
                return Ok(await _banderaIFace.GetBanderas());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, listado de banderas");
            }
        }

        [HttpGet("{banderaid:int}")]
        public async Task<ActionResult<G172Bandera>> GetBandera(int banderaId)
        {
            try
            {
                var resultado = await _banderaIFace.GetBandera(banderaId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando una bandera");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G172Bandera>> NewBandera(G172Bandera bandera)
        {
            try
            {
                if (bandera == null) return BadRequest();
                var newBandera = await _banderaIFace.AddBandera(bandera);
                return CreatedAtAction(nameof(GetBandera), new { banderaId = bandera.Id }, newBandera);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar crear una nueva bandera en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G172Bandera>> UpdateBandera(G172Bandera bandera)
        {
            try
            {
                return bandera != null ? await _banderaIFace.UpdateBandera(bandera) : 
                    NotFound($"La bandera {bandera.Color} del campo no puedo actualizarce");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar actualizar la base de datos, de la bandera");
            }
        }

    }
}
