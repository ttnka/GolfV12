using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G170campoRepo : IG170Campo
    {
        private readonly ApplicationDbContext _appDbContext;

        public G170campoRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G170Campo> AddCampo(G170Campo campo)
        {
            var res = await _appDbContext.Campos.AddAsync(campo);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G170Campo>> Buscar(string? corto, string? nombre, string? ciudad, 
            string? pais)
        {
            IQueryable<G170Campo> querrey = _appDbContext.Campos;
            if (!string.IsNullOrEmpty(corto)) querrey = querrey.Where(e => e.Corto.Contains(corto));
            if (!string.IsNullOrEmpty(nombre)) querrey = querrey.Where(e => e.Nombre.Contains(nombre));
            if (!string.IsNullOrEmpty(ciudad)) querrey = querrey.Where(e => e.Ciudad.Contains(ciudad));
            if (!string.IsNullOrEmpty(pais)) querrey = querrey.Where(e => e.Pais.Contains(pais));
            
            return await querrey.ToListAsync();
        }

        public async Task<G170Campo> GetCampo(int campoId)
        {
            var res = await _appDbContext.Campos.FirstOrDefaultAsync(e => e.Id == campoId);
            return res != null ? res : new G170Campo();
        }

        public async Task<IEnumerable<G170Campo>> GetCampos()
        {
            return await _appDbContext.Campos.ToListAsync();
        }

        public async Task<G170Campo> UpdateCampo(G170Campo campo)
        {
            var res = await _appDbContext.Campos.FirstOrDefaultAsync(e => e.Id == campo.Id);
            if (res != null)
            {
                if(campo.Status == false)
                {
                    res.Status = false;
                } else
                {
                    res.Corto = campo.Corto;
                    res.Nombre = campo.Nombre;
                    res.Desc = campo.Desc;
                    res.Ciudad = campo.Ciudad;
                    res.Pais = campo.Pais;
                    res.Estado = campo.Estado;
                    res.Status = campo.Status;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G170Campo();
            }
            return res;
        }
    }
}
