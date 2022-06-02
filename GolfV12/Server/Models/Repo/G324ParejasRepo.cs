using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G324ParejasRepo : IG324Parejas 
    {
        private readonly ApplicationDbContext _appDbContext;

        public G324ParejasRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G324Parejas> AddPareja(G324Parejas pareja)
        {
            var res = await _appDbContext.Parejas.AddAsync(pareja);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G324Parejas>> Filtro(string? clave)
        {
            // clave = bol1id
            // ejeplo = G324Parejas?clave=bol1id_-_id_-_ñk34_-_desc_-_conocido
            // Id, titulo, creador, grupo, publico
            IQueryable<G324Parejas> querry = _appDbContext.Parejas;
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
                case "par1id":
                    querry = querry.Where(e => e.Id == ParaDic["id"]);
                    break;

                case "par2id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Status == true);
                    break;

                case "par3id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Estado == int.Parse(ParaDic["estado"]) &&
                                            e.Status == true);
                    break;

                case "par1azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"]);
                    break;

                case "par2azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"] && e.Status == true);
                    break;

                case "par3azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"] && e.Estado == int.Parse(ParaDic["estado"]) &&
                            e.Status == true);
                    break;
                case "par1tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"]);
                    break;

                case "par2tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"] &&
                                e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
                case "par1Jugador":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"] && 
                                e.J1 == ParaDic["j1"] && e.Status == true);
                    break;
                case "par2Jugador":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"] && 
                                e.J2 == ParaDic["j2"] && e.Status == true);
                    break;
                case "par3Jugador":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"] && 
                                e.J3 == ParaDic["j3"] && e.Status == true);
                    break;
                case "par4Jugador":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"] && 
                                e.J4 == ParaDic["j4"] && e.Status == true);
                    break;
                case "par5Jugador":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"] &&
                                (e.J1 == ParaDic["j"] || e.J2 == ParaDic["j"] || e.J3 == ParaDic["j"] || e.J4 == ParaDic["j"]) &&
                                e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
            }

            return await querry.ToListAsync();
        }
        public async Task<G324Parejas> UpdatePareja(G324Parejas pareja)
        {
            var res = await _appDbContext.Parejas.FirstOrDefaultAsync(e => e.Id == pareja.Id);
            if (res != null)
            {
                if (pareja.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Tarjeta = pareja.Tarjeta;
                    res.Azar = pareja.Azar;
                    res.Precio = pareja.Precio;
                    res.J1 = pareja.J1;
                    res.J2 = pareja.J2;
                    res.J3 = pareja.J1;
                    res.J4 = pareja.J2;

                    res.H1V = pareja.H1V;
                    res.H2V = pareja.H2V;
                    res.H3V = pareja.H3V;
                    res.H4V = pareja.H4V;
                    res.H5V = pareja.H5V;
                    res.H6V = pareja.H6V;
                    res.H7V = pareja.H7V;
                    res.H8V = pareja.H8V;
                    res.H9V = pareja.H9V;
                    res.H10V = pareja.H10V;
                    res.H11V = pareja.H11V;
                    res.H12V = pareja.H12V;
                    res.H13V = pareja.H13V;
                    res.H14V = pareja.H14V;
                    res.H15V = pareja.H15V;
                    res.H16V = pareja.H16V;
                    res.H17V = pareja.H17V;
                    res.H18V = pareja.H18V;

                    res.Estado = pareja.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G324Parejas();
            }
            return res;
        }
    }
}
