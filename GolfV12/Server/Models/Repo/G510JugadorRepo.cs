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
            Dictionary<string, string> Condiciones = new Dictionary<string, string>();
            
            for (int i = 1; i < parametros.Length; i+=2)
            {
                if (!Condiciones.ContainsKey(parametros[i])) 
                    Condiciones.Add(parametros[i], parametros[i+1].ToString());
            }

            switch (parametros[0])
            {
                case "jug1id":
                    querry = querry.Where(e => e.Id == Condiciones["id"]);
                    break;
                case "jug2id":
                    querry = querry.Where(e => e.Id == Condiciones["id"] &&
                                e.Status == true);
                    break;
                case "jug1tarjeta":
                    querry = querry.Where(e => e.Tarjeta == Condiciones["tarjeta"]);
                    break;
                case "jug2tarjeta":
                    querry = querry.Where(e => e.Tarjeta == Condiciones["tarjeta"] &&
                                e.Status == true);
                    break;
                case "jug1player":
                    querry = querry.Where(e => e.Player == Condiciones["player"]);
                    break;
                case "jug2player":
                    querry = querry.Where(e => e.Player == Condiciones["player"] &&
                                e.Status == true);
                    break;
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
