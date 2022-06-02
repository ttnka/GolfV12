using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G390TiposAzarController : ControllerBase  
    {
        private readonly IG390TiposAzar _azarTIFace;

        public G390TiposAzarController(IG390TiposAzar azarTIFace)
        {
            this._azarTIFace = azarTIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G390TiposAzar>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _azarTIFace.Filtro(clave);
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
        public async Task<ActionResult<G390TiposAzar>> AddTiposAzar(G390TiposAzar azarT)
        {
            try
            {
                if (azarT == null) return BadRequest();
                return await _azarTIFace.AddTiposAzar(azarT);
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
        public async Task<ActionResult<G390TiposAzar>> UpdateTiposAzar(G390TiposAzar azarT)
        {
            try
            {
                return azarT != null ? await _azarTIFace.UpdateTiposAzar(azarT) :
                    NotFound($"Azar {azarT.Id} no fue encontrada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las tarjetas de juego");
            }
        }
    }
}
