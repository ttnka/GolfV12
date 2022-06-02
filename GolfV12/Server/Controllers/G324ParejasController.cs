using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G324ParejasController : ControllerBase
    {
        private readonly IG324Parejas _parejasIFace;

        public G324ParejasController(IG324Parejas parejasIFace)
        {
            this._parejasIFace = parejasIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G324Parejas>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _parejasIFace.Filtro(clave);
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
        public async Task<ActionResult<G324Parejas>> AddParejas(G324Parejas pareja)
        {
            try
            {
                if (pareja == null) return BadRequest();
                return await _parejasIFace.AddPareja(pareja);
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
        public async Task<ActionResult<G324Parejas>> UpdatePareja(G324Parejas parejas)
        {
            try
            {
                return parejas != null ? await _parejasIFace.UpdatePareja(parejas) :
                    NotFound($"Azar {parejas.Id} no fue encontrada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las tarjetas de juego");
            }
        }
    }
}
