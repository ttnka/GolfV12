using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G390TiposAzarRepo : IG390TiposAzar 
    {
        private readonly ApplicationDbContext _appDbContext;

        public G390TiposAzarRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G390TiposAzar> AddTiposAzar(G390TiposAzar azarT)
        {
            var res = await _appDbContext.TiposAzar.AddAsync(azarT);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G390TiposAzar>> Filtro(string? clave)
        {
            // clave = azar1
            // ejeplo = azar?clave=azar1id_-_id_-_ñk34_-_desc_-_conocido
            // Id, titulo, creador, grupo, publico
            IQueryable<G390TiposAzar> querry = _appDbContext.TiposAzar;
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
                case "azart1id":
                    querry = querry.Where(e => e.Id == ParaDic["id"]);
                    break;

                case "azart2id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Status == true);
                    break;

                case "azart1creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"]);
                    break;

                case "azart2creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Status == true);
                    break;

                case "azart3creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Publico == Convert.ToBoolean(ParaDic["publico"]) &&
                            e.Status == true);
                    break;
                case "azart4creador":
                    querry = querry.Where(e => (e.Creador == ParaDic["creador"] || e.Publico == true) &&
                            e.Status == true);
                    break;

                case "azart1publico":
                    querry = querry.Where(e => e.Publico == Convert.ToBoolean(ParaDic["publico"]));
                    break;
                case "azart2publico":
                    querry = querry.Where(e => e.Publico == Convert.ToBoolean(ParaDic["publico"]) && e.Status == true);
                    break;

            }

            return await querry.ToListAsync();
        }
        public async Task<G390TiposAzar> UpdateTiposAzar(G390TiposAzar azarT)
        {
            var res = await _appDbContext.TiposAzar.FirstOrDefaultAsync(e => e.Id == azarT.Id);
            if (res != null)
            {
                if (azarT.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Creador = azarT.Creador;
                    res.Titulo = azarT.Titulo;
                    res.Desc = azarT.Desc;
                    res.Indivual = azarT.Indivual;
                    res.Publico = azarT.Publico;
                    res.Estado = azarT.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G390TiposAzar();
            }
            return res;
        }
    }
}
