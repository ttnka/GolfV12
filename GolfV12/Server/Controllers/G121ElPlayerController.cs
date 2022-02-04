using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G121ElPlayerController : ControllerBase 
    {
        private readonly IG121ElPlayer _elPlayerIFace;

        public G121ElPlayerController(IG121ElPlayer ElPlayerIFace)
        {
           this._elPlayerIFace = ElPlayerIFace;
        }
        [HttpGet("{userid}")]
        public async Task<ActionResult<G120Player>> GetPlayer(string userId)
        {
            try
            {
                var resultado = await _elPlayerIFace.GetPlayer(userId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, buscando un jugador");
            }
        }
    }
}
