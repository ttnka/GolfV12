using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G170CampoController : ControllerBase
    {
        private readonly IG170Campo _campoIFace;
        public G170CampoController(IG170Campo campoIFace)
        {
            this._campoIFace = campoIFace;
        }

        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G170Campo>>> Buscar(string corto, string nombre, string ciudad, string pais)
        {
            try
            {
                var resultado = await _campoIFace.Buscar(corto, nombre, ciudad, pais);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando campos");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCampos()
        {
            try
            {
                return Ok(await _campoIFace.GetCampos());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, listado de campos");
            }
        }

        [HttpGet("{campoid:int}")]
        public async Task<ActionResult<G170Campo>> GetCampo(int campoId)
        {
            try
            {
                var resultado = await _campoIFace.GetCampo(campoId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un campo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G170Campo>> NewCampo(G170Campo campo)
        {
            try
            {
                if (campo == null) return BadRequest();
                var newCampo = await _campoIFace.AddCampo(campo);
                return CreatedAtAction(nameof(GetCampo), new { campo = campo.Id }, newCampo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear un nuevo campo en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G170Campo>> UpdateCampo(G170Campo campo)
        {
            try
            {
                return campo != null ? await _campoIFace.UpdateCampo(campo) :
                    NotFound($"Campo {campo.Corto} {campo.Nombre} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, del campo");
            }
        }
    }
}
