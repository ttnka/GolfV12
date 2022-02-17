using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G224RolTController : ControllerBase 
    {
        private readonly IG224RolT _rolIFace;

        public G224RolTController(IG224RolT rolIFace)
        {
            this._rolIFace = rolIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G224RolT>>> Buscar(int torneo)
        {
            try
            {
                var resultado = await _rolIFace.Buscar(torneo);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando rol de torneo");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRoles()
        {
            try
            {
                return Ok(await _rolIFace.GetRoles());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de roles");
            }
        }

        [HttpGet("{rolid:int}")]
        public async Task<ActionResult<G224RolT>> GetRol(int rolId)
        {
            try
            {
                var resultado = await _rolIFace.GetRol(rolId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un rol de juego del torneo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G224RolT>> AddRol(G224RolT rol)
        {
            try
            {
                if (rol == null) return BadRequest();
                var newRol = await _rolIFace.AddRol(rol);
                return CreatedAtAction(nameof(GetRol), new { rolId = rol.Id }, newRol);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear un nuevo Rol de juego de torneo en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G224RolT>> UpdateRol(G224RolT rol)
        {
            try
            {
                return rol != null ? await _rolIFace.UpdateRol(rol) :
                    NotFound($"El Rol numero {rol.Id} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de los roles de juego del torneo");
            }
        }
    }
}
