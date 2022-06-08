using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G320BolitasRepo : IG320Bolitas 
    {
        private readonly ApplicationDbContext _appDbContext;

        public G320BolitasRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G320Bolitas> AddBolitas(G320Bolitas bolita)
        {
            var res = await _appDbContext.Bolitas.AddAsync(bolita);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G320Bolitas>> Filtro(string? clave)
        {
            // clave = bol1id
            // ejeplo = G320Bolitas?clave=bol1id_-_id_-_ñk34_-_desc_-_conocido
            // Id, titulo, creador, grupo, publico
            IQueryable<G320Bolitas> querry = _appDbContext.Bolitas;
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
                case "bol1id":
                    querry = querry.Where(e => e.Id == ParaDic["id"]);
                    break;

                case "bol2id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Status == true);
                    break;

                case "bol3id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Estado == int.Parse(ParaDic["estado"]) &&
                                            e.Status == true);
                    break;

                case "bol1azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"]);
                    break;

                case "bol2azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"] && e.Status == true);
                    break;

                case "bol3azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"] && e.Estado == int.Parse(ParaDic["estado"]) &&
                            e.Status == true);
                    break;
                case "bol1tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"]);
                    break;

                case "bol2tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] &&
                                e.Status == true);
                    break;
                case "bol1Jugador":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"] && e.J1 == ParaDic["j1"] &&
                                e.Status == true);
                    break;
                case "bol2Jugador":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"] && e.J2 == ParaDic["j2"] &&
                                e.Status == true);
                    break;
                case "bol3Jugador":
                    querry = querry.Where(e => (e.J1 == ParaDic["j"] || e.J2 == ParaDic["j2"]) && 
                                e.Azar == ParaDic["azar"] && e.Status == true);
                    break;
                    
            }

            return await querry.ToListAsync();
        }
        public async Task<G320Bolitas> UpdateBolitas(G320Bolitas bolita)
        {
            var res = await _appDbContext.Bolitas.FirstOrDefaultAsync(e => e.Id == bolita.Id);
            if (res != null)
            {
                if (bolita.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Tarjeta = bolita.Tarjeta;
                    res.Azar = bolita.Azar;
                    res.Precio = bolita.Precio;
                    res.J1 = bolita.J1;
                    res.J2 = bolita.J2;
                    
                    res.H1V = bolita.H1V;
                    res.H2V = bolita.H2V;
                    res.H3V = bolita.H3V;
                    res.H4V = bolita.H4V;
                    res.H5V = bolita.H5V;
                    res.H6V = bolita.H6V;
                    res.H7V = bolita.H7V;
                    res.H8V = bolita.H8V;
                    res.H9V = bolita.H9V;
                    res.H10V = bolita.H10V;
                    res.H11V = bolita.H11V;
                    res.H12V = bolita.H12V;
                    res.H13V = bolita.H13V;
                    res.H14V = bolita.H14V;
                    res.H15V = bolita.H15V;
                    res.H16V = bolita.H16V;
                    res.H17V = bolita.H17V;
                    res.H18V = bolita.H18V;

                    res.Estado = bolita.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G320Bolitas();
            }
            return res;
        }
    }
}
