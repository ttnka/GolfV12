using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G502TarjetasController : ControllerBase
    {
        private readonly IG502Tarjetas _tarjIFace;

        public G502TarjetasController(IG502Tarjetas tarjIFace)
        {
            this._tarjIFace = tarjIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G502Tarjetas>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _tarjIFace.Filtro(clave);
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
        public async Task<ActionResult<G502Tarjetas>> AddTarjeta(G502Tarjetas tarjeta)
        {
            try
            {
                if (tarjeta == null) return BadRequest();
                return await _tarjIFace.AddTarjeta(tarjeta);
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
        public async Task<ActionResult<G502Tarjetas>> UpdateTarjeta(G502Tarjetas tarjeta)
        {
            try
            {
                return tarjeta != null ? await _tarjIFace.UpdateTarjeta(tarjeta) :
                    NotFound($"Tarjeta {tarjeta.Titulo} no fue encontrada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las tarjetas de juego");
            }
        }
    }
}
