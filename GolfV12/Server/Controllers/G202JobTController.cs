using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G202JobTController : ControllerBase
    {
        private readonly IG202JobT _jobTIFace;

        public G202JobTController(IG202JobT jobTIFace)
        {
            this._jobTIFace = jobTIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G202JobT>>> Buscar(int torneo, string? player, string? contrincante)
        {
            try
            {
                var resultado = await _jobTIFace.Buscar(torneo, player, contrincante);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando el tipo de rol del jugador -capturista-");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetJob()
        {
            try
            {
                return Ok(await _jobTIFace.GetJobs());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de roles");
            }
        }

        [HttpGet("{jobid:int}")]
        public async Task<ActionResult<G202JobT>> GetJob(int jobId)
        {
            try
            {
                var resultado = await _jobTIFace.GetJob(jobId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando un rol -captura-");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G202JobT>> Addjob(G202JobT job)
        {
            try
            {
                if (job == null) return BadRequest();
                var newJob = await _jobTIFace.AddJob(job);
                return CreatedAtAction(nameof(GetJob), new { jobId = job.Id }, newJob);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear una nuevo rol -captura- la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G202JobT>> UpdateJob(G202JobT job)
        {
            try
            {
                return job != null ? await _jobTIFace.UpdateJob(job) :
                    NotFound($"el Rol -captura- del jugador {job.Player} no fue encontrado");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de los roles de -captura-");
            }
        }
    }
}
