using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G242ExtrasRepo : IG242Extras
    {
        private readonly ApplicationDbContext _appDbContext;

        public G242ExtrasRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G242Extras> AddExtra(G242Extras extra)
        {
            var res = await _appDbContext.Extras.AddAsync(extra);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }
        /*
        public async Task<IEnumerable<G242Extras>> Buscar(int rol, string? player, int hoyo, int tipoExtra)
        {
            IQueryable<G242Extras> querry = _appDbContext.Extras;
            if (rol > 0) querry = querry.Where(e => e.Rol == rol);
            if (!string.IsNullOrEmpty(player)) querry = querry.Where(e => e.Player.Contains(player));
            if (tipoExtra > 0) querry = querry.Where(e => e.TipoExtra == tipoExtra);

            return await querry.ToListAsync();
        }

        public async Task<G242Extras> GetExtra(int extraId)
        {
            var res = await _appDbContext.Extras.FirstOrDefaultAsync(e => e.Id == extraId);
            return res != null ? res : new G242Extras();
        }

        public async Task<IEnumerable<G242Extras>> GetExtras()
        {
            return await _appDbContext.Extras.ToListAsync();
        }
        */
        public async Task<IEnumerable<G242Extras>> Filtro(string? clave)
        {
            // clave = ext1
            // ejeplo = ext?clave=exttipo1_-_nombre_-_ivan_-_desc_-_conocido
            // Id, tarjeta, player, hoyo, tipoextra
            IQueryable<G242Extras> querry = _appDbContext.Extras;
            if (string.IsNullOrEmpty(clave) || clave == "all") return await querry.ToListAsync();
            string[] parametros = clave.Split("_-_");

            Dictionary<string, string> ParaDic = new Dictionary<string, string>();

            for (int i = 1; i < parametros.Length; i += 2)
            {
                if (!ParaDic.ContainsKey(parametros[i]))
                    ParaDic.Add(parametros[i], parametros[i + 1]);
            }

            switch (parametros[0])
            {
                case "ext1id":
                    querry = querry.Where(e => e.Id == int.Parse(ParaDic["id"]));
                    break;

                case "ext2id":
                    querry = querry.Where(e => e.Id == int.Parse(ParaDic["id"]) && e.Status == true);
                    break;

                case "ext1tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"]);
                    break;

                case "ext2tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Status == true);
                    break;

                case "ext3tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.TipoExtra == int.Parse(ParaDic["tipoextra"]) &&
                            e.Status == true);
                    break;
                /*
                case "exttipo1publico":
                    querry = querry.Where(e => e.Publico == Convert.ToBoolean(ParaDic["publico"]));
                    break;

                case "exttipo2publico":
                    querry = querry.Where(e => e.Publico == Convert.ToBoolean(ParaDic["publico"]) &&
                                e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
               
                    case "exttipo3publico":
                        querry = querry.Where(e => e.Status == Convert.ToBoolean(ParaDic["status"]));
                        break;
                    */
            }
            return await querry.ToListAsync();
        }

            public async Task<G242Extras> UpdateExtra(G242Extras extra)
        {
            var res = await _appDbContext.Extras.FirstOrDefaultAsync(e => e.Id == extra.Id);
            if (res != null)
            {
                if (extra.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Rol = extra.Rol;
                    res.Player = extra.Player;
                    res.Hoyo = extra.Hoyo; 
                    res.TipoExtra = extra.TipoExtra;
                    res.Valor = extra.Valor;
                    res.Estado = extra.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G242Extras();
            }
            return res;
        }
    }
}
