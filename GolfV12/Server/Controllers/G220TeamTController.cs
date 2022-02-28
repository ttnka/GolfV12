using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G220TeamTController : ControllerBase 
    {
        private readonly IG220TeamT _teamIFace;

        public G220TeamTController(IG220TeamT teamIFace)
        {
            this._teamIFace = teamIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G220TeamT>>> Buscar(int torneo, int teamNum, string? nombre)
        {
            try
            {
                var resultado = await _teamIFace.Buscar(torneo, teamNum, nombre);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando Equipos de Torneo");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTeams()
        {
            try
            {
                return Ok(await _teamIFace.GetTeams());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de equipos de Toreno");
            }
        }

        [HttpGet("{teamid:int}")]
        public async Task<ActionResult<G220TeamT>> GetTeam(int teamId)
        {
            try
            {
                var resultado = await _teamIFace.GetTeam(teamId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un equipo de toreno");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G220TeamT>> AddTeam(G220TeamT team)
        {
            try
            {
                if (team == null) return BadRequest();
                var newTeam = await _teamIFace.AddTeam(team);
                return CreatedAtAction(nameof(GetTeam), new { teamId = team.Id }, newTeam);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear un nuevo equipo para torneo en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G220TeamT>> UpdateTeam(G220TeamT team)
        {
            try
            {
                return team != null ? await _teamIFace.UpdateTeam(team) :
                    NotFound($"El equipo numero {team.TeamNum} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de la equipos del torneo");
            }
        }
    }
}
