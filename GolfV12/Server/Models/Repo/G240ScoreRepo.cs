using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G240ScoreRepo : IG240Score
    {
        private readonly ApplicationDbContext _appDbContext;

        public G240ScoreRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G240Score> AddScore(G240Score score)
        {
            var res = await _appDbContext.Scores.AddAsync(score);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G240Score>> Buscar(int rol, int player, int hoyo)
        {
            IQueryable<G240Score> querry = _appDbContext.Scores;
            if (rol > 0) querry = querry.Where(e => e.Rol == rol);
            if (player > 0 ) querry = querry.Where(e => e.Player == player);
            if (hoyo > 0) querry = querry.Where(e => e.Hoyo == hoyo);

            return await querry.ToListAsync();
        }

        public async Task<G240Score> GetScore(int scoreId)
        {
            var res = await _appDbContext.Scores.FirstOrDefaultAsync(e => e.Id == scoreId);
            return res != null ? res : new G240Score();
        }

        public async Task<IEnumerable<G240Score>> GetScores()
        {
            return await _appDbContext.Scores.ToListAsync();
        }

        public async Task<G240Score> UpdateScore(G240Score score)
        {
            var res = await _appDbContext.Scores.FirstOrDefaultAsync(e => e.Id == score.Id);
            if (res != null)
            {
                if (score.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Rol = score.Rol;
                    res.Player = score.Player;
                    res.Hoyo = score.Hoyo;
                    res.Score = score.Score;
                    res.Estado = score.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G240Score();
            }
            return res;
        }
    }
}
