using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G242ExtrasRepo : IG242Extras
    {
        private readonly ApplicationDbContext _appDbContext;

        public G242ExtrasRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G242Extras> AddExtra(G242Extras extra)
        {
            var res = await _appDbContext.Extras.AddAsync(extra);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G242Extras>> Buscar(int rol, int player, int hoyo, int tipoExtra)
        {
            IQueryable<G242Extras> querry = _appDbContext.Extras;
            if (rol > 0) querry = querry.Where(e => e.Rol == rol);
            if (player > 0) querry = querry.Where(e => e.Player == player);
            if (tipoExtra > 0) querry = querry.Where(e => e.TipoExtra == tipoExtra);

            return await querry.ToListAsync();
        }

        public async Task<G242Extras> GetExtra(int extraId)
        {
            var res = await _appDbContext.Extras.FirstOrDefaultAsync(e => e.Id == extraId);
            return res != null ? res : new G242Extras();
        }

        public async Task<IEnumerable<G242Extras>> GetExtras()
        {
            return await _appDbContext.Extras.ToListAsync();
        }

        public async Task<G242Extras> UpdateExtra(G242Extras extra)
        {
            var res = await _appDbContext.Extras.FirstOrDefaultAsync(e => e.Id == extra.Id);
            if (res != null)
            {
                if (extra.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Rol = extra.Rol;
                    res.Player = extra.Player;
                    res.Hoyo = extra.Hoyo; 
                    res.TipoExtra = extra.TipoExtra;
                    res.Valor = extra.Valor;
                    res.Estado = extra.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G242Extras();
            }
            return res;
        }
    }
}
