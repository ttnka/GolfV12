using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G520ScoreRepo : IG520Score
    {
        private readonly ApplicationDbContext _appDbContext;

        public G520ScoreRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G520Score> AddScore(G520Score score)
        {
            var res = await _appDbContext.MyScore.AddAsync(score);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G520Score>> Filtro(string? clave)
        {
            // clave = sco1
            // ejeplo = score?clave=sco1_-_player_-_abcd_-_hoyo_-_10
            IQueryable<G520Score> querry = _appDbContext.MyScore;
            if (string.IsNullOrWhiteSpace(clave) || clave.Length < 14 ) return await querry.ToListAsync();

            string[] parametros = clave.Split("_-_");
            Dictionary<string, string> ScoresDic = new Dictionary<string, string>();
            for (int i = 1; i < parametros.Length; i+=2)
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
                case "sco3tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ScoresDic["tarjeta"] &&
                                    e.Status == true).OrderBy(e => e.Hoyo);
                   // querry = querry.OrderBy(e => e.Hoyo);
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

        public async Task<G520Score> UpdateScore(G520Score score)
        {
            var res = await _appDbContext.MyScore.FirstOrDefaultAsync(e => e.Id == score.Id);
            if (res != null)
            {
                if (score.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Tarjeta = score.Tarjeta;
                    res.Campo = score.Campo;
                    res.Player = score.Player;                    
                    res.Hcp = score.Hcp;
                    res.Publico = score.Publico;
                    res.Hoyo = score.Hoyo;
                    res.Score = score.Score;
                    res.Estado = score.Estado;
                    res.Status = score.Status;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G520Score();
            }
            return res;
        }
    }
}
