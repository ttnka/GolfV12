using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G249TiroEstRepo : IG249TiroEst
    {
        private readonly ApplicationDbContext _appDbContext;

        public G249TiroEstRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G249TiroEstadistica> AddTiroEst(G249TiroEstadistica tiroEst)
        {
            var res = await _appDbContext.TirosEst.AddAsync(tiroEst);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G249TiroEstadistica>> Buscar(int rol, int player, 
            int hoyo, TiroTipo? tiroTipo)
        {
            IQueryable<G249TiroEstadistica> querry = _appDbContext.TirosEst;
            if (rol > 0) querry = querry.Where(e => e.Rol == rol);
            if (player > 0) querry = querry.Where(e => e.Player == player); 
            if (hoyo > 0) querry = querry.Where(e => e.Hoyo == hoyo);
            if (tiroTipo.HasValue) querry = querry.Where(e => e.TirosTipo == tiroTipo );
            
            return await querry.ToListAsync();
        }

        public async Task<G249TiroEstadistica> GetTiroEst(int tiroEstId)
        {
            var res = await _appDbContext.TirosEst.FirstOrDefaultAsync(e => e.Id == tiroEstId);
            return res != null ? res : new G249TiroEstadistica();
        }

        public async Task<IEnumerable<G249TiroEstadistica>> GetTiroEsts()
        {
            return await _appDbContext.TirosEst.ToListAsync();
        }

        public async Task<G249TiroEstadistica> UpdateTiroEst(G249TiroEstadistica tiroEst)
        {
            var res = await _appDbContext.TirosEst.FirstOrDefaultAsync(e => e.Id == tiroEst.Id);
            if (res != null)
            {
                if (tiroEst.Status == false)
                {
                    res.Status = false;
                } else
                {
                    res.Rol = tiroEst.Rol;
                    res.Player = tiroEst.Player;
                    res.Hoyo = tiroEst.Hoyo;
                    res.TirosTipo = tiroEst.TirosTipo;
                    res.Cantidad = tiroEst.Cantidad;
                    res.Estado = tiroEst.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G249TiroEstadistica();
            }
            return res;
        }
    }
}
