using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G208CategoriaTRepo : IG208CategoriaT
    {
        private readonly ApplicationDbContext _appDbContext;

        public G208CategoriaTRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G208CategoriaT> AddCategoria(G208CategoriaT categoria)
        {
            var res = await _appDbContext.CategoriasT.AddAsync(categoria);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G208CategoriaT>> Buscar(int torneo, string? titulo)
        {
            IQueryable<G208CategoriaT> querry = _appDbContext.CategoriasT;
            if (torneo > 0) querry = querry.Where(e => e.Torneo == torneo);
            if (!string.IsNullOrEmpty(titulo)) querry = querry.Where(e => e.Titulo.Contains(titulo));

            return await querry.ToListAsync();
        }

        public async Task<G208CategoriaT> GetCategoria(int categoriaId)
        {
            var res = await _appDbContext.CategoriasT.FirstOrDefaultAsync(e => e.Id == categoriaId);
            return res != null ? res : new G208CategoriaT();
        }

        public async Task<IEnumerable<G208CategoriaT>> GetCategorias()
        {
            return await _appDbContext.CategoriasT.ToListAsync();
        }

        public async Task<G208CategoriaT> UpdateCategoria(G208CategoriaT categoria)
        {
            var res = await _appDbContext.CategoriasT.FirstOrDefaultAsync(e => e.Id == categoria.Id);
            if (res != null)
            {
                if (categoria.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Torneo = categoria.Torneo;
                    res.Titulo = categoria.Titulo;
                    res.Desc = categoria.Desc;
                    res.Bandera = categoria.Bandera;
                    res.NumJugadores = categoria.NumJugadores;
                    res.Estado = categoria.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G208CategoriaT();
            }
            return res;
        }
    }
}
