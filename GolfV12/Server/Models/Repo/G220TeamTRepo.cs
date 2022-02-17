using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G220TeamTRepo : IG220TeamT
    {
        private readonly ApplicationDbContext _appDbContext;

        public G220TeamTRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G220TeamT> AddTeam(G220TeamT team)
        {
            var res = await _appDbContext.TeamsT.AddAsync(team);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G220TeamT>> Buscar(int teamNum, string? nombre)
        {
            IQueryable<G220TeamT> querry = _appDbContext.TeamsT;
            if (teamNum > 0) querry = querry.Where(e => e.TeamNum == teamNum);
            if (!string.IsNullOrEmpty(nombre)) querry = querry.Where(e => e.Nombre.Contains(nombre));
            
            return await querry.ToListAsync();
        }

        public async Task<G220TeamT> GetTeam(int teamId)
        {
            var res = await _appDbContext.TeamsT.FirstOrDefaultAsync(e => e.Id == teamId);
            return res != null ? res : new G220TeamT();
        }

        public async Task<IEnumerable<G220TeamT>> GetTeams()
        {
            return await _appDbContext.TeamsT.ToListAsync();
        }

        public async Task<G220TeamT> UpdateTeam(G220TeamT team)
        {
            var res = await _appDbContext.TeamsT.FirstOrDefaultAsync(e => e.Id == team.Id);
            if (res != null)
            {
                if (team.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.TeamNum = team.TeamNum;
                    res.Nombre = team.Nombre;
                    res.NumJugadores = team.NumJugadores;
                    res.Estado = team.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G220TeamT();
            }
            return res;
        }
    }
}
