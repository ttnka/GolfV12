using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G280FormatoTController : ControllerBase 
    {
        private readonly IG280FormatoT _formatoIFace;

        public G280FormatoTController(IG280FormatoT formatoIFace)
        {
            this._formatoIFace = formatoIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G280FormatoT>>> Buscar(string? clave, string? titulo,
               string? desc, bool individual)
        {
            try
            {
                var resultado = await _formatoIFace.Buscar(clave, titulo, desc, individual);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando formatos de torneos");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetFormatos()
        {
            try
            {
                return Ok(await _formatoIFace.GetFormatos());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de formatos");
            }
        }

        [HttpGet("{formatoid:int}")]
        public async Task<ActionResult<G280FormatoT>> GetFormato(int formatoId)
        {
            try
            {
                var resultado = await _formatoIFace.GetFormato(formatoId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un formato de torneo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G280FormatoT>> AddFormato(G280FormatoT formato)
        {
            try
            {
                if (formato == null) return BadRequest();
                var newFormato = await _formatoIFace.AddFormato(formato);
                return CreatedAtAction(nameof(GetFormato), new { formatoId = formato.Id }, newFormato);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear una nueva formato en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G280FormatoT>> UpdateFormato(G280FormatoT formato)
        {
            try
            {
                return formato != null ? await _formatoIFace.UpdateFormato(formato) :
                    NotFound($"El Formato {formato.Id} {formato.Titulo} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de los formatos");
            }
        }
    }
}
