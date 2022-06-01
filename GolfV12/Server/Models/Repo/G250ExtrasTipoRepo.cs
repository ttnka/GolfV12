using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G250ExtrasTipoRepo : IG250ExtrasTipo
    {
        private readonly ApplicationDbContext _appDbContext;

        public G250ExtrasTipoRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G250ExtrasTipo> AddExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            var res = await _appDbContext.ExtrasTipos.AddAsync(extrasTipo);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }
        
        public async Task<IEnumerable<G250ExtrasTipo>> Filtro(string? clave)
        {
            // clave = exttipo1
            // ejeplo = exttipo?clave=exttipo1_-_nombre_-_ivan_-_desc_-_conocido
            // Id, titulo, creador, grupo, publico
            IQueryable<G250ExtrasTipo> querry = _appDbContext.ExtrasTipos;
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
                case "exttipo1id":
                    querry = querry.Where(e => e.Id == int.Parse(ParaDic["id"]));
                    break;

                case "exttipo2id":
                    querry = querry.Where(e => e.Id == int.Parse(ParaDic["id"]) && e.Status == true);
                    break;

                case "exttipo3id":
                    querry = querry.Where(e => e.Id == int.Parse(ParaDic["id"]) && e.Estado == int.Parse(ParaDic["estado"]) &&
                                            e.Status == true);
                    break;

                case "exttipo1creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"]);
                    break;

                case "exttipo2creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Status == true);
                    break;

                case "exttipo3creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Estado == int.Parse(ParaDic["estado"]) && 
                            e.Status == true);
                    break;
                case "exttipo4creador":
                    querry = querry.Where(e => (e.Creador == ParaDic["creador"] || e.Publico == true) &&
                            e.Status == true);
                    break;
                case "exttipo1publico":
                    querry = querry.Where(e => e.Publico == Convert.ToBoolean(ParaDic["publico"]));
                    break;

                case "exttipo2publico":
                    querry = querry.Where(e => e.Publico == Convert.ToBoolean(ParaDic["publico"]) &&
                                e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
                /*
                case "exttipo3publico":
                    querry = querry.Where(e => e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
                */
            }

            return await querry.ToListAsync();
        }
        public async Task<G250ExtrasTipo> UpdateExtrasTipo(G250ExtrasTipo extrasTipo)
        {
            var res = await _appDbContext.ExtrasTipos.FirstOrDefaultAsync(e => e.Id == extrasTipo.Id);
            if (res != null)
            {
                if (extrasTipo.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Titulo = extrasTipo.Titulo;
                    res.Desc = extrasTipo.Desc;
                    res.Valor = extrasTipo.Valor;
                    res.Creador = extrasTipo.Creador;
                    res.Grupo = extrasTipo.Grupo;
                    res.Publico = extrasTipo.Publico;
                    res.Estado = extrasTipo.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G250ExtrasTipo();
            }
            return res;
        }
    }
}
