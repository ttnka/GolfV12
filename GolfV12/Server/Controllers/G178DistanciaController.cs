using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G178DistanciaController : ControllerBase
    {
        private readonly IG178Distancia _distanciaIFace;

        public G178DistanciaController(IG178Distancia distanciaIFace)
        {
            this._distanciaIFace = distanciaIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G178Distancia>>> Buscar(string campo, string bandera, int hoyoN)
        {
            try
            {
                var resultado = await _distanciaIFace.Buscar( campo, bandera, hoyoN);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando distancia de hoyos");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetDistancias()
        {
            try
            {
                return Ok(await _distanciaIFace.GetDistancias());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, listado de distancias de hoyos");
            }
        }

        [HttpGet("{distanciaid:int}")]
        public async Task<ActionResult<G178Distancia>> GetDistancia(int distanciaId)
        {
            try
            {
                var resultado = await _distanciaIFace.GetDistancia(distanciaId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando una distancia de hoyo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G178Distancia>> NewDistancia(G178Distancia distancia)
        {
            try
            {
                if (distancia == null) return BadRequest();
                var newDistancia = await _distanciaIFace.AddDistancia(distancia);
                return CreatedAtAction(nameof(GetDistancia), new { distanciaId = distancia.Id }, newDistancia);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar crear una distancia a un hoyo en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G178Distancia>> UpdateDistancia(G178Distancia distancia)
        {
            try
            {
                return distancia != null ? await _distanciaIFace.UpdateDistancia(distancia) : 
                    NotFound($"El hoyo {distancia.Hoyo} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar actualizar la base de datos, del hoyo para actualizar la distancia");
            }
        }
    }
}
