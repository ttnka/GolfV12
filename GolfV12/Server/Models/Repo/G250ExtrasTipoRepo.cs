using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G250ExtrasTipoRepo : IG250ExtrasTipo
    {
        private readonly ApplicationDbContext _appDbContext;

        public G250ExtrasTipoRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G250ExtrasTipo> AddExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            var res = await _appDbContext.ExtrasTipos.AddAsync(extrasTipo);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G250ExtrasTipo>> Buscar(string? titulo, int creador, 
            string? grupo, bool publico)
        {
            IQueryable<G250ExtrasTipo> querry = _appDbContext.ExtrasTipos;
            if (!string.IsNullOrEmpty(titulo)) querry = querry.Where(e => e.Titulo.Contains(titulo));
            if (creador > 0) querry = querry.Where(e => e.Creador == creador);
            if (!string.IsNullOrEmpty(grupo)) querry = querry.Where(e => e.Grupo.Contains(grupo));
            if(publico) querry = querry.Where(e=>e.Publico == true);   
            
            return await querry.ToListAsync();
        }

        public async Task<G250ExtrasTipo> GetExtrasTipo(int extrasTipoId)
        {
            var res = await _appDbContext.ExtrasTipos.FirstOrDefaultAsync(e => e.Id == extrasTipoId);
            return res != null ? res : new G250ExtrasTipo();
        }

        public async Task<IEnumerable<G250ExtrasTipo>> GetExtrasTipos()
        {
            return await _appDbContext.ExtrasTipos.ToListAsync();
        }

        public async Task<G250ExtrasTipo> UpdateExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            var res = await _appDbContext.ExtrasTipos.FirstOrDefaultAsync(e => e.Id == extrasTipo.Id);
            if (res != null)
            {
                if (extrasTipo.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Titulo = extrasTipo.Titulo;
                    res.Desc = extrasTipo.Desc;
                    res.Valor = extrasTipo.Valor;
                    res.Creador = extrasTipo.Creador;
                    res.Grupo = extrasTipo.Grupo;
                    res.Publico = extrasTipo.Publico;
                    res.Estado = extrasTipo.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G250ExtrasTipo();
            }
            return res;
        }
    }
}
