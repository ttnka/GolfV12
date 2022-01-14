using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G120PlayerRepo : IG120Player
    {
        private readonly ApplicationDbContext _appDbContext;

        public G120PlayerRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G120Player> AddPlayer(G120Player player)
        {
            var newPlayer = await _appDbContext.Players.AddAsync(player);
            await _appDbContext.SaveChangesAsync();
            return newPlayer.Entity;
        }

        public async Task<IEnumerable<G120Player>> Buscar(string org,
            string apodo, string nombre, string paterno, DateTime bday)
        {
            IQueryable<G120Player> querry = _appDbContext.Players;
            if (!string.IsNullOrWhiteSpace(org)) querry = querry.Where(e =>
                e.Organizacion.Clave.Contains(org) || e.Organizacion.Nombre.Contains(org));
            if (!string.IsNullOrWhiteSpace(apodo)) querry = querry.Where(e =>
                 e.Apodo.Contains(apodo));
            if (!string.IsNullOrWhiteSpace(nombre)) querry = querry.Where(e =>
                e.Nombre.Contains(nombre));
            if (!string.IsNullOrWhiteSpace(paterno)) querry = querry.Where(e =>
                e.Paterno.Contains(paterno));
            if (bday > DateTime.MinValue) querry = querry.Where(e => e.Bday.Date == bday.Date);
            return await querry.ToListAsync();
        } 
        public async Task<G120Player> GetPlayer(int playerId)
        {
            var resultado = await _appDbContext.Players.FirstOrDefaultAsync(x => x.Id == playerId);
            //return resultado != null ? resultado : await _appDbContext.Players.FirstOrDefaultAsync(x => x.Id == 1);
            return  resultado != null ? resultado : new G120Player();
        }

        public async Task<IEnumerable<G120Player>> GetPlayers()
        {
            return await _appDbContext.Players.ToListAsync();
        }

        public async Task<G120Player> UpdatePlayer(G120Player player)
        {
            var res = await _appDbContext.Players.FirstOrDefaultAsync(e => e.Id == player.Id);
            if (res != null)
            {
                if (player.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Bday = player.Bday;
                    res.Apodo = player.Apodo;
                    res.Nombre = player.Nombre;
                    res.Paterno = player.Paterno;
                    res.Materno = player.Materno;
                    res.Bday = player.Bday;
                    res.Organizacion = player.Organizacion;
                    res.Nivel = player.Nivel;
                    res.Status = player.Status;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G120Player();
            }
                return res;
        }
    }
}
