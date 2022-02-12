using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G136FotoController : ControllerBase
    {
        private readonly IG136Foto _fotoIFace;
        public G136FotoController(IG136Foto fotoIFace)
        {
            this._fotoIFace = fotoIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G136Foto>>> Buscar(string? playerId, string? titulo, DateTime bday)
        {
            try
            {
                var resultado = await _fotoIFace.Buscar(playerId, titulo, bday);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, buscando fotografias");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetFotos()
        {
            try
            {
                return Ok(await _fotoIFace.GetFotos());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, listado de fotografias");
            }
        }

        [HttpGet("{fotoid:int}")]
        public async Task<ActionResult<G136Foto>> GetFoto(int fotoId)
        {
            try
            {
                var resultado = await _fotoIFace.GetFoto(fotoId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, al traer una fotografia");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G136Foto>> NewPlayer(G136Foto foto)
        {
            try
            {
                if (foto == null) return BadRequest();
                var newFoto = await _fotoIFace.AddFoto(foto);
                return CreatedAtAction(nameof(GetFoto), new { fotoId = foto.Id }, newFoto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar agregar una fotografia a la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G136Foto>> UpdateFoto(G136Foto foto)
        {
            try
            {
                return foto != null ? await _fotoIFace.UpdateFoto(foto) : 
                    NotFound($"Fotografia no encontrada {foto.Titulo} ");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar actualizar la base de datos, de la fotografia.");
            }
        }
    }

}
