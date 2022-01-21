using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G180EstadoRepo : IG180Estado
    {
        private readonly ApplicationDbContext _appDbContext;

        public G180EstadoRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G180Estado> AddEstado(G180Estado estado)
        {
            var res = await _appDbContext.Estados.AddAsync(estado);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G180Estado>> Buscar(string titulo, string grupo)
        {
            IQueryable<G180Estado> querry = _appDbContext.Estados;
            if (!string.IsNullOrEmpty(titulo)) querry = querry.Where(e => e.Titulo.Contains(titulo));
            if(!string.IsNullOrEmpty(grupo)) querry = querry.Where(e => e.Grupo.Contains(grupo));
            return await querry.ToListAsync();
        }

        public async Task<G180Estado> GetEstado(int estadoId)
        {
            var res = await _appDbContext.Estados.FirstOrDefaultAsync(e => e.Id == estadoId);
            return res != null ? res : new G180Estado();
        }

        public async Task<IEnumerable<G180Estado>> GetEstados()
        {
            return await _appDbContext.Estados.ToListAsync();
        }

        public async Task<G180Estado> UpdateEstado(G180Estado estado)
        {
            var res = await _appDbContext.Estados.FirstOrDefaultAsync(e => e.Id == estado.Id);
            if (res != null)
            {
                if (estado.Status == false) 
                {
                    res.Status = false;
                } else
                {
                    res.Indice = estado.Indice;
                    res.Titulo = estado.Titulo;
                    res.Grupo = estado.Grupo;
                    res.Estado = estado.Estado; 
                    res.Status = estado.Status;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G180Estado();
            }
            return res;
        }
    }
}
