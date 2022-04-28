using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G222PlayerTRepo : IG222PlayerT
    {
        private readonly ApplicationDbContext _appDbContext;

        public G222PlayerTRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G222PlayerT> AddPlayer(G222PlayerT player)
        {
            var res = await _appDbContext.PlayersT.AddAsync(player);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G222PlayerT>> Buscar(int team, string? player)
        {
            IQueryable<G222PlayerT> querry = _appDbContext.PlayersT;
            if (team > 0 ) querry = querry.Where(e => e.Team == team);
            if (!string.IsNullOrEmpty(player) ) querry = querry.Where(e => e.Player.Contains(player));

            return await querry.ToListAsync();
        }

        public async Task<G222PlayerT> GetPlayer(int playerId)
        {
            var res = await _appDbContext.PlayersT.FirstOrDefaultAsync(e => e.Id == playerId);
            return res != null ? res : new G222PlayerT();
        }

        public async Task<IEnumerable<G222PlayerT>> GetPlayers()
        {
            return await _appDbContext.PlayersT.ToListAsync();
        }

        public async Task<G222PlayerT> UpdatePlayer(G222PlayerT player)
        {
            var res = await _appDbContext.PlayersT.FirstOrDefaultAsync(e => e.Id == player.Id);
            if (res != null)
            {
                if (player.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Team = player.Team;
                    res.Player = player.Player;
                    res.Hcp = player.Hcp;
                    res.Estado = player.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G222PlayerT();
            }
            return res;
        }
    }
}
