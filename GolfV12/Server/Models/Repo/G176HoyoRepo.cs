using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G176HoyoRepo : IG176Hoyo
    {
        private readonly ApplicationDbContext _appDbContext;

        public G176HoyoRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G176Hoyo> AddHoyo(G176Hoyo hoyo)
        {
            var res = await _appDbContext.Hoyos.AddAsync(hoyo);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G176Hoyo>> Buscar(int campo, string ruta, int hoyoN)
        {
            IQueryable<G176Hoyo> querry = _appDbContext.Hoyos;
            if (campo > -1) querry = querry.Where(e => e.CampoId == campo);
            if (!string.IsNullOrEmpty(ruta)) querry = querry.Where(e => e.Ruta.Contains(ruta));
            if (hoyoN > -1) querry = querry.Where(e => e.Hoyo == hoyoN);

            return await querry.ToListAsync();
        }

        public async Task<G176Hoyo> GetHoyo(int hoyoId)
        {
            var res = await _appDbContext.Hoyos.FirstOrDefaultAsync(e => e.Id == hoyoId);
            return res != null ? res : new G176Hoyo();
        }

        public async Task<IEnumerable<G176Hoyo>> GetHoyos()
        {
            return await _appDbContext.Hoyos.ToListAsync(); 
        }

        public async Task<G176Hoyo> UpdateHoyo(G176Hoyo hoyo)
        {
            var res = await _appDbContext.Hoyos.FirstOrDefaultAsync(e =>e.Id == hoyo.Id);
            if (res != null)
            {
                if (hoyo.Status == false)
                {
                    res.Status = false;
                } else
                {
                    res.CampoId = hoyo.CampoId;
                    res.Ruta = hoyo.Ruta;
                    res.Hoyo = hoyo.Hoyo;
                    res.Par = hoyo.Par;
                    res.HcpHombres = hoyo.HcpHombres;
                    res.HcpMujeres = hoyo.HcpMujeres;
                    res.HcpMenores = hoyo.HcpMenores;
                    res.HcpOtros = hoyo.HcpOtros;
                    res.Estado = hoyo.Estado;
                    res.Status = hoyo.Status;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G176Hoyo();
            }
            return res;
        }
    }
}
