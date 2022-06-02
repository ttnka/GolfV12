using GolfV12.Server.Data;
using GolfV12.Server.Models.IFace;
using GolfV12.Shared;
using Microsoft.EntityFrameworkCore;

namespace GolfV12.Server.Models.Repo
{
    public class G310ParticipanteRepo : IG310Participante
    {
        private readonly ApplicationDbContext _appDbContext;

        public G310ParticipanteRepo(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }
        public async Task<G310Participantes> AddParticipante(G310Participantes participante)
        {
            var res = await _appDbContext.Participantes.AddAsync(participante);
            await _appDbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<G310Participantes>> Filtro(string? clave)
        {
            // clave = part1
            // ejeplo = participante?clave=part1id_-_id_-_ñk34_-_desc_-_conocido
            // Id, titulo, creador, grupo, publico
            IQueryable<G310Participantes> querry = _appDbContext.Participantes;
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
                case "part1id":
                    querry = querry.Where(e => e.Id == ParaDic["id"]);
                    break;

                case "part2id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Status == true);
                    break;

                case "part3id":
                    querry = querry.Where(e => e.Id == ParaDic["id"] && e.Estado == int.Parse(ParaDic["estado"]) &&
                                            e.Status == true);
                    break;

                case "part1azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"]);
                    break;

                case "part2azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"] && e.Status == true);
                    break;

                case "part3azar":
                    querry = querry.Where(e => e.Azar == ParaDic["azar"] && e.Estado == int.Parse(ParaDic["estado"]) &&
                            e.Status == true);
                    break;
                case "part1tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"]);
                    break;

                case "part2tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] &&
                                e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
                case "part3tarjeta":
                    querry = querry.Where(e => e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"] &&
                                e.Status == Convert.ToBoolean(ParaDic["status"]));
                    break;
                   
                case "part1j1":
                    querry = querry.Where(e => e.J1 == ParaDic["j1"] && e.Tarjeta == ParaDic["tarjeta"] && 
                                e.Status == true );
                        break;
                case "part2j1":
                    querry = querry.Where(e => e.J1 == ParaDic["j1"] && e.Tarjeta == ParaDic["tarjeta"] && 
                                e.Azar == ParaDic["azar"] &&  e.Status == true);
                    break;
                case "part3j1":
                    querry = querry.Where(e => (e.J1 == ParaDic["j"] || e.J2 == ParaDic["j"] ) && 
                                e.Tarjeta == ParaDic["tarjeta"] && e.Status == true);
                    break;
                case "part4j1":
                    querry = querry.Where(e => (e.J1 == ParaDic["j"] || e.J2 == ParaDic["j"]) && 
                            e.Tarjeta == ParaDic["tarjeta"] && e.Azar == ParaDic["azar"] && e.Status == true);
                    break;
                case "part1j2":
                    querry = querry.Where(e => e.J2 == ParaDic["j2"] && e.Tarjeta == ParaDic["tarjeta"] && 
                                e.Status == true);
                    break;
                case "part2j2":
                    querry = querry.Where(e => e.J2 == ParaDic["j2"] && e.Tarjeta == ParaDic["tarjeta"] && 
                                e.Azar == ParaDic["azar"] && e.Status == true);
                    break;
            }

            return await querry.ToListAsync();
        }
        public async Task<G310Participantes> UpdateParticipante(G310Participantes participante)
        {
            var res = await _appDbContext.Participantes.FirstOrDefaultAsync(e => e.Id == participante.Id);
            if (res != null)
            {
                if (participante.Status == false)
                {
                    res.Status = false;
                }
                else
                {
                    res.Tarjeta = participante.Tarjeta;
                    res.Azar = participante.Azar;
                    res.Team = participante.Team;
                    res.J1 = participante.J1;
                    res.J2 = participante.J2;

                    res.Estado = participante.Estado;
                    res.Status = true;
                }
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                res = new G310Participantes();
            }
            return res;
        }
    }
}
