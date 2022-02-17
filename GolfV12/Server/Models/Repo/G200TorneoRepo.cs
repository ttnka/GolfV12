using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G200TorneoRepo : IG200Torneo
    {
        private readonly ApplicationDbContext _appDbContext;

        public G200TorneoRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G200Torneo> AddTorneo(G200Torneo torneo)
        {
            var res = await _appDbContext.Torneos.AddAsync(torneo);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G200Torneo>> Buscar(int ejercicio, string? titulo, 
            int creador)
        {
            IQueryable<G200Torneo> querry = _appDbContext.Torneos;
            if (ejercicio > 0) querry = querry.Where(e => e.Ejercicio == ejercicio);
            if (!string.IsNullOrEmpty(titulo)) querry = querry.Where(e => e.Titulo.Contains(titulo));
            if (creador > 0) querry = querry.Where(e => e.Creador == creador);

            return await querry.ToListAsync();
        }
        public async Task<G200Torneo> GetTorneo(int torneoId)
        {
            var res = await _appDbContext.Torneos.FirstOrDefaultAsync(e => e.Id == torneoId);
            return res != null ? res : new G200Torneo();
        }
        public async Task<IEnumerable<G200Torneo>> GetTorneos()
        {
            return await _appDbContext.Torneos.ToListAsync();
        }

        public async Task<G200Torneo> UpdateTorneo(G200Torneo torneo)
        {
            var res = await _appDbContext.Torneos.FirstOrDefaultAsync(e => e.Id == torneo.Id);
            if (res != null)
            {
                if (torneo.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Ejercicio = torneo.Ejercicio;
                    res.Titulo = torneo.Titulo;
                    res.Desc = torneo.Desc;
                    res.Creador = torneo.Creador;
                    res.Formato = torneo.Formato;
                    res.Rondas = torneo.Rondas;
                    res.Campo = torneo.Campo;
                    res.TorneoE = torneo.TorneoE;
                    res.TorneoV = torneo.TorneoV;
                    res.Estado = torneo.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G200Torneo();
            }
            return res;
        }
    }
}
