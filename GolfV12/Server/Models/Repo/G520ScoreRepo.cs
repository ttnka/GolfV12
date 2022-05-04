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
            string Data1 = string.Empty;
            string Data2 = string.Empty;

            if (parametros[0] == "sco1")
            {
                for (int i = 1; i < parametros.Length; i++)
                {
                    Data1 = parametros[i];
                    Data2 = parametros[i + 1];
                    switch (Data1)
                    {
                        case "id":
                            querry = querry.Where(e => e.Id == Data2);
                            break;

                        case "tarjeta":
                            querry = querry.Where(e => e.Tarjeta == Data2);
                            break;

                        case "campo":
                            querry = querry.Where(e => e.Campo == int.Parse(Data2));
                            break;

                        case "player":
                            querry = querry.Where(e => e.Player == Data2);
                            break;

                        case "publico":
                            querry = querry.Where(e => e.Publico.Equals(Data2));
                            break;

                        case "hoyo":
                            querry = querry.Where(e => e.Hoyo == int.Parse(Data2));
                            break;

                        case "Estado":
                            querry = querry.Where(e => e.Estado == int.Parse(Data2));
                            break;

                        case "Status":
                            querry = querry.Where(e => e.Status.Equals(Data2));
                            break;
                    }
                    return await querry.ToListAsync();
                }
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
