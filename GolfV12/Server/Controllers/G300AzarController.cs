using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G300AzarController : ControllerBase
    {
        private readonly IG300Azar _azarIFace;

        public G300AzarController(IG300Azar tarjIFace)
        {
            this._azarIFace = tarjIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G300Azar>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _azarIFace.Filtro(clave);
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
        public async Task<ActionResult<G300Azar>> AddAzar(G300Azar azar)
        {
            try
            {
                if (azar == null) return BadRequest();
                return await _azarIFace.AddAzar(azar);
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
        public async Task<ActionResult<G300Azar>> UpdateAzar(G300Azar azar)
        {
            try
            {
                return azar != null ? await _azarIFace.UpdateAzar(azar) :
                    NotFound($"Azar {azar.Id} no fue encontrada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las tarjetas de juego");
            }
        }
    }
}
