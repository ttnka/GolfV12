using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G522ScoresRepo : IG522Scores
    {
        private readonly ApplicationDbContext _appDbContext;

        public G522ScoresRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G522Scores> AddScore(G522Scores score)
        {
            var res = await _appDbContext.MyScore522.AddAsync(score);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G522Scores>> Filtro(string? clave)
        {
            // clave = sco1
            // ejeplo = score?clave=sco1_-_player_-_abcd_-_hoyo_-_10
            IQueryable<G522Scores> querry = _appDbContext.MyScore522;
            if (string.IsNullOrWhiteSpace(clave) || clave.Length < 14) return await querry.ToListAsync();

            string[] parametros = clave.Split("_-_");
            Dictionary<string, string> ScoresDic = new Dictionary<string, string>();
            for (int i = 1; i < parametros.Length; i += 2)
            {
                if (!ScoresDic.ContainsKey(parametros[i]))
                    ScoresDic.Add(parametros[i], parametros[i + 1]);
            }
            switch (parametros[0])
            {
                case "sco1id":
                    querry = querry.Where(e => e.Id == ScoresDic["id"]);
                    break;
                case "sco2id":
                    querry = querry.Where(e => e.Id == ScoresDic["id"] &&
                                    e.Status == true);
                    break;
                case "sco1tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ScoresDic["tarjeta"]);
                    break;
                case "sco2tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ScoresDic["tarjeta"] &&
                                    e.Status == true);
                    break;
                case "sco1player":
                    querry = querry.Where(e => e.Player == ScoresDic["player"]);
                    break;
                case "sco2player":
                    querry = querry.Where(e => e.Player == ScoresDic["player"] &&
                                    e.Status == true);
                    break;

            }
            return await querry.ToListAsync();
        }

        public async Task<G522Scores> UpdateScore(G522Scores score)
        {
            var res = await _appDbContext.MyScore522.FirstOrDefaultAsync(e => e.Id == score.Id);
            if (res != null)
            {
                if (score.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Tarjeta = score.Tarjeta;
                    res.Categoria = score.Categoria;
                    res.Teams = score.Teams;
                    res.Player = score.Player;
                    res.Hcp = score.Hcp;

                    res.Estado = score.Estado;
                    res.Status = score.Status;
                    res.H1 = score.H1;
                    res.H2 = score.H2;
                    res.H3 = score.H3;
                    res.H4 = score.H4;
                    res.H5 = score.H5;
                    res.H6 = score.H6;
                    res.H7 = score.H7;
                    res.H8 = score.H8;
                    res.H9 = score.H9;
                    res.H10 = score.H10;    
                    res.H11 = score.H11;
                    res.H12 = score.H12;
                    res.H13 = score.H13;
                    res.H14 = score.H14;
                    res.H15 = score.H15;
                    res.H16 = score.H16;    
                    res.H17 = score.H17;
                    res.H18 = score.H18;
                    
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G522Scores();
            }
            return res;
        }
    }
}
