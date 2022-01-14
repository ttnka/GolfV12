using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G176HoyoController : ControllerBase
    {
        private readonly IG176Hoyo _hoyoIFace;

        public G176HoyoController(IG176Hoyo hoyoIFace)
        {
            this._hoyoIFace = hoyoIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G176Hoyo>>> Buscar(string campo, string ruta, int hoyoN)
        {
            try
            {
                var resultado = await _hoyoIFace.Buscar( campo, ruta, hoyoN);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando Hoyos del campo");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetHoyos()
        {
            try
            {
                return Ok(await _hoyoIFace.GetHoyos());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, listado de hoyos del campo");
            }
        }

        [HttpGet("{hoyoid:int}")]
        public async Task<ActionResult<G176Hoyo>> GetHoyo(int hoyoId)
        {
            try
            {
                var resultado = await _hoyoIFace.GetHoyo(hoyoId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando un hoyo del campo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G176Hoyo>> NewHoyo(G176Hoyo hoyo)
        {
            try
            {
                if (hoyo == null) return BadRequest();
                var newHoyo = await _hoyoIFace.AddHoyo(hoyo);
                return CreatedAtAction(nameof(GetHoyo), new { hoyo = hoyo.Id }, newHoyo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar crear un nuevo Hoyo de campo en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G176Hoyo>> UpdateHoyo(G176Hoyo hoyo)
        {
            try
            {
                return hoyo != null ? await _hoyoIFace.UpdateHoyo(hoyo) : 
                    NotFound($"En el campo {hoyo.Campo.Corto} el hoyo {hoyo.Hoyo} no fue encontrado para actualizar");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar actualizar la base de datos, en hoyo");
            }
        }
    }
}
