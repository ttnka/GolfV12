using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G176HoyoRepo : IG176Hoyo
    {
        private readonly ApplicationDbContext _appDbContext;

        public G176HoyoRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public async Task<G176Hoyo> AddHoyo(G176Hoyo hoyo)
        {
            var res = await _appDbContext.Hoyos.AddAsync(hoyo);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G176Hoyo>> Filtro(string? clave)
        {
            // clave = Hoy1
            // ejeplo = G176Hoyo/filtro?clave=hoy1id_-_id_-_1_-_campo_-_2
            IQueryable<G176Hoyo> querry = _appDbContext.Hoyos;
            if (string.IsNullOrWhiteSpace(clave) || clave.Count() < 10) return await querry.ToListAsync();

            string[] parametros = clave.Split("_-_");
            Dictionary<string, string> ParaDic = new Dictionary<string, string>();

            for (int i = 1; i < parametros.Length; i += 2)
            {
                if (!ParaDic.ContainsKey(parametros[i]))
                    ParaDic.Add(parametros[i], parametros[i + 1]);
            }

            switch (parametros[0])
            {
                case "hoy1id":
                    querry = querry.Where(e => e.Id == int.Parse(ParaDic["id"]));
                    break;

                case "hoy2id":
                    querry = querry.Where(e => e.Id == int.Parse(ParaDic["id"]) &&
                            e.Status == true);
                    break;

                case "hoy1campo":
                    querry = querry.Where(e => e.CampoId == int.Parse(ParaDic["campo"]));
                    break;

                case "hoy2campo":
                    querry = querry.Where(e => e.CampoId == int.Parse(ParaDic["campo"]) &&
                            e.Status == true);
                    break;
                /*
                case "hoy3hoyo":
                    querry = querry.Where(e => e.CampoId == int.Parse(ParaDic["campo"]) && e.Hoyo == int.Parse(ParaDic["hoyo"]) &&
                             e.Status == true).OrderByDescending(e => e.Fecha);
                    break;
                case "tar4creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Estado != int.Parse(ParaDic["estado"]) &&
                             e.Status == true).OrderByDescending(e => e.Fecha);
                    break;
                */
            }
            return await querry.ToListAsync();
        }

        /*
        public async Task<IEnumerable<G176Hoyo>> Buscar(int campo, string? ruta, int hoyoN)
        {
            IQueryable<G176Hoyo> querry = _appDbContext.Hoyos;
            if (campo > 0) querry = querry.Where(e => e.CampoId == campo);
            if (!string.IsNullOrEmpty(ruta)) querry = querry.Where(e => e.Ruta.Contains(ruta));
            if (hoyoN > 0) querry = querry.Where(e => e.Hoyo == hoyoN);

            return await querry.ToListAsync();
        }

        public async Task<G176Hoyo> GetHoyo(int hoyoId)
        {
            var res = await _appDbContext.Hoyos.FirstOrDefaultAsync(e => e.Id == hoyoId);
            return res != null ? res : new G176Hoyo();
        }

        public async Task<IEnumerable<G176Hoyo>> GetHoyos()
        {
            return await _appDbContext.Hoyos.ToListAsync(); 
        }
        */
        public async Task<G176Hoyo> UpdateHoyo(G176Hoyo hoyo)
        {
            var res = await _appDbContext.Hoyos.FirstOrDefaultAsync(e =>e.Id == hoyo.Id);
            if (res != null)
            {
                if (hoyo.Status == false)
                {
                    res.Status = false;
                } else
                {
                    res.CampoId = hoyo.CampoId;
                    res.Ruta = hoyo.Ruta;
                    res.Hoyo = hoyo.Hoyo;
                    res.Par = hoyo.Par;
                    res.HcpHombres = hoyo.HcpHombres;
                    res.HcpMujeres = hoyo.HcpMujeres;
                    res.HcpMenores = hoyo.HcpMenores;
                    res.HcpOtros = hoyo.HcpOtros;
                    res.Estado = hoyo.Estado;
                    res.Status = hoyo.Status;
                }
                await _appDbContext.SaveChangesAsync();
            } else
            {
                res = new G176Hoyo();
            }
            return res;
        }
    }
}
