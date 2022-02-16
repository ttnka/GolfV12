using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G172BanderaRepo : IG172Bandera
    {
        private readonly ApplicationDbContext _appDbContext;

        public G172BanderaRepo(ApplicationDbContext applicationDbContext
            )
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G172Bandera> AddBandera(G172Bandera bandera)
        {
            var res = await _appDbContext.Banderas.AddAsync(bandera);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G172Bandera>> Buscar(int campo, string? color)
        {
            IQueryable<G172Bandera> querry = _appDbContext.Banderas;
            if (campo > 0 ) querry = querry.Where(e => e.CampoId == campo);
            if (!string.IsNullOrEmpty(color)) querry = querry.Where(e => e.Color.Contains(color));
            return await querry.ToListAsync();
        }

        public async Task<G172Bandera> GetBandera(int banderaId)
        {
            var res = await _appDbContext.Banderas.FirstOrDefaultAsync(e => e.Id == banderaId);
            return res != null ? res : new G172Bandera();
        }

        public async Task<IEnumerable<G172Bandera>> GetBanderas()
        {
            return await _appDbContext.Banderas.ToListAsync();
        }

        public async Task<G172Bandera> UpdateBandera(G172Bandera bandera)
        {
            var res = await _appDbContext.Banderas.FirstOrDefaultAsync(e => e.Id == bandera.Id);
            if (res != null)
            {
                if (bandera.Status == false)
                {
                    res.Status = bandera.Status;
                } else
                {
                    res.CampoId = bandera.CampoId;
                    res.Color = bandera.Color;
                    res.Estado = bandera.Estado;
                    res.Status = bandera.Status;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G172Bandera();
            }
            return res;
        }
    }
}
