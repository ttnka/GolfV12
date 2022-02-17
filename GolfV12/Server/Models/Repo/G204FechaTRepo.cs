using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G204FechaTRepo : IG204FechaT
    {
        private readonly ApplicationDbContext _appDbContext;

        public G204FechaTRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G204FechaT> AddFechaT(G204FechaT fechaT)
        {
            var res = await _appDbContext.FechasT.AddAsync(fechaT);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G204FechaT>> Buscar(int torneo, DateTime fecha)
        {
            IQueryable<G204FechaT> querry = _appDbContext.FechasT;
            if (torneo >0 ) querry = querry.Where(e => e.Torneo == torneo);
            if (fecha > DateTime.MinValue) querry = querry.Where(e => e.Fecha.Date == fecha.Date);
         
            return await querry.ToListAsync();
        }

        public async Task<IEnumerable<G204FechaT>> GetFechasT()
        {
            return await _appDbContext.FechasT.ToListAsync();
        }

        public async Task<G204FechaT> GetFechaT(int fechaTId)
        {
            var res = await _appDbContext.FechasT.FirstOrDefaultAsync(e => e.Id == fechaTId);
            return res != null ? res : new G204FechaT();
        }

        public async Task<G204FechaT> UpdateFechaT(G204FechaT fechaT)
        {
            var res = await _appDbContext.FechasT.FirstOrDefaultAsync(e => e.Id == fechaT.Id);
            if (res != null)
            {
                if (fechaT.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Torneo = fechaT.Torneo;
                    res.Ronda = fechaT.Ronda;
                    res.Fecha = fechaT.Fecha;
                    res.Estado = fechaT.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G204FechaT();
            }
            return res;
        }
    }
}
