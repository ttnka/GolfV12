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

        /*
        public async Task<IEnumerable<G120Player>> Buscar(string? userId, int org,
            string? apodo, string? nombre, string? paterno)
        {
            IQueryable<G120Player> querry = _appDbContext.Players;

            if (!string.IsNullOrEmpty(userId)) querry = querry.Where(x => x.UserId == userId);
            if (org > -1) querry = querry.Where(e => e.OrganizacionId == org);
            if (!string.IsNullOrWhiteSpace(apodo)) querry = querry.Where(e =>
                 e.Apodo.Contains(apodo));
            if (!string.IsNullOrWhiteSpace(nombre)) querry = querry.Where(e =>
                e.Nombre.Contains(nombre));
            if (!string.IsNullOrWhiteSpace(paterno)) querry = querry.Where(e =>
                e.Paterno.Contains(paterno));
            
            return await querry.ToListAsync();
        } 
        
        public async Task<G120Player> GetPlayer(string userId)
        {
            var resultado = await _appDbContext.Players.FirstOrDefaultAsync(x => x.UserId.Contains(userId));
            //return resultado != null ? resultado : await _appDbContext.Players.FirstOrDefaultAsync(x => x.Id == 1);
            //return  resultado != null ? resultado : new G120Player();
            if (resultado != null) 
            { return resultado; } 
            else 
            {
                return new G120Player(); }
        }
        
        public async Task<IEnumerable<G120Player>> GetPlayers()
        {
            return await _appDbContext.Players.ToListAsync();
        }
        */
        public async Task<IEnumerable<G120Player>> Filtro(string? clave)
        {
            // clave = play1
            // ejeplo = player?clave=org1_-_nombre_-_ivan_-_desc_-_conocido
            IQueryable<G120Player> querry = _appDbContext.Players;
            if (string.IsNullOrEmpty(clave) || clave=="all") return await querry.ToListAsync();
            string[] parametros = clave.Split("_-_");
            Dictionary<string, string> ParaDic = new Dictionary<string, string>();

            for (int i = 1; i < parametros.Length; i += 2)
            {
                if (!ParaDic.ContainsKey(parametros[i]))
                    ParaDic.Add(parametros[i], parametros[i + 1]);
            }

            switch (parametros[0])
            {
                case "play1id":
                    querry = querry.Where(e => e.UserId == ParaDic["userid"]);
                    break;

                case "play2id":
                    querry = querry.Where(e => e.UserId == ParaDic["userid"] && e.Status == true);
                    break;

                case "play3id":
                    querry = querry.Where(e => e.UserId == ParaDic["userid"] && e.Nivel == (Niveles)Enum.Parse(typeof(Niveles), ParaDic["nivel"]) &&
                       e.Status == true);
                    break;

                case "play1nombre":
                    querry = querry.Where(e => e.Nombre == ParaDic["nombre"]);
                    break;

                case "play2nombre":
                    querry = querry.Where(e => e.Nombre == ParaDic["nombre"] && e.Paterno == ParaDic["paterno"]);
                    break;

                case "play3nombre":
                    querry = querry.Where(e => e.Nombre == ParaDic["nombre"] && e.Paterno == ParaDic["paterno"] && e.Materno == ParaDic["materno"]);
                    break;

                case "play1nivel":
                    querry = querry.Where(e => e.Nivel == (Niveles)Enum.Parse(typeof(Niveles), ParaDic["nivel"]));
                    break;

                case "play2nivel":
                    querry = querry.Where(e => e.Nivel == (Niveles)Enum.Parse(typeof(Niveles), ParaDic["nivel"]) &&
                       e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;

                case "play1status":
                    querry = querry.Where(e => e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
            }

                    return await querry.ToListAsync();
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
                    
                    res.Apodo = player.Apodo;
                    res.Nombre = player.Nombre;
                    res.Paterno = player.Paterno;
                    res.Materno = player.Materno;
                    res.Bday = player.Bday;
                    res.OrganizacionId = player.OrganizacionId;
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
