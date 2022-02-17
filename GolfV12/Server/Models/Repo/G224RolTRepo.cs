using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G224RolTRepo : IG224RolT
    {
        private readonly ApplicationDbContext _appDbContext;

        public G224RolTRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G224RolT> AddRol(G224RolT rol)
        {
            var res = await _appDbContext.RolsT.AddAsync(rol);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G224RolT>> Buscar(int torneo)
        {
            IQueryable<G224RolT> querry = _appDbContext.RolsT;
            if (torneo > 0 ) querry = querry.Where(e => e.Toreno == torneo);

            return await querry.ToListAsync();
        }

        public async Task<G224RolT> GetRol(int rolId)
        {
            var res = await _appDbContext.RolsT.FirstOrDefaultAsync(e => e.Id == rolId);
            return res != null ? res : new G224RolT();
        }

        public async Task<IEnumerable<G224RolT>> GetRoles()
        {
            return await _appDbContext.RolsT.ToListAsync();
        }

        public async Task<G224RolT> UpdateRol(G224RolT rol)
        {
            var res = await _appDbContext.RolsT.FirstOrDefaultAsync(e => e.Id == rol.Id);
            if (res != null)
            {
                if (rol.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Toreno = rol.Toreno;
                    res.Fecha = rol.Fecha;
                    res.Ronda = rol.Ronda;
                    res.Indice = rol.Indice;
                    res.Team = rol.Team;
                    res.PlayerT = rol.PlayerT;
                    res.Estado = rol.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G224RolT();
            }
            return res;
        }
    }
}
