using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GolfV12.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G208CategoriaTController : ControllerBase 
    {
        private readonly IG208CategoriaT _catIFace;

        public G208CategoriaTController(IG208CategoriaT catIFace)
        {
            this._catIFace = catIFace;
        }
        [HttpGet("{filtro}")]
        public async Task<ActionResult<IEnumerable<G208CategoriaT>>> Buscar(int torneo, string? titulo)
        {
            try
            {
                var resultado = await _catIFace.Buscar(torneo, titulo);
                return Ok(resultado);
                //return resultado.Any() ? Ok(resultado) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al leer la base de datos, " +
                    "buscando categorias de torneo");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCategorias()
        {
            try
            {
                return Ok(await _catIFace.GetCategorias());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando lista de categorias de torneo");
            }
        }

        [HttpGet("{categoriaid:int}")]
        public async Task<ActionResult<G208CategoriaT>> GetCategoria(int categoriaId)
        {
            try
            {
                var resultado = await _catIFace.GetCategoria(categoriaId);
                return resultado != null ? resultado : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al leer la base de datos, buscando una categoria de torneo");
            }
        }
        [HttpPost]
        public async Task<ActionResult<G208CategoriaT>> AddCategoria(G208CategoriaT categoria)
        {
            try
            {
                if (categoria == null) return BadRequest();
                var newCategoria = await _catIFace.AddCategoria(categoria);
                return CreatedAtAction(nameof(GetCategoria), new { categoriaId = categoria.Id }, newCategoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar crear una nueva categoria en la base de datos.");
            }
        }
        [HttpPut]
        public async Task<ActionResult<G208CategoriaT>> Updatecategoria(G208CategoriaT categoria)
        {
            try
            {
                return categoria != null ? await _catIFace.UpdateCategoria(categoria) :
                    NotFound($"La categoria {categoria.Titulo} no fue encontrada");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error al intentar actualizar la base de datos, de las categorias");
            }
        }
    }
}
