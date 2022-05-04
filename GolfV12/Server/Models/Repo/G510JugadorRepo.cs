using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G510JugadorRepo : IG510Jugador
    {
        private readonly ApplicationDbContext _appDbContext;

        public G510JugadorRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G510Jugador> AddJugador(G510Jugador jugador)
        {
            var res = await _appDbContext.Jugadores.AddAsync(jugador);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G510Jugador>> Filtro(string? clave)
        {
            // clave = jug1
            // ejeplo = jugador?clave=jug1_-_player_-_ivan_-_desc=conocido
            IQueryable<G510Jugador> querry = _appDbContext.Jugadores;
            if (string.IsNullOrWhiteSpace(clave)) return await querry.ToListAsync();

            string[] parametros = clave.Split("_-_");
            string Data1 = string.Empty;
            string Data2 = string.Empty;

            if (parametros[0] == "jug1")
            {
                for (int i = 1; i < parametros.Length; i++)
                {
                    Data1 = parametros[i];
                    Data2 = parametros[i + 1];
                    switch (Data1)
                    {
                        case "id":
                            querry = querry.Where(e => e.Id.Contains(Data2));
                            break;
                        case "tarjeta":
                            querry = querry.Where(e => e.Tarjeta.Contains(Data2));
                            break;
                        case "player":
                            querry = querry.Where(e => e.Player.Contains(Data2));
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

        public async Task<G510Jugador> UpdateJugador(G510Jugador jugador)
        {
            var res = await _appDbContext.Jugadores.FirstOrDefaultAsync(e => e.Id == jugador.Id);
            if (res != null)
            {
                if (jugador.Status == false)
                {
                    res.Tarjeta = jugador.Tarjeta;
                    res.Status = false;
                }
                else
                {
                    res.Tarjeta = jugador.Tarjeta;
                    res.Player = jugador.Player;
                    res.Estado = jugador.Estado;
                    res.Status = jugador.Status;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G510Jugador();
            }
            return res;
        }
    }
}
