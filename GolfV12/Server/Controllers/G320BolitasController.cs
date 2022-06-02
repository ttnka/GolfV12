using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G320BolitasController : ControllerBase 
    {
        private readonly IG320Bolitas _bolitasIFace;

        public G320BolitasController(IG320Bolitas bolitasIFace)
        {
            this._bolitasIFace = bolitasIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G320Bolitas>>> Filtro(string? clave)
        {
            try
            {
                var resultado = await _bolitasIFace.Filtro(clave);
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
        public async Task<ActionResult<G320Bolitas>> AddBolitas(G320Bolitas bolitas)
        {
            try
            {
                if (bolitas == null) return BadRequest();
                return await _bolitasIFace.AddBolitas(bolitas);
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
        public async Task<ActionResult<G320Bolitas>> UpdateBolitas(G320Bolitas bolitas)
        {
            try
            {
                return bolitas != null ? await _bolitasIFace.UpdateBolitas(bolitas) :
                    NotFound($"Azar {bolitas.Id} no fue encontrada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las tarjetas de juego");
            }
        }
    }
}
