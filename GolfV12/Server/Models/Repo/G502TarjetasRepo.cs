using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G502TarjetasRepo : IG502Tarjetas
    {
        private readonly ApplicationDbContext _appDbContext;

        public G502TarjetasRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G502Tarjetas> AddTarjeta(G502Tarjetas tarjeta)
        {
            var res = await _appDbContext.Tarjetas502.AddAsync(tarjeta);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G502Tarjetas>> Filtro(string? clave)
        {
            // clave = org1
            // ejeplo = organizaciones?clave=org1_-_nombre_-_ivan_-_desc_-_conocido
            IQueryable<G502Tarjetas> querry = _appDbContext.Tarjetas502;
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
                case "tar1id":
                    querry = querry.Where(e => e.Id == ParaDic["id"]);
                    break;

                case "tar2id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] &&
                            e.Status == true);
                    break;

                case "tar1creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"]);
                    break;

                case "tar2creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] &&
                            e.Status == true);
                    break;
                case "tar3creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Estado == int.Parse(ParaDic["estado"]) &&
                             e.Status == true).OrderByDescending(e => e.Fecha);
                    break;
                case "tar4creador":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Estado != int.Parse(ParaDic["estado"]) &&
                             e.Status == true).OrderByDescending(e => e.Fecha);
                    break;
                /*
                case "tar1participante":
                    querry = querry.Where(e => (e.Creador == ParaDic["creador"] || e.UnScore.Player == ParaDic["creador"] )
                                && e.Estado == int.Parse(ParaDic["estado"]) && e.Status == true).OrderByDescending(e => e.Fecha);
                    break;
                case "tar2participante":
                    querry = querry.Where(e => (e.Creador == ParaDic["creador"] || e.UnScore.Player == ParaDic["creador"])
                                && e.Estado != int.Parse(ParaDic["estado"]) && e.Status == true).OrderByDescending(e => e.Fecha);
                    break;
                */
                case "tar1participante":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"] && e.Estado == int.Parse(ParaDic["estado"]) && e.Status == true).OrderByDescending(e => e.Fecha);
                    break;
                case "tar2participante":
                    querry = querry.Where(e => e.Creador == ParaDic["creador"]
                                && e.Estado != int.Parse(ParaDic["estado"]) && e.Status == true).OrderByDescending(e => e.Fecha);
                    break;
            }
            return await querry.ToListAsync();
        }

        public async Task<G502Tarjetas> UpdateTarjeta(G502Tarjetas tarjeta)
        {
            var res = await _appDbContext.Tarjetas502.FirstOrDefaultAsync(e => e.Id == tarjeta.Id);
            if (res != null)
            {
                if (tarjeta.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Fecha = tarjeta.Fecha;
                    res.Creador = tarjeta.Creador;
                    res.Campo = tarjeta.Campo;
                    res.Titulo = tarjeta.Titulo;
                    res.Captura = tarjeta.Captura;
                    res.Consulta = tarjeta.Consulta;
                    res.Estado = tarjeta.Estado;
                    res.Status = tarjeta.Status;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G502Tarjetas();
            }
            return res;

        }
    }
}
