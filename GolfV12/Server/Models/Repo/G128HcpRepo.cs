using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G128HcpRepo : IG128Hcp
    {
        private readonly ApplicationDbContext _appDBContext;

        public G128HcpRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDBContext = applicationDbContext;
        }
        public async Task<G128Hcp> AddHcp(G128Hcp hcp)
        {
            var newHcp = await _appDBContext.Hcps.AddAsync(hcp);
            await _appDBContext.SaveChangesAsync();
            return newHcp.Entity;
        }

        public async Task<IEnumerable<G128Hcp>> Buscar(string playerId)
        {
            IQueryable<G128Hcp> querry = _appDBContext.Hcps;
            if (!string.IsNullOrEmpty(playerId)) querry = querry.Where(e => e.PlayerId.Contains(playerId));
            
            return await querry.ToListAsync();
        }

        public async Task<G128Hcp> GetHcp(int hcpId)
        {
            var res = await _appDBContext.Hcps.FirstOrDefaultAsync(e => e.Id == hcpId);
            return res != null ? res : new G128Hcp();
        }

        public async Task<IEnumerable<G128Hcp>> GetHcps()
        {
            return await _appDBContext.Hcps.ToListAsync();
        }

        public async Task<G128Hcp> UpdateHcp(G128Hcp hcp)
        {
            var res = await _appDBContext.Hcps.FirstOrDefaultAsync(e => e.Id == hcp.Id);
            if (res != null)
            {
                if (hcp.Status == false )
                {
                    res.Status = false;
                } else
                {
                    res.PlayerId = hcp.PlayerId;
                    res.Fecha = hcp.Fecha;
                    res.BanderaId = hcp.BanderaId;
                    res.Hcp = hcp.Hcp;
                    res.Estado = hcp.Estado;
                    res.Status = hcp.Status;
                }
                await _appDBContext.SaveChangesAsync();
            } 
            else
            {
                res = new G128Hcp();
            }
            return res;
        }
        
    }
}
