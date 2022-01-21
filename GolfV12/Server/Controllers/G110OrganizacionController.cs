using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G110OrganizacionController : ControllerBase
    {
        private readonly IG110Organizacion _orgIFace;

        public G110OrganizacionController(IG110Organizacion orgIFace)
        {
            this._orgIFace = orgIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G110Organizacion>>> Buscar(
            string clave, string nombre, string desc)
        {
            try
            {
                var resultado = await _orgIFace.Buscar(clave, nombre, desc);
                return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando organizaciones");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetOrganizaciones()
        {
            try
            {
                return Ok(await _orgIFace.GetOrganizaciones());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de organizaciones");
            }
        }

        [HttpGet("{organizacionid:int}")]
        public async Task<ActionResult<G110Organizacion>> GetOrganizacion(int organizacionId)
        {
            try
            {
                var resultado = await _orgIFace.GetOrganizacion(organizacionId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al leer la base de datos, buscando una organizacion");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G110Organizacion>> AddOrganizacion(G110Organizacion organizacion)
        {
            try
            {
                if (organizacion == null) return BadRequest();
                var newOrganizacion = await _orgIFace.AddOrganizacion(organizacion);
                return CreatedAtAction(nameof(GetOrganizacion), new { organizacionId = organizacion.Id }, newOrganizacion);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar crear una nueva organizacion en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G110Organizacion>> UpdateOrganizacion(G110Organizacion organizacion)
        {
            try
            {
                return organizacion != null ? await _orgIFace.UpdateOrganizacion(organizacion) :
                    NotFound($"Organizacion {organizacion.Nombre} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error al intentar actualizar la base de datos, de las organizaciones");
            }
        }
    }
}
