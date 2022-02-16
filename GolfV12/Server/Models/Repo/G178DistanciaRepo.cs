using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G178DistanciaRepo : IG178Distancia
    {
        private readonly ApplicationDbContext _appDbContext;

        public G178DistanciaRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G178Distancia> AddDistancia(G178Distancia distancia)
        {
            var res = await _appDbContext.Distancias.AddAsync(distancia);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G178Distancia>> Buscar(int bandera, int hoyoN)
        {
            IQueryable<G178Distancia> querry = _appDbContext.Distancias;
            
            if (bandera > 0) querry = querry.Where(e => e.BanderaId ==  bandera);
            if (hoyoN > 0) querry = querry.Where(e => e.Hoyo == hoyoN);

            return await querry.ToListAsync();
        }

        public async Task<G178Distancia> GetDistancia(int distanciaId)
        {
            var res = await _appDbContext.Distancias.FirstOrDefaultAsync(e => e.Id == distanciaId); 
            return res != null ? res : new G178Distancia();
        }

        public async Task<IEnumerable<G178Distancia>> GetDistancias()
        {
            return await _appDbContext.Distancias.ToListAsync();
        }
        
        public async Task<G178Distancia> UpdateDistancia(G178Distancia distancia)
        {
            var res = await _appDbContext.Distancias.FirstOrDefaultAsync(e => e.Id == distancia.Id);
            if (res != null)
            {
                if (distancia.Status == false)
                {
                    res.Status = false;
                } else
                {
                    res.Fecha = distancia.Fecha;
                    res.BanderaId = distancia.BanderaId;
                    res.Hoyo = distancia.Hoyo;
                    res.Estado = distancia.Estado;
                    res.Status = distancia.Status;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G178Distancia();
            }
            return res;
        }
    }
}
