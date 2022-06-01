using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G300AzarRepo : IG300Azar
    {
        private readonly ApplicationDbContext _appDbContext;

        public G300AzarRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G300Azar> AddAzar(G300Azar azar)
        {
            var res = await _appDbContext.Azar.AddAsync(azar);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G300Azar>> Filtro(string? clave)
        {
            // clave = azar1
            // ejeplo = azar?clave=azar1id_-_id_-_ñk34_-_desc_-_conocido
            // Id, titulo, creador, grupo, publico
            IQueryable<G300Azar> querry = _appDbContext.Azar;
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
                case "azar1id":
                    querry = querry.Where(e => e.Id == ParaDic["id"]);
                    break;

                case "azar2id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Status == true);
                    break;

                case "azar3id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Estado == int.Parse(ParaDic["nivel"]) &&
                                            e.Status == true);
                    break;

                case "azar1creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"]);
                    break;

                case "azar2creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Status == true);
                    break;

                case "azar3creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Estado == int.Parse(ParaDic["estado"]) &&
                            e.Status == true);
                    break;
                case "azar1tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"]));
                    break;

                case "azar2tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] &&
                                e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
                case "azar3tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Creador == ParaDic["creador"] &&
                                e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
                    /*
                    case "azar3publico":
                        querry = querry.Where(e => e.Status == Convert.ToBoolean(ParaDic["status"]));
                        break;
                    */
            }

            return await querry.ToListAsync();
        }
        public async Task<G300Azar> UpdateAzar(G300Azar azar)
        {
            var res = await _appDbContext.Azar.FirstOrDefaultAsync(e => e.Id == azar.Id);
            if (res != null)
            {
                if (azar.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Tarjeta = azar.Tarjeta;
                    res.Creador = azar.Creador;
                    res.TipoAzar = azar.TipoAzar;
                    
                    res.Estado = azar.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G300Azar();
            }
            return res;
        }
    }
}
