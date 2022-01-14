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

        public async Task<IEnumerable<G128Hcp>> Buscar(int PlayerId, string apodo, 
                                    string nombre, string apellido, string campo)
        {
            IQueryable<G128Hcp> querry = _appDBContext.Hcps;
            if (PlayerId > -1) querry = querry.Where(e => e.Player.Id == PlayerId);
            if(!string.IsNullOrEmpty(apodo)) querry = querry.Where(e => 
                                                    e.Player.Apodo.Contains(apodo)); 
            if(!string.IsNullOrEmpty(nombre)) querry = querry.Where(e =>
                                                    e.Player.Nombre.Contains(nombre));
            if(!string.IsNullOrEmpty(apellido)) querry = querry.Where(e =>
                                                    e.Player.Paterno.Contains(apellido));
            if(!string.IsNullOrEmpty(campo)) querry = querry.Where(e =>
                                                    e.Bandera.Campo.Nombre.Contains(campo));
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
                    res.Player = hcp.Player;
                    res.Fecha = hcp.Fecha;
                    res.Bandera = hcp.Bandera;
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
